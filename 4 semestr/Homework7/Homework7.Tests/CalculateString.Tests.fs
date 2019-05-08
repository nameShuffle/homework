namespace Tests

open NUnit.Framework
open FsUnit

module ``Tests for CalculateString task`` =
    open CalculateString

    let calculate = new CalculateStringBuilder()

    [<Test>]
    let ``normal string calculation test with adding`` () =
        let result = calculate {
            let! x = "25"
            let! y = "12"
            let z = x + y
            return z
        }
        result |> should equal (Some 37)

    [<Test>]
    let ``string calculation test with incorrect strings`` () =
        let result = calculate {
            let! x = "25"
            let! y = "hello"
            let z = x + y
            return z
        }
        result |> should equal None

    [<Test>]
    let ``normal string calculation test with multiplying`` () =
        let result = calculate {
            let! x = "25"
            let! y = "4"
            let z = x * y
            return z
        }
        result |> should equal (Some 100)