module BinaryTree

/// Бинарное дерево.
/// Заданно с помощью размеченного объединения: является либо кортежем, 
/// состоящим из элемента и двух поддеревьев, либо пустым значением.
type Tree<'a> =
    | Tree of 'a * Tree<'a> * Tree<'a>
    | Null


/// Задача 2: map для деревьев.
/// На вход принимает бинарное дерево и функцию, применяет переданную функцию
/// к каждому элементу дерева, возвращает измененное дерево.
let rec mapWithTree func tree =
    match tree with 
    | Tree(element, left, right) -> Tree(func element, mapWithTree func left, mapWithTree func right) 
    | Null -> Null


(*
/// Задача 3: подсчет значения дерева разбора арифметического выражения. 
/// Непосредственно сам обход дерева и подсчет арифметического выражения.
let rec calculateParseTree tree = 
    try
        match tree with
        | Tree(element, left, right) ->
            match element with
            | "+" -> Some(Option.get(calculateParseTree left) + Option.get(calculateParseTree right))
            | "-" -> Some(Option.get(calculateParseTree left) - Option.get(calculateParseTree right))
            | "/" -> Some(Option.get(calculateParseTree left) / Option.get(calculateParseTree right))
            | "*" -> Some(Option.get(calculateParseTree left) * Option.get(calculateParseTree right))
            | _ ->
                try
                    Some(float element)
                with
                | :? System.FormatException -> raise(System.ArgumentException("Все аргументы должны быть числами!"))
        | Null -> raise(System.ArgumentException("Дерево разбора не должно быть пустым и каждая операция должна иметь два аргумента!"))
    with
    | :? System.ArgumentException as ex -> printfn "%s" ex.Message; None
*)