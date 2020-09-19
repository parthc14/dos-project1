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
type ProcessMessage = ProcessJob of int * int * int

let n = 10000 |> float
let k = 24 |> float
let noOfCores = Environment.ProcessorCount |> float
let noActors = 10.0 * noOfCores |> float
let workRange =  n / noActors |> float |> ceil |>int

let N = n |>int
let K = k |> int


let orderProcessor (mailbox: Actor<_>) =
    let rec loop () = actor {
        let! ProcessJob(startNum,endNum,k) = mailbox.Receive ()
       // mailbox.Sender() <!sprintf "Message sent"
        // printfn "%i %i " startNum endNum  
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
        
        // printfn "%s" message
        return! loop ()
    }
    loop ()

let caller orderProcessor (n: int)(k:int)(workRange:int)(mailbox: Actor<_>) =
    let rec loop () = actor {
        let! message = mailbox.Receive ()
        printfn ""
        // printfn "Message sent! %s N is %f K is %f Workrange is %i" message n k workRange
    }
   
    for i in [1.. workRange-1] do
        orderProcessor <! ProcessJob(((0+(i-1)*workRange)+1), ((0)+(i)*workRange), (k))  

    
    orderProcessor <! ProcessJob(((0+(workRange-1)*workRange)+1), (n) ,(k))
    loop ()





printfn "Workrange is %i" workRange
let orderProcessorRef = spawn system "orderProcessor" orderProcessor
let callerRef = spawn system "caller" <| caller orderProcessorRef N K workRange



