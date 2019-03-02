namespace Tests

open NUnit.Framework
open FsUnit

module ``Tests for FindElement Task`` =
    open FirstPosition

    [<Test>]
    let ``the function should return None for an empty list`` () =
        firstPosition 'a' [] |> should equal None
    
    [<Test>]
    let ``the function should return None for a list without element`` () =
        firstPosition 'd' ['a';'b';'c'] |> should equal None

    [<Test>]
    let ``first position of 'c' should be 2`` () =
        Option.get(firstPosition 'c' ['a';'b';'c';'d']) |> should equal 2

    [<Test>]
    let ``first position of 'c' should be 2 with more then one 'c'`` () =
        Option.get(firstPosition 'c' ['a';'b';'c';'c'; 'd']) |> should equal 2
    
module ``Tests for Palindrome Task`` =
    open Palindrome

    [<Test>]
    let ``isPalindrome should return true for an empty string`` () =
        isPalindrome "" |> should be True
    
    [<Test>]
    let ``isPalindrome should return true for a palindrome`` () =
        isPalindrome "топот" |> should be True

    [<Test>]
    let ``isPalindrome should return false for not palindrome string`` () =
        isPalindrome "hello my name is Dan" |> should be False

module ``Test for Mergesort`` =
    open Mergesort

    [<Test>]
    let ``sorting a list of five elements`` () =
        mergesort [4;1;3;5;2] |> should equal [1;2;3;4;5]
    
    [<Test>]
    let ``an empty list after sorting should be an empty list`` () =
        mergesort [] |> should equal []

    [<Test>]
    let ``sort already sorted list`` () =
        mergesort ['a';'b';'c'] |> should equal ['a';'b';'c']