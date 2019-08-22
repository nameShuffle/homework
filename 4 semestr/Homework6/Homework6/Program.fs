open System
open Homework6

open OperatingSystem
open Computer
open Network

[<EntryPoint>]
let main argv =
    let windows = new OperatingSystem("windows", 50)
    let linux = new OperatingSystem("linux", 25)
    let android = new OperatingSystem("android", 50)

    let comp1 = new Computer(windows, false)
    let comp2 = new Computer(linux, false)
    let comp3 = new Computer(android, false)
    let comp4 = new Computer(windows, true)

    let listComputers = [| comp1; comp2; comp3; comp4 |]
    let matrix = [| [| 0; 1; 0; 0|]; [| 1; 0; 0; 1 |]; [| 0; 0; 0; 1 |]; [| 0; 1; 1; 0 |]; |]
    
    let net = new Network(listComputers, matrix)
    net.Simulate()
    net.Simulate()

    let status = net.getStatus()
    for computer in status do
        printfn "%s" computer
    
    Console.ReadLine()
    0