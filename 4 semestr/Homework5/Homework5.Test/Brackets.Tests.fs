namespace Tests

open NUnit.Framework
open FsUnit
open Brackets

module ``Tests for Brackets Task`` =
    [<Test>]
    let ``the function should return true for an empty string`` () =
        brackets "" |> should equal true
    
    [<Test>]
    let ``the function should return true for a correct string`` () =
        brackets "a(kdfj{} fj) [dfd]" |> should equal true

    [<Test>]
    let ``the function should return false for an incorrect string`` () =
        brackets "sdf({)} []" |> should equal false

    [<Test>]
    let ``the function should return true for a list without brackets`` () =
        brackets "hello" |> should equal true