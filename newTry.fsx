#time "on"
#r "nuget: Akka.FSharp"
#r "nuget: Akka.TestKit"
// #load "Bootstrap.fsx"

open System
open Akka.Actor
open Akka.Configuration
open Akka.FSharp
open Akka.TestKit
// let system = ActorSystem.Create("FSharp")


type ActorMsg =
    | Greet of string
    | Hi

let system = System.create "MySystem" <| ConfigurationFactory.Default()
let greeter =
    spawn system "Greeter"
    <| fun mailbox ->
        let rec loop() = actor {
            let! msg = mailbox.Receive()
            match msg with
            | Greet name -> printfn "Hello %s" name
            | Hi         -> printfn "Hi"
            return! loop() }
        loop()

greeter <! Greet "Alex"
greeter <! Hi