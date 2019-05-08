namespace Tests

open NUnit.Framework
open FsUnit

module ``Tests for Rounding task`` =
    open Rounding

    let rounding number = new RoundingBuilder(number)

    [<Test>]
    let ``round to 3`` () =
        let result = 
            rounding 3 {
                let! a = 2.0 / 12.0
                let! b = 3.5
                return a / b
            }
        result |> should equal 0.048

    [<Test>]
    let ``round to 5`` () =
        let result = 
            rounding 5 {
                let! a = 2.0 / 12.0
                let! b = 3.5
                return a / b
            }
        result |> should equal 0.04762

    [<Test>]
    let ``round to 0`` () =
        let result = 
            rounding 0 {
                let! a = 12.0
                let! b = 3.5
                return a / b
            }
        result |> should equal 3