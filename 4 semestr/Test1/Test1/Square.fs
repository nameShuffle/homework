module Square

///Создание списка строк, которые должны образовывать квадрат из *;
let createSquare n = 
    if n < 0 then printfn "Проверьте корректность введенных данных!"; []
    else
        ///Вспомогательная функция для создания "границ" - создает список из n *.
        let createBorderList = 
            let rec helpBorderList currentN currentList = 
                if currentN < 1 then currentList
                else helpBorderList (currentN - 1) ("*" + currentList)

            helpBorderList n ""
    
        ///Вспомогательная функция для создания обычных строк - создает список из n
        ///элементов, при этом * только первый и последний символ, остальные - пробел.
        let createCommonList = 
            let rec helpCommonList currentN currentList = 
                if currentN < 1 then currentList
                else
                    match currentN with 
                    | _ when currentN = n 
                        -> helpCommonList (currentN - 1) ("*" + currentList)
                    | _ when currentN = 1 
                        -> helpCommonList (currentN - 1) ("*" + currentList)
                    | _ -> helpCommonList (currentN - 1) (" " + currentList)

            helpCommonList n ""
    
        ///Функция для создания списка из строк. Первый и последний элемент - "граничные"
        ///строки, остальные элементы - обычные строки.
        let rec createListOfStrings currentN currentList =
            if currentN < 1 then currentList
            else 
                match currentN with 
                    | _ when currentN = n 
                        -> createListOfStrings (currentN - 1) (createBorderList :: currentList)
                    | _ when currentN = 1 
                        -> createListOfStrings (currentN - 1) (createBorderList :: currentList)
                    | _ -> createListOfStrings (currentN - 1) (createCommonList :: currentList)
        
        ///Непосредственно создание списка из строк, формирующих квадрат.
        createListOfStrings n []

///Функция, которая печатает квадрат по списку
let rec printSquare (list:list<_>) = 
    match list with 
    | [] -> None
    | head :: tail -> printfn "%s" head; printSquare tail
