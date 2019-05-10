module HashTable

/// Функция для удаления первого вхождения элемента в список.
/// На вход принимает список и элемент для удаления, возвращает
/// список без первого вхождения данного элемента, если он был найдет.
let rec removeElementFromList list element =
    match list with 
    | head :: tail -> 
        if head = element then tail
        else head :: (removeElementFromList tail element)
    | _ -> []

/// Класс, реализующий хеш-таблицу.
/// Для создания своей хеш-таблицы необходимо передать хеш-функцию и количество строк в таблице.
/// Предполагается, что передаваемая хеш-функция возвращает произвольный int.
type HashTable (hashFunction : (obj -> int), numberOfElements) = 
    let mutable table = [| for _ in 1 .. numberOfElements -> [] |]

    member this.Add(element) = 
        let index = (hashFunction element) % numberOfElements
        if index < 0 then 
            table.[-index] <- element :: table.[-index]
        else 
            table.[index] <- element :: table.[index]

    member this.Delete(element) = 
        let index = (hashFunction element) % numberOfElements
        if index < 0 then 
            table.[-index] <- removeElementFromList table.[-index] element
        else
            table.[index] <- removeElementFromList table.[index] element

    member this.isExist(element) = 
        let index = (hashFunction element) % numberOfElements
        if index < 0 then 
            List.contains element table.[-index] 
        else 
            List.contains element table.[index] 