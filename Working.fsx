#time "on"

#r "nuget: Akka.FSharp"
#r "nuget: Akka.TestKit"
open System
open Akka.Actor
open Akka.Configuration
open Akka.FSharp
open Akka.TestKit
open System.Collections.Generic



let system = ActorSystem.Create("FSharp")



let bigInt (x: int) = bigint (x)
let args : string array = fsi.CommandLineArgs |> Array.tail
let N =  args.[0] |> float
let k = args.[1] |> float |> int |> bigint



let numOfCores = 80 * Environment.ProcessorCount |> float

let worRange = N / numOfCores |> float |> ceil |> int |> bigInt
let numOfActors = numOfCores |> int
let n= N |> int |> bigint




type ActorCreator(startNum: bigint, endNum: bigint, k: bigint, caller: IActorRef) =
    inherit Actor()
    do
        let mutable ansList: bigint list = []
        for i in [ startNum .. endNum ] do
            let y = i + k - bigint(1)
            let sum1 = (y * (y + bigint (1))*((bigint (2) * y) + bigint (1)))

            let sum = sum1 / bigint (6)
            let un = i - bigint(1)
            let unSum1 =(un*(un + bigint (1))* ((bigint (2) * un) + bigint (1)))
           
            let unSum = unSum1 / bigint (6)
            let realSum = sum - unSum |> float
            let sqrRoot = sqrt realSum
            let roundRoot = sqrRoot |> floor

            if sqrRoot - roundRoot = 0.0 then
                
                let temp = [ i ]
                ansList <- ansList @ temp
        
        ansList |> List.iter (fun x -> printfn "%A" x)
        caller.Tell("done")
        ()

    override x.OnReceive message = ()



let BossActor =
    spawn system "EchoServer"
    <| fun mailbox ->
        let t = 0
        let mutable numActors = 0

        let rec spawnChild (startNum: bigint, workRange: bigint, k: bigint, numOfActors: bigint, n: bigint) =
            if (numOfActors = bigint(1) || startNum + workRange - bigint(1) > n) then
                let properties =
                    [| startNum :> obj;n :> obj;k :> obj;mailbox.Context.Self :> obj |]

                let actorRef =
                    system.ActorOf(Props(typedefof<ActorCreator>, properties))

                numActors <- numActors + 1
                ()
            else
                let properties =
                    [| startNum :> obj;startNum + workRange - bigint(1) :> obj ;k :> obj;mailbox.Context.Self :> obj |]

                let actorRef =
                    system.ActorOf(Props(typedefof<ActorCreator>, properties))

                numActors <- numActors + 1
                spawnChild (startNum + workRange, workRange, k, numOfActors - bigint(1), n)

        spawnChild (bigint(1), worRange, k, bigint(25), n)
        let rec loop () =
            actor {
                let! message = mailbox.Receive()
                // printfn "%A" mailbox.Log
                match box message with
                | :? string as msg ->
                    numActors <- numActors - 1
                    if (numActors > 0) then return! loop () else ()
                | _ -> failwith "unknown message"
            }

        loop ()





system.Terminate()