// Learn more about F# at http://fsharp.org

open System

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
let rec createActor nOfCores t workRange k n =
    let notLastActor =
        [ 1 .. workRange-1 ]
        |> List.map (fun id ->
            let properties = [| string(id) :> obj; string (noOfCores) :> obj; string (t) :> obj ; string(k) :> obj; string (n) :> obj |]
            system.ActorOf(Props(typedefof<EchoServer>, properties)))


let system = ActorSystem.Create("FSharp")

let args : string array = fsi.CommandLineArgs |> Array.tail
let n =  args.[0] |> float
let k = args.[1] |> float

let noOfCores = 10*Environment.ProcessorCount |> float
let temp = n / (noOfCores: float) 
let workRange =  temp |> ceil |>int
let t = 1


type EchoServer(name) =
    inherit Actor()

    override x.OnReceive message =
        match message with
        | :? string as msg -> printfn "%s Name is : %s" msg name
        | _ -> failwith "unknown message"



let notLastActor =
    [ 1 .. workRange-1 ]
    |> List.map (fun id ->
        let properties = [| string(id) :> obj; string (noOfCores) :> obj; string (t) :> obj ; string(k) :> obj; string (n) :> obj |]
        system.ActorOf(Props(typedefof<EchoServer>, properties)))




let LastActor =
    [ workRange-1 .. workRange ]
    |> List.map (fun id ->
        let properties = [| string(id) :> obj; string (noOfCores) :> obj; string (t) :> obj ; string(k) :> obj; string (n) :> obj |]
        system.ActorOf(Props(typedefof<EchoServer>, properties)))


let worker startNum, endNum, k, n, pid =
    let low = startNum |> int
    let high = endNum |> int
    for x in [startNum..endNum] do
        let y = x+k - 1
        let sum1 = (y *(y+1)  *((2*y)+1))                                    
        let sum = sum1 / 6 |>int
        let un = x-1
        let unSum = (un * (un+1) * ((2*un)+1))                            
        let unwantedSum = unSum / 6 |>int
        let realSum =  sum - unwanted_sum                                  
        let sqrRoot = sqrt realSum                                 
        let roundSqrt = floor sqrRoot |> float                  
        if sqrRoot-roundSqrt = 0 then                               
            printfn "%i" x
    EchoServer <! sprintf "Done" 


// let echoServers =
//     [ 1 .. workRange ]
//     |> List.map (fun id ->
//         let properties = [| string(id) :> obj || string (noOfCores) :> obj || string (t) :> obj || string(k) :> obj|| string (n) :> obj |]
//         system.ActorOf(Props(typedefof<EchoServer>, properties)))
    
    //     for _ in [1..noOfCores] do
    //     // pass
    
    
   
    //     let list1 = [t, t+workRange, k, n]
    //     spawn system "myActor" Project1.worker list1    
    //         // Check list 1 is passed
        
        
        
        
        
    // let worker start_num end_num k n 
