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


let args : string array = fsi.CommandLineArgs |> Array.tail
let n =  args.[0] |> float
let k = args.[1] |> int
let bigInt (x: int) = bigint (x)
let numOfCores = 10 * Environment.ProcessorCount |> float
let workRange = n / numOfCores |> float |> ceil |> int
let numOfActors = numOfCores |> int
let N = n |> int


type ActorSpawn(startNum: int, endNum: int, k: int, caller: IActorRef) =
    inherit Actor()
    do
        // let mutable ansList: int list = []
        for i in [ startNum .. endNum ] do
            let y = i + k - 1
            let y' = y |> bigInt
            let sum1 =(y'* (y' + bigint (1))*((bigint (2) * y') + bigint (1)))
            let sum = sum1 / bigint (6)
            let un = i - 1
            let un' = un |> bigInt
            let unSum =(un'* (un' + bigint (1))* ((bigint (2) * un') + bigint (1)))
            let unSum' = unSum / bigint (6)
            let realSum = sum - unSum' |> float
            let sqrRoot = sqrt realSum
            let roundRoot = sqrRoot |> floor
            if sqrRoot - roundRoot = 0.0 then
                // printfn "ans %i" 23660
                printfn "%i" i 
        //         let temp = [ i ]
        //         ansList <- ansList @ temp
        // ansList |> List.iter (fun x -> printfn "%d" x)
        caller.Tell("Ok, done")
        ()

    override x.OnReceive message = ()

let mutable a: int list = []

let SuperVisor1 =
    spawn system "EchoServer"
    <| fun mailbox ->
        let mutable numActors = 0
        let rec createActors (startNum: int, workRange: int, k: int, numOfActors: int, n: int) =
            if (numOfActors = 1 || startNum + workRange - 1 > n) then
                let properties =[| startNum :> obj;n :> obj;k :> obj;mailbox.Context.Self :> obj |]
                let actorRef = system.ActorOf(Props(typedefof<ActorSpawn>, properties))
                numActors <- numActors + 1
                ()
            else
                let properties =[| startNum :> obj;startNum + workRange - 1 :> obj;k :> obj;mailbox.Context.Self :> obj |]
                let actorRef = system.ActorOf(Props(typedefof<ActorSpawn>, properties))
                numActors <- numActors + 1
                createActors (startNum + workRange, workRange, k, numOfActors - 1, n)

        createActors (1, workRange, k, 25, N)
        let rec loop () =
            actor {
                let! message = mailbox.Receive()
                match box message with
                | :? string as msg ->
                    numActors <- numActors - 1
                    if (numActors > 0) then return! loop () else ()
                | _ -> failwith "unknown message"
            }

        loop ()
system.Terminate()