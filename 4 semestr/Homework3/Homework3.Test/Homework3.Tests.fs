namespace Tests

open NUnit.Framework
open FsUnit

module ``Tests for EvenNumbers Task`` =
    open EvenNumbers

    [<Test>]
    let ``evenNumbers1 should return zero for an empty list`` () =
        evenNumbers1 [] |> should equal 0

    [<Test>]
    let ``evenNumbers2 should return zero for an empty list`` () =
        evenNumbers2 [] |> should equal 0

    [<Test>]
    let ``evenNumbers3 should return zero for an empty list`` () =
        evenNumbers3 [] |> should equal 0

    
    [<Test>]
    let ``evenNumbers1 should return zero for a list without even elements`` () =
        evenNumbers1 [1; 3; 5] |> should equal 0

    [<Test>]
    let ``evenNumbers2 should return zero for a list without even elements`` () =
        evenNumbers2 [1; 3; 5] |> should equal 0

    [<Test>]
    let ``evenNumbers3 should return zero for a list without even elements`` () =
        evenNumbers3 [1; 3; 5] |> should equal 0


    [<Test>]
    let ``evenNumbers1 should return 2 for this list`` () =
        evenNumbers1 [1; 2; 4] |> should equal 2

    [<Test>]
    let ``evenNumbers2 should return 2 for this list`` () =
        evenNumbers2 [1; 2; 4] |> should equal 2

    [<Test>]
    let ``evenNumbers3 should return 2 for this list`` () =
        evenNumbers3 [1; 2; 4] |> should equal 2
    
module ``Tests for MapWithTrees Task`` =
    open BinaryTree

    let addOne x = x + 1

    let rec createTreeFromOneNumber x height = 
       if height = 0 then Null
       else Tree(x, createTreeFromOneNumber x (height - 1), createTreeFromOneNumber x (height - 1))

    [<Test>]
    let ``mapWithTrees should return Null for an empty Tree`` () =
        mapWithTree addOne Null |> should equal (Null:Tree<int>)
        
    [<Test>]
    let ``mapping tree of 2 with function addOne `` () =
        let tree = createTreeFromOneNumber 2 5
        let resultTree = createTreeFromOneNumber 3 5
        mapWithTree addOne tree |> should equal resultTree

module ``Tests for CalculateParseTree Task`` =
    open CalculateTree

    [<Test>]
    let ``test add`` () =
        let tree = Add(Value(5.0), Value(5.0))
        calculateParseTree tree |> should equal 10
    
    [<Test>]
    let ``test subtrarct`` () =
        let tree = Subtract(Value(5.0), Value(3.0))
        calculateParseTree tree |> should equal 2

    [<Test>]
    let ``test divide`` () =
        let tree = Divide(Value(6.0), Value(3.0))
        calculateParseTree tree |> should equal 2

    [<Test>]
    let ``test multiply`` () =
        let tree = Multiply(Value(5.0), Value(3.0))
        calculateParseTree tree |> should equal 15

    [<Test>]
    let ``the function should return inf for a tree with devidin by zero`` () =
        let tree = Divide(Value(5.0), Value(0.0))
        calculateParseTree tree |> should equal infinity

    [<Test>]
    let ``the function should return 30 for the complex tree`` () =
        let tree = Multiply(Add(Value(5.0), Value(10.0)), Value(2.0))
        calculateParseTree tree |> should equal 30

module ``Tests for PrimeNumbers Task`` =
    open PrimeNumbers
    
    [<Test>]
    let ``the function should return false for a not prime`` () =
        isPrime 33 |> should be False 
    
    [<Test>]
    let ``the function should return true for a prime`` () =
        isPrime 2027 |> should be True
    
    [<Test>]
    let ``a big prime number should be in sequence`` () =
        Seq.contains 515041 infinitPrimes |> should be True
