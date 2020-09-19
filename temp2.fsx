#time "on"
#r "nuget: Akka.FSharp"
#r "nuget: Akka.TestKit"
// #load "Bootstrap.fsx"

open System
open Akka.Actor
open Akka.Configuration
open Akka.FSharp
open Akka.TestKit




let system = System.create "system" <| Configuration.load ()
type ProcessMessage = ProcessJob of int64 * int64 * int64
let int64(x:int) = int64(x)
let n = 100000 |> float
let k = 2 |> float
let noOfCores = Environment.ProcessorCount |> float
let noActors = 10.0 * noOfCores |> float
let workRange =  n / noActors |> float |> ceil |>int

let N = n |>int
let K = k |> int
let intActors = N / workRange
// printfn "IntActors: %i" intActors 

let N' = N |> int64
let K' = K |> int64
let newWorkRange = workRange |> int64
let newActors = intActors |> int64


let orderProcessor (mailbox: Actor<_>) =
    let rec loop () = actor {
        let! ProcessJob(startNum,endNum,k) = mailbox.Receive ()
       // mailbox.Sender() <!sprintf "Message sent"
        // printfn "StartNum: %i EndNum: %i " startNum endNum  
        for x in [startNum..endNum] do
            let y = x+k - int64(1)
            let sum1 = (y *(y+int64(1))  *((int64(2)*y)+int64(1)))                                    
            let sum = sum1 / int64(6) |> float
            let un = x-int64(1)
            let unSum = (un * (un+int64(1)) * ((int64(2)*un)+int64(1)))                            
            let unwantedSum = unSum / int64(6) |> float
            let realSum =  sum - unwantedSum                                  
            let sqrRoot = sqrt realSum                                 
            let roundSqrt =  sqrRoot |> floor |> float
            if sqrRoot-roundSqrt = 0.0 then                               
                printfn "%i " x
       
        // printfn "%s" message
        return! loop ()
    }
    loop ()

let caller orderProcessor (n: int64)(k:int64)(workRange:int64)(mailbox: Actor<_>) =
    let rec loop () = actor {
        let! message = mailbox.Receive ()
        printfn ""
    }
    let allActors = newActors-int64(1)
    for i in [int64(1)..allActors] do
        orderProcessor <! ProcessJob(((i)-int64(1))*newWorkRange+int64(1), ((i))*newWorkRange, K')  

    
    orderProcessor <! ProcessJob((((newActors-int64(1))*newWorkRange)+int64(1)), (N') ,(K'))
    loop ()

printfn "Workrange is %i" workRange
let orderProcessorRef = spawn system "orderProcessor" orderProcessor
let callerRef = spawn system "caller" <| caller orderProcessorRef N' K' newWorkRange



