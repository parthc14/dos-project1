#time "on"
#r "nuget: Akka.FSharp"
#r "nuget: Akka.TestKit"
// #load "Bootstrap.fsx"

open System
open Akka.Actor
open Akka.Configuration
open Akka.FSharp
open Akka.TestKit

// #Using Actor
// Actors are one of Akka's concurrent models.
// An Actor is a like a thread instance with a mailbox.
// It can be created with system.ActorOf: use receive to get a message, and <! to send a message.
// This example is an EchoServer which can receive messages then print them.


let system = System.create "system" <| Configuration.load ()
let n = 40 |> float
let k = 24 |> float
let noOfCores = Environment.ProcessorCount |> float
let noActors = 10.0 * noOfCores |> float
let workRange =  n / noActors |> float |> ceil |>int

let bossActor(n: float)(k:float)(workRange:int)(mailBox: Actor<_>) =
    let rec loop() = actor{
        let! message = mailBox.Receive()
        printfn "%s N: %f K: %f workRange : %i" message n k workRange
        return! loop()
    }
    loop()

let bossActorRef = spawn system "bossActor" <| bossActor n k workRange
[1..10] |> List.iter(fun i-> bossActorRef <! sprintf "Need to create Actors here! %i" i)


// let alternatingRouter (processor1: IActorRef) (processor2: IActorRef) (mailbox: Actor<_>) =
//     let rec loop alternate = actor {
//         let alternateProcessor () = if alternate = 1 then processor1, 2 else processor2, 1
//         let! message = mailbox.Receive ()
//         let processor, nextAlternate = alternateProcessor ()
//         printfn "AlternatingRouter: routing %O to %s" message processor.Path.Name
//         processor <! message
//         return! loop nextAlternate
//     }
//     loop 1


// let processor (mailbox: Actor<_>) =
//     let rec loop () = actor {
//         let! message = mailbox.Receive ()
//         printfn "Processor: %s received %O"  message mailbox.Self.Path.Name
//         return! loop ()
//     }
//     loop ()

// let processor1Ref = spawn system "processor1" processor
// let processor2Ref = spawn system "processor2" processor
// let alternatingRouterRef = spawn system "alternatingRouter" <| alternatingRouter processor1Ref processor2Ref

// [1..10] |> List.iter (fun i -> alternatingRouterRef <! sprintf "Message #%i" i)

