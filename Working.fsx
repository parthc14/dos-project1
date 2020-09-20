#time "on"
#r "nuget: Akka.FSharp"
#r "nuget: Akka.TestKit"
open System
open Akka.FSharp

let system = System.create "system" <| Configuration.load()

let myActor (mailbox: Actor<_>) =
    let rec loop() = actor {
        let! message = mailbox.Receive()
        printfn "The message recieved is %s" message
        return! loop ()
    }
    loop()


let actorRef = spawn system "myActor" myActor

actorRef <! "Learning F#"

