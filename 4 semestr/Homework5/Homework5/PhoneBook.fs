module PhoneBook

open System
open System.IO

type Person = 
   { Name : string
     Phone : string }

let rec readPersonsInfoFromFile (reader : StreamReader) currentList = 
    match reader.ReadLine() with
    | null -> reader.Close(); currentList 
    | name -> 
        let phone = reader.ReadLine()
        let newPerson = { Name = name; Phone = phone}
        readPersonsInfoFromFile reader (newPerson :: currentList)

let findPersonByName name = 

let phoneBook = 
    let rec phoneBookAction book = 
        printf "New command: "
        let command = Console.ReadLine()
        if command = "add" then
            printf "Name: "
            let name = Console.ReadLine()
            printf "Phone: "
            let phone = Console.ReadLine()
            let newPerson = { Name = name; Phone = phone}

            phoneBookAction (newPerson :: book)
        else if command = "search by name" then
            printf "Name: "
            let nameToSearch = Console.ReadLine()
            let info = List.find (fun person -> person.Name = nameToSearch) book
            
            printfn "Founded phone: %s" info.Phone
            phoneBookAction book
        else if command = "search by phone" then
            printf "Phone: "
            let phoneToSearch = Console.ReadLine()
            let info = List.find (fun person -> person.Phone = phoneToSearch) book

            printfn "Founded name: %s" info.Name
            phoneBookAction book
        else if command = "show all" then
            List.iter (fun person -> 
                printf "%s " person.Name 
                printfn "%s" person.Phone) book
            
            phoneBookAction book
        else if command ="save to file" then
            printf "File name: "
            let fileName = Console.ReadLine()
            let writer = File.CreateText fileName 
            List.iter (fun person -> 
                fprintfn writer "%s" person.Name 
                fprintfn writer "%s" person.Phone) book
                
            writer.Close()
            phoneBookAction book
        else if command = "get from file" then
            printf "File name: "
            let fileName = Console.ReadLine()
            let reader = File.OpenText fileName
            let newBook = readPersonsInfoFromFile reader book

            phoneBookAction newBook
        else if command = "exit" then 0 
        else 
            printfn "incorrect command"
            phoneBookAction book

    phoneBookAction []

