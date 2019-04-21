namespace Tests

open NUnit.Framework
open FsUnit
open PhoneBook

module ``Tests for PhoneBook Task`` =
    let personOne = { Name = "Phil"; Phone = "8888"}
    let newPerson = { Name = "Dan"; Phone = "9999"}
    let book = [ personOne ]

    [<Test>]
    let ``test add function`` () =
        addPersonToBook "Dan" "9999" book |> should equal [ newPerson; personOne]
    
    [<Test>]
    let ``test search by name function`` () =
        Option.get (findPersonByName "Phil" book) |> should equal personOne

    [<Test>]
    let ``test search by name function with person that not in the book`` () =
        findPersonByName "Dan" book |> should equal None
    
    [<Test>]
    let ``test search by phone function`` () =
        Option.get (findPersonByPhone "8888" book) |> should equal personOne

    [<Test>]
    let ``test search by phone function with person that not in the book`` () =
        findPersonByPhone "9999" book |> should equal None