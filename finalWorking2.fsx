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



// let numOfCores = 80 * Environment.ProcessorCount |> float

let worRange = N / 25.0 |> float |> ceil |> int |> bigInt
// let numOfActors = numOfCores |> int
let n= N |> int |> bigint




type ActorCreator(startNum: bigint, endNum: bigint, k: bigint, caller: IActorRef) =
    inherit Actor()
    do
        // let mutable ansList: int list = []
        // let mutable sqSum = 0 |> bigInt
        // let mutable sqrRoot = 0 |> double
        let mutable temp = 1 |> bigInt
        let mutable st = 1|> bigint
        let mutable res = 0 |> bigInt 
        for i in [startNum..endNum] do
            let startPlusWindow = (i+k-bigInt(1))
            for j in [i..startPlusWindow] do
                    temp <- (j*j)
                    res <- res + temp
            while(st*st <=(res)) do
                if(st*st = res) then
                    printfn "%A \n" (startPlusWindow-k + bigint(1))

                st<- st + bigint(1)
            res <- bigint(0) 
            // printfn "SqSum %i" sqSum
            // let sumDouble = sqSum |> double        
            // sqrRoot <- sqrt sumDouble 
            // match ((sqrRoot%1.0)<=0.0) with 
            // | true -> printfn "%A" i
            // | false -> ()  
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