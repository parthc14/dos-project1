#time "on"
#r "nuget: Akka.FSharp"
#r "nuget: Akka.TestKit"
// #load "Bootstrap.fsx"

open System
open Akka.Actor
open Akka.Configuration
open Akka.FSharp
open Akka.TestKit



let system = ActorSystem.Create("FSharp")

open System
open Akka.Actor
open Akka.Configuration
open Akka.FSharp
open Akka.TestKit


type EchoServer(id, startNum, endNum, k) =
    inherit Actor()
        
    override x.OnReceive message =
        
        for x in [startNum..endNum] do
            let y = x+k - 1
            let sum1 = (y *(y+1)  *((2*y)+1))                                    
            let sum = sum1 / 6 |> float
            let un = x-1
            let unSum = (un * (un+1) * ((2*un)+1))                            
            let unwantedSum = unSum / 6 |> float
            let realSum =  sum - unwantedSum                                  
            let sqrRoot = sqrt realSum                                 
            let roundSqrt =  sqrRoot |> floor |> float                  
            if sqrRoot-roundSqrt = 0.0 then                               
                printfn "%i" x

        match message with
        | :? string as msg -> () //printfn " %s"  msg   
        | _ ->  failwith "unknown message"



let args : string array = fsi.CommandLineArgs |> Array.tail
let n =  args.[0] |> float
let k = args.[1] |> float
let noOfCores = Environment.ProcessorCount |> float
let noActors = 10.0 * noOfCores |> float
let workRange =  n / noActors |> float |> ceil |>int
// printfn "Number of Actors %f" noActors
printfn "Work Range %i" workRange


let intActors = noActors |>int
let allActors = 
    [1.. workRange-1]
    |> List.map(fun id ->   let properties = [|  int(id):> obj; int(0 + (id-1)* workRange + 1):>obj; int(0 + (id)*workRange) :> obj ;int(k) :> obj |]
                            system.ActorOf(Props(typedefof<EchoServer>, properties)))
            


let lastList =  [|  int(workRange):>obj; int(0 + (workRange - 1)*workRange + 1) :> obj ;int(n) :> obj; (int)(k):> obj |]
let finalActor = system.ActorOf(Props(typedefof<EchoServer>, lastList))

let temp = [finalActor]
let finalList = allActors @ temp

// let finalActor = 
//     [workRange-1 .. workRange]
//     |> List.map(fun id ->   let properties = [|  int(workRange):>obj; int(0 + (workRange - 1)*workRange + 1) :> obj ;int(n) :> obj; (int)(k):> obj |]
//                             system.ActorOf(Props(typedefof<EchoServer>, properties)))

// let finalList = allActors @ finalActor   


for id in [ 0 .. workRange-1] do
    List.item (id) finalList
    <! sprintf "Done! %d" id


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
// 8576