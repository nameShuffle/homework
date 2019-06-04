namespace Tests

open NUnit.Framework
open FsUnit
open Homework6
open Network
open OperatingSystem
open Computer

module ``Tests for Network Task`` =
    let matrix = [| [| 0; 1; 0; 0|]; [| 1; 0; 0; 1 |]; [| 0; 0; 0; 1 |]; [| 0; 1; 1; 0 |]; |]

    [<Test>]
    let ``test network with 100% chance and one move`` () =
        let windows = new OperatingSystem("windows", 100)
        let linux = new OperatingSystem("linux", 100)
        let android = new OperatingSystem("android", 100)

        let comp1 = new Computer(windows, false)
        let comp2 = new Computer(linux, false)
        let comp3 = new Computer(android, false) 
        let comp4 = new Computer(windows, true)

        let listComputers = [| comp1; comp2; comp3; comp4 |]
        
        let net = new Network(listComputers, matrix)
        net.Simulate()

        comp1.IsInfected |> should equal false
        comp2.IsInfected |> should equal true
        comp3.IsInfected |> should equal true
        comp4.IsInfected |> should equal true

    [<Test>]
    let ``test network with 100% chance and two moves`` () =
        let windows = new OperatingSystem("windows", 100)
        let linux = new OperatingSystem("linux", 100)
        let android = new OperatingSystem("android", 100)

        let comp1 = new Computer(windows, false)
        let comp2 = new Computer(linux, false)
        let comp3 = new Computer(android, false)
        let comp4 = new Computer(windows, true)

        let listComputers = [| comp1; comp2; comp3; comp4 |]
        
        let net = new Network(listComputers, matrix)
        net.Simulate()
        net.Simulate()

        comp1.IsInfected |> should equal true
        comp2.IsInfected |> should equal true
        comp3.IsInfected |> should equal true
        comp4.IsInfected |> should equal true

    [<Test>]
    let ``test network with 0% chance`` () =
        let windows = new OperatingSystem("windows", 0)
        let linux = new OperatingSystem("linux", 0)
        let android = new OperatingSystem("android", 0)

        let comp1 = new Computer(windows, false)
        let comp2 = new Computer(linux, false)
        let comp3 = new Computer(android, false)
        let comp4 = new Computer(windows, true)

        let listComputers = [| comp1; comp2; comp3; comp4 |]
        
        let net = new Network(listComputers, matrix)
        net.Simulate()
        net.Simulate()

        comp1.IsInfected |> should equal false
        comp2.IsInfected |> should equal false
        comp3.IsInfected |> should equal false
        comp4.IsInfected |> should equal true

    [<Test>]
    let ``test network with 100% chance, two moves and changing network`` () =
        let newMatrix = [| [| 0; 0; 0; 0|]; [| 0; 0; 0; 1 |]; [| 0; 0; 0; 1 |]; [| 0; 1; 1; 0 |]; |]
        
        let windows = new OperatingSystem("windows", 100)
        let linux = new OperatingSystem("linux", 100)
        let android = new OperatingSystem("android", 100)

        let comp1 = new Computer(windows, false)
        let comp2 = new Computer(linux, false)
        let comp3 = new Computer(android, false)
        let comp4 = new Computer(windows, true)

        let listComputers = [| comp1; comp2; comp3; comp4 |]
        
        let net = new Network(listComputers, matrix)
        net.Simulate()
        net.ChangeNetwork(newMatrix)
        /// После изменения сети компьютер 1 изолирован от других и не должен заразиться за второй шаг.
        net.Simulate()

        comp1.IsInfected |> should equal false
        comp2.IsInfected |> should equal true
        comp3.IsInfected |> should equal true
        comp4.IsInfected |> should equal true
    
    