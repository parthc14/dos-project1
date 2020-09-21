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

let mutable flag = true

let bigInt (x: int) = bigint (x)
let args : string array = fsi.CommandLineArgs |> Array.tail
let N =  args.[0] |> float
let k = args.[1] |> float |> int |> bigint



// let numOfCores = 80 * Environment.ProcessorCount |> float

let worRange = N / 30.0 |> float |> ceil |> int |> bigInt
// let numOfActors = numOfCores |> int
let n= N |> int |> bigint




type ActorCreator(startNum: bigint, endNum: bigint, k: bigint, caller: IActorRef) =
    inherit Actor()
    do
        
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
        caller.Tell("done")
        ()

    override x.OnReceive message = ()



let BossActor =
    spawn system "EchoServer"
    <| fun mailbox ->
      
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

        spawnChild (bigint(1), worRange, k, bigint(30), n)
        let rec loop () =
            actor {
                let! message = mailbox.Receive()
                match box message with
                | :? string as msg ->
                    numActors <- numActors - 1
                    if (numActors > 0) then return! loop () else 
                    flag <- false
                    ()
                | _ -> failwith "unknown message"
            }

        loop ()

while flag do
    ignore



system.Terminate()|>ignore