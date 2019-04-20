module PhoneBook

open System
open System.IO

type person = 
   { name : string
     phone : string }

let phoneBook = 
    let rec phoneBookAction book = 
        printf "New command: "
        let command = Console.ReadLine()
        if command = "add" then
            printf "Name: "
            let name = Console.ReadLine()
            printf "Phone: "
            let phone = Console.ReadLine()
            let newPerson = { name = name; phone = phone}

            phoneBookAction (newPerson :: book)
        else if command = "searchByName" then
            printf "Name: "
            let nameToSearch = Console.ReadLine()
            let info = List.find (fun y -> y.name = nameToSearch) book
            
            printfn "Founded phone: %s" info.phone
            phoneBookAction book
        else if command = "searchByPhone" then
            printf "Phone: "
            let phoneToSearch = Console.ReadLine()
            let info = List.find (fun y -> y.phone = phoneToSearch) book

            printfn "Founded name: %s" info.name
            phoneBookAction book
        else if command ="saveToFile" then
            printf "File name: "
            let fileName = Console.ReadLine()
            use file = File.CreateText(fileName) 
            List.iter (fun elem -> file.WriteLine(elem.name); file.WriteLine(elem.phone)) book
            
            phoneBookAction book
        else if command = "exit" then 0 
        else 
            printfn "incorrect command"
            phoneBookAction book

    phoneBookAction []