namespace Tests

open NUnit.Framework
open FsUnit

module ``Tests for FibonacciSum Task`` =
    open fibonacci 

    [<Test>]
    let ``the sum of even fibonacci numbers`` () =
        fibonacciSum |> should equal 1089154
 
module ``Tests for PrintSquare Task`` =
    open Square

    [<Test>]
    let ``square from incorrect data`` () =
        createSquare -1 |> should equal []

    [<Test>]
    let ``square from 2 elements`` () =
        createSquare 2 |> should equal ["**"; "**"]

    [<Test>]
    let ``square from 4 elements`` () =
        createSquare 4 |> should equal ["****"; "*  *"; "*  *"; "****"]