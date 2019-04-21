module PhoneBook

open System
open System.IO

///Тип для удобства хранения информации в записной книжке. Один объект соответсвует одному человеку.
///Каждый человек имеет свой номер телефона и свое имя.
type Person = 
   { Name : string
     Phone : string }

///Ниже записаны функции для отдельных команд по работе с телефонным справочником.
///Вынесены для удобства тестирования программы.

///Добавление нового человека в список. Возвращает новый список.
let addPersonToBook name phone book = 
    { Name = name; Phone = phone} :: book

///Поиск человека по имени. Функция возвращает найденного человека.
let findPersonByName nameToSearch book = 
    List.tryFind (fun person -> person.Name = nameToSearch) book

///Поиск человека по номеру телефона. Функция возвращает найденного человека.
let findPersonByPhone phoneToSearch book =
    List.tryFind (fun person -> person.Phone = phoneToSearch) book

///Запись содержимое списка нужного формата в файл по указанному имени.
let writeInfoToFile fileName book =
    let writer = File.CreateText fileName 
    List.iter (fun person -> 
        fprintfn writer "%s" person.Name 
        fprintfn writer "%s" person.Phone) book
    writer.Close()

///Чтение информации о людях в нужном формате из файла. Информация добавляется к уже имеющемуся списку,
///функция возвращает готовый список.
let readInfoFromFile fileName book = 
    let rec readPersonsInfoFromFile (reader : StreamReader) currentList = 
        match reader.ReadLine() with
        | null -> reader.Close(); currentList 
        | name -> 
            let phone = reader.ReadLine()
            let newPerson = { Name = name; Phone = phone}
            readPersonsInfoFromFile reader (newPerson :: currentList)
    if File.Exists fileName then
        let reader = File.OpenText fileName
        Some (readPersonsInfoFromFile reader book)
    else
        None

///Непосредственно реализация программы-телефонного справочника. Выполняет команды в интерактивном режиме.
///Выводит приглашение на ввод в консоль. Реализованы следующие команды:
/// - "add" - добавление нового человека в справочник.
/// - "search by name" - поиск нужного человека по имени.
/// - "search by phone" - поиск нужного человека по номеру телефона.
/// - "show all" - вывод на консоль данные всех имеющихся в справочнике людей.
/// - "save to file" - сохрание в файл текущего содержимого справочника.
/// - "get from file" - получение информации из файла и запись ее в справочник.
///Остальные команды считаются некорректными и игнорируются.
let phoneBook = 
    let rec phoneBookAction book = 
        printf "New command: "
        let command = Console.ReadLine()
        if command = "add" then
            printf "Name: "
            let name = Console.ReadLine()
            printf "Phone: "
            let phone = Console.ReadLine()

            phoneBookAction (addPersonToBook name phone book)
        else if command = "search by name" then
            printf "Name: "
            let nameToSearch = Console.ReadLine()
            let info = findPersonByName nameToSearch book
            match info with
            | Some person -> printfn "Founded phone %s" person.Phone
            | None -> printfn "There is no such person in the book."
            
            phoneBookAction book
        else if command = "search by phone" then
            printf "Phone: "
            let phoneToSearch = Console.ReadLine()
            let info = findPersonByPhone phoneToSearch book
            match info with
            | Some person -> printfn "Founded person %s" person.Name
            | None -> printfn "There is no such phone in the book."
            
            phoneBookAction book
        else if command = "show all" then
            List.iter (fun person -> 
                printf "%s " person.Name 
                printfn "%s" person.Phone) book
            
            phoneBookAction book
        else if command ="save to file" then
            printf "File name: "
            let fileName = Console.ReadLine()
            writeInfoToFile fileName

            printfn "Info was writen to file."
            phoneBookAction book
        else if command = "get from file" then
            printf "File name: "
            let fileName = Console.ReadLine()
            let info = readInfoFromFile fileName book
            match info with
            | Some newBook -> printfn "Info from file was added"; phoneBookAction newBook
            | None -> printfn "There if no such file"; phoneBookAction book
        else if command = "exit" then 0 
        else 
            printfn "incorrect command"
            phoneBookAction book

    phoneBookAction []

