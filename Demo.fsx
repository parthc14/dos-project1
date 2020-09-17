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

type EchoServer(startNum, endNum,n, k) =
    inherit Actor()
        
    override x.OnReceive message =

        // let low = startNum |> int
        // let high = endNum |> int
        // for x in [low..high] do
        //     let y = x+k - 1
        //     let sum1 = (y *(y+1)  *((2*y)+1))                                    
        //     let sum = sum1 / 6 |>int
        //     let un = x-1
        //     let unSum = (un * (un+1) * ((2*un)+1))                            
        //     let unwantedSum = unSum / 6 |>int
        //     let realSum =  sum - unwantedSum                                  
        //     let sqrRoot = sqrt realSum                                 
        //     let roundSqrt = floor sqrRoot |> float                  
        //     if sqrRoot-roundSqrt = 0 then                               
        //         printfn "%i" x

        match message with
        | :? string as msg -> printfn "startNum: %f  endNum %f N %f K %f" startNum endNum n k
        | _ ->  failwith "unknown message"

let n = 1000 |> float
let k = 100 |> float
let noOfCores = Environment.ProcessorCount |> float
let noActors = 10.0 * noOfCores |> float
let workRange =  n / noActors |> float |> ceil |>int
let t = 1

printfn " WorkRange is %i" workRange

let intActors = noActors |>int

let allActors = 
    [0 .. intActors-1]
    |> List.map(fun id ->   let properties = [|  float(id):> obj; float(0 + (id-1)* workRange + 1):>obj; float(0 + (id)*workRange) :> obj ;float(k) :> obj |]
                            system.ActorOf(Props(typedefof<EchoServer>, properties)))
            
            

let lastList =  [|  float(intActors):>obj; float(0 + (intActors - 1)*workRange + 1) :> obj ;float(n) :> obj; (float)(k):> obj |]
     
let finalActor = system.ActorOf(Props(typedefof<EchoServer>, lastList))
        
let temp = [finalActor]
let finalList = allActors @ temp     









// et list = createWorkder newCore t workRange k n
// let allActors = 
//     [0 .. workRange-1]
//     |> List.map(fun id ->   let properties = [|  string(t):> obj; string( id +  workRange*(t)):>obj; string(k) :> obj ;string(n) :> obj |]
//                             system.ActorOf(Props(typedefof<EchoServer>, properties)))


// let lastActor = 
//     [workRange-1 .. workRange]
//     |> List.map(fun id ->   let properties = [|  string(t):> obj; string(n):>obj; string(k) :> obj ;string(n) :> obj |]
//                             system.ActorOf(Props(typedefof<EchoServer>, properties)))


// let finalList = allActors @ lastActor


// let finalList = allActors 

for id in [ 0 .. intActors] do
    List.item (id) finalList
    <! sprintf "F# %d!" id



system.Terminate()

