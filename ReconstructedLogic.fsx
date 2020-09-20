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


let arguments = Environment.GetCommandLineArgs()
let bigInt (x: int) = bigint (x)
// for i in arguments do
//     printfn "%s" i
// let n = arguments.[3] |> float
let n = 10000000.0
// let k = arguments.[4] |> int
let k = 24 |> bigint
let numOfCores = 10 * Environment.ProcessorCount |> float
// let numOfCores = 80 |> float // for testing purpose only
let worRange = n / numOfCores |> float |> ceil |> int |> bigInt
let numOfActors = numOfCores |> int
let N= n |> int |> bigint
//printfn "workrange %d" worRange



type Worker(startNum: bigint, endNum: bigint, k: bigint, caller: IActorRef) =
    inherit Actor()
    do
        let mutable ansList: int list = []
        let mutable sqSum = 0 |> bigInt
        let mutable sqrRoot = 0 |> double
        for i in [startNum..endNum] do
            sqSum <-bigInt(0)
            for j in [i..(i+k-bigInt(1))] do
                    let a = j 
                    sqSum <- (a*a) + sqSum
            // printfn "SqSum %i" sqSum
            let sumDouble = sqSum |> double        
            sqrRoot <- sqrt sumDouble 
            match ((sqrRoot%1.0)<=0.0) with 
            | true -> printfn "%A" i
            | false -> ()    
                // printfn "ans %i" 23660
                // let temp = [ i ]
                // ansList <- ansList @ temp
        // ansList.Add(i)
        // printfn "...   %A   " ansList
        // ansList |> List.iter (fun x -> printfn "%d" x)
        caller.Tell("done")
        ()

    override x.OnReceive message = ()
// let mutable b = new List<int>()
// let a = new List<int>()
// let mutable b:List<int>
let mutable a: int list = []
// let b: int list = []
// a @ b

// System.Object.ReferenceEquals( a, b )
// let l = List.empty<string>
// Parent actor
let SuperVisor1 =
    spawn system "EchoServer"
    <| fun mailbox ->
        let t = 0
        let mutable numActors = 0

        let rec createChild (startNum: bigint, workRange: bigint, k: bigint, numOfActors: bigint, n: bigint) =
            if (numOfActors = bigint(1) || startNum + workRange - bigint(1) > n) then
                let properties =
                    [| startNum :> obj;n :> obj;k :> obj;mailbox.Context.Self :> obj |]

                let actorRef =
                    system.ActorOf(Props(typedefof<Worker>, properties))

                numActors <- numActors + 1
                ()
            else
                let properties =
                    [| startNum :> obj;startNum + workRange - bigint(1) :> obj ;k :> obj;mailbox.Context.Self :> obj |]

                let actorRef =
                    system.ActorOf(Props(typedefof<Worker>, properties))

                numActors <- numActors + 1
                createChild (startNum + workRange, workRange, k, numOfActors - bigint(1), n)

        createChild (bigint(1), worRange, k, bigint(25), N)
        // let mutable a = []
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