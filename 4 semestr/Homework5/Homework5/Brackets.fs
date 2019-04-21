module Brackets

///Проверка корректности расстановки скобок в строке. Всего может быть три вида скобок:
/// [], {}, ()
/// Корретность расстановки другого вида скобок не проверяется.
let brackets string = 
    let stringInList = Seq.toList string

    let rec handleString currentString listOfOpenBrackets =  
        match currentString with
        | head :: tail -> 
            if head = '(' || head = '[' || head = '{' then
                handleString tail (head :: listOfOpenBrackets)
            else if head = ')' || head = ']' || head = '}' then
                match listOfOpenBrackets with
                | headBrackets :: tailBrackets ->
                    if headBrackets = '(' && head = ')' then
                        handleString tail tailBrackets
                    else if headBrackets = '{' && head = '}' then
                        handleString tail tailBrackets
                    else if headBrackets = '[' && head = ']' then 
                        handleString tail tailBrackets
                    else false
                | _ -> false
            else handleString tail listOfOpenBrackets
        | _ -> 
            match listOfOpenBrackets with 
            | head :: tail -> false
            | _ -> true

    handleString stringInList []