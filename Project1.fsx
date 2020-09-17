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

let system = ActorSystem.Create("FSharp")

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

type EchoServer(id, startNum, endNum, k) =
    inherit Actor()
        
    override x.OnReceive message =
        for x in [startNum..endNum] do
            let y = x+k - 1
            let sum1 = (y *(y+1)  *((2*y)+1))                                    
            let sum = sum1 / 6 |> float
            let un = x-1
            let unSum = (un * (un+1) * ((2*un)+1))                            
            let unwantedSum = unSum / 6 |>float
            let realSum =  sum - unwantedSum                                  
            let sqrRoot = sqrt realSum                                 
            let roundSqrt = floor sqrRoot |> float                  
            if sqrRoot-roundSqrt = 0.0 then                               
                printfn "%i" x

        match message with
        | :? string as msg ->  () //printfn "ID : %i startsNum %i endNum %i k %i " id startNum endNum k  
        | _ ->  failwith "unknown message"

let n = 40 |> float
let k = 24 |> float
let noOfCores = Environment.ProcessorCount |> float
let noActors = 10.0 * noOfCores |> float
let workRange =  n / noActors |> float |> ceil |>int



let intActors = noActors |>int

let allActors = 
    [1 .. intActors-1]
    |> List.map(fun id ->   let properties = [|  int(id):> obj; int(0 + (id-1)* workRange + 1):>obj; int(0 + (id)*workRange) :> obj ;int(k) :> obj |]
                            system.ActorOf(Props(typedefof<EchoServer>, properties)))
            
            
let finalActor = 
    [intActors-1 .. intActors]
    |> List.map(fun id ->   let properties = [|  int(intActors):>obj; int(0 + (intActors - 1)*workRange + 1) :> obj ;int(n) :> obj; (int)(k):> obj |]
                            system.ActorOf(Props(typedefof<EchoServer>, properties)))

let finalList = allActors @ finalActor   


for id in [ 1 .. intActors] do
    List.item (id) finalList
    <! sprintf "F# %d!" id


system.Terminate()
// et list = createWorkder newCore t workRange k n
// let allActors = 
//     [0 .. workRange-1]
//     |> List.map(fun id ->   let properties = [|  string(t):> obj; string( id +  workRange*(t)):>obj; string(k) :> obj ;string(n) :> obj |]
//                             system.ActorOf(Props(typedefof<EchoServer>, properties)))


// let lastActor = 
//     [workRange-1 .. workRange]
//     |> List.map(fun id ->   let properties = [|  string(t):> obj; string(n):>obj; string(k) :> obj ;string(n) :> obj |]
//                             system.ActorOf(Props(typedefof<EchoServer>, properties)))

// let temp = [finalActor]
// let finalList = allActors @ lastActor


// let finalList = allActors 


// let lastList =  [|  int(intActors):>obj; int(0 + (intActors - 1)*workRange + 1) :> obj ;int(n) :> obj; (int)(k):> obj |]
// let finalActor = system.ActorOf(Props(typedefof<EchoServer>, lastList))
