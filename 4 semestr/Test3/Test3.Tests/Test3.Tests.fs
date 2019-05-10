namespace Tests

open NUnit.Framework
open FsUnit
open HashTable

module ``Tests for Rounding task`` =

    let hashFunc (key : obj) = 
        int (key :?> string).[0]

    let hashTable = new HashTable(hashFunc, 100)
    hashTable.Add("sun")
   
    [<Test>]
    let ``test exist method`` () =
        hashTable.isExist("sun") |> should equal true

    [<Test>]
    let ``test add method`` () =
        hashTable.Add("hello")
        hashTable.isExist("hello") |> should equal true

    [<Test>]
    let ``test delete method`` () =
        hashTable.Add("world")
        hashTable.Delete("world")
        hashTable.isExist("world") |> should equal false

    