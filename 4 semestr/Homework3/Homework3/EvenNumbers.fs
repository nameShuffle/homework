module EvenNumbers

///Задача 1: подсчет количества четных чисел в списке.
///Создание списка на основе переданного только из четных элементов, подсчет его длины.
let evenNumbers1 list = (list |> List.filter (fun element -> element % 2 = 0)).Length

///Проверка элемента на четность, увеличение значения аккумулятрова в случае
///прохождения проверки.
let evenNumbers2 list = 
    list |> List.fold (fun acc element -> if element % 2 = 0 then acc + 1 else acc) 0

///Предварительно изменение списка: если число четное, то элемент заменяется на 1,
///в противном случае на ноль. В качестве результата возвращается сумма из элементов
///полученного списка.
let evenNumbers3 list = 
    let countList = list |> List.map (fun element -> if element % 2 = 0 then 1 else 0)
    countList |> List.fold (fun acc element -> acc + element) 0
