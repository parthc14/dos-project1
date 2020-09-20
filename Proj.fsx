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

let n = 10000 |> float
let k = 2 |> float
let noOfCores = Environment.ProcessorCount |> float
let noActors = 10.0 * noOfCores |> float
let workRange =  n / noActors |> float |> ceil |>int

let N = n |> int |> int64
let K = k |> int |> int64
let newWorkRange = workRange |> int |> int64
let intActors = N / newWorkRange
let bigint(x:int) = bigint(x)

let orderProcessor (mailbox: Actor<_>) =
    printfn "Trying BigInt"
    printfn "bigInt: %A " 123456789I
    let rec loop () = actor {
        let! ProcessJob(startNum,endNum,k) = mailbox.Receive ()
        mailbox.Sender() <!sprintf "Message sent"
        
        // let mutable sqSum = 0 |> int64
        // let mutable sqrRoot = 0 |> double
        // for i in [startNum..endNum] do
        //     sqSum <-int64(0)
        //     for j in [i..(i+k-int64(1))] do
        //             let a = j |> int64
        //             sqSum <- (a*a) + sqSum
        //     // printfn "SqSum %i" sqSum
        //     let sumDouble = sqSum |> double        
        //     sqrRoot <- sqrt sumDouble 
        //     match ((sqrRoot%1.0)<=0.0) with 
        //     | true -> printfn "%i" i
        //     | false -> ()    
       
      
        // for x in [startNum..endNum] do
        //     let y = x+k - 1
        //     let z = y |> bigint.op_Implicit        // let sum1 = (y' *(y'+toBigInt(1))  *((toBigInt(2)*y')+toBigInt(1)))                                    
        //     // let sum = sum1 / toBigInt(6)
        //     printfn "Z %A" zI 
        //     let un = x-1
        //     let un' = toBigInt(un)
        //     let unSum = (un' * (un'+toBigInt(1)) * ((toBigInt(2)*un')+toBigInt(1)))                            
        //     let unwantedSum = unSum / toBigInt(6)
        //     let realSum =  sum - unwantedSum    |> double                              
        //     let sqrRoot = sqrt realSum                                 
        //     let roundSqrt =  sqrRoot |> floor |> double
            
        //     if sqrRoot-roundSqrt = 0.0 then                               
        //        printfn " Number is %i " x
                
        
                
        
        // let mutable sqSum = 0.0
        // let mutable sqrRoot = 0.0 
        // for i in [startNum..endNum] do
        //     for j in [i..(i+k-1)] do
        //         let x = j |> double
        //         sqSum <- x*x
        //     sqrRoot <- sqrt sqSum
             
        //     if <>sqrRoot % 1.0 > 0.0 then
        //         printfn "%i" i

            
                
        
        // printfn "%s" message
        return! loop ()
    }
    loop ()

let caller orderProcessor (n: int64)(k:int64)(workRange:int64)(mailbox: Actor<_>) =
    let rec loop () = actor {
        let! message = mailbox.Receive ()
        printfn ""
        // printfn "Message sent! %s N is %f K is %f Workrange is %i" message n k workRange
    }
    let allActors = intActors-int64(1)
    for i in [int64(1)..allActors] do
        orderProcessor <! ProcessJob(((i)-int64(1))*newWorkRange+int64(1), ((i))*newWorkRange, K)  

    
    orderProcessor <! ProcessJob((((intActors-int64(1))*newWorkRange)+int64(1)), (N) ,(K))
    loop ()

printfn "Workrange is %i" workRange
let orderProcessorRef = spawn system "orderProcessor" orderProcessor
let callerRef = spawn system "caller" <| caller orderProcessorRef N K newWorkRange






    

