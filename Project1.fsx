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

let bigInt (x: int) = bigint (x)


let n = 100 |> float
let k = 24 |> float
let noOfCores = Environment.ProcessorCount |> float
let noActors = 10.0 * noOfCores |> float
let workRange =  n / noActors |> float |> ceil |>int
// printfn "Number of Actors %f" noActors
printfn "Work Range %i" workRange



type EchoServer(id, startNum, endNum, k) =
    inherit Actor()
    
        
    override x.OnReceive message =
        
        let mutable temp = 1 |> bigInt
        let mutable st = 1|> bigint
        let mutable res = 0 |> bigInt 
        for x in [startNum..endNum] do
            let startPlusWindow = (x+k-bigInt(1))
            for j in [x..startPlusWindow] do
                    temp <- (j*j)
                    res <- res + temp
            while(st*st <=(res)) do
                if(st*st = res) then
                    printfn "%A" (startPlusWindow-k + bigint(1))

                st<- st + bigint(1)
            res <- bigint(0)





let intActors = noActors |>int
// let allActors = 
//     [1 .. workRange-1]
//     |> List.map(fun id ->   let properties = [|  int(id):> obj; int(0 + (id-1)* workRange + 1):>obj; int(0 + (id)*workRange) :> obj ;int(k) :> obj |]
//                             system.ActorOf(Props(typedefof<EchoServer>, properties)))

let allActors = 
    [0 .. workRange-1]
    |> List.map(fun id ->   let properties = [|  string(id):> obj; string( id +  workRange*(id)):>obj; string(k) :> obj ;string(n) :> obj |]
                            system.ActorOf(Props(typedefof<EchoServer>, properties)))

let lastList =  [|  int(intActors):>obj; int(0 + (intActors - 1)*workRange + 1) :> obj ;int(n) :> obj; (int)(k):> obj |]
let finalActor = system.ActorOf(Props(typedefof<EchoServer>, lastList))           
let temp = [finalActor]
let finalList = allActors @ temp  


for id in [ 1 .. workRange] do
    List.item (id) finalList
    <! sprintf "Done! %d" id


system.Terminate()



// let lastList =  [|  int(intActors):>obj; int(0 + (intActors - 1)*workRange + 1) :> obj ;int(n) :> obj; (int)(k):> obj |]
// let finalActor = system.ActorOf(Props(typedefof<EchoServer>, lastList))


// let finalActor = 
//     [workRange-1 .. workRange]
//     |> List.map(fun id ->   let properties = [|  int(workRange):>obj; int(0 + (workRange - 1)*workRange + 1) :> obj ;int(n) :> obj; (int)(k):> obj |]
//                             system.ActorOf(Props(typedefof<EchoServer>, properties)))






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