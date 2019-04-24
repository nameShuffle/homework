namespace Tests

open NUnit.Framework
open FsUnit
open PhoneBook

module ``Tests for PhoneBook Task`` =
    open System
    open System.IO

    let personOne = { Name = "Phil"; Phone = "8888" }
    let newPerson = { Name = "Dan"; Phone = "9999" }
    let book = [ personOne ]

    let pathToProgram = Environment.CurrentDirectory
    let path = (System.IO.DirectoryInfo pathToProgram).Parent.Parent.Parent.Parent

    [<Test>]
    let ``test add function`` () =
        addPersonToBook "Dan" "9999" book |> should equal [ newPerson; personOne ]
    
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

    [<Test>]
    let ``test write to file function`` () =
        let nameOfFile = path.FullName + "/write.txt"
        writeInfoToFile nameOfFile  book
        File.ReadAllLines nameOfFile |> should equal [ "Phil"; "8888" ]

    [<Test>]
    let ``test read from file`` () =
        let nameOfFile = path.FullName + "/read.txt"
        Option.get (readInfoFromFile nameOfFile []) |> should equal [ personOne; newPerson ]

    [<Test>]
    let ``test read from file with file that does not exist`` () =
        let nameOfFile = path.FullName + "/cantread.txt"
        readInfoFromFile nameOfFile [] |> should equal None
