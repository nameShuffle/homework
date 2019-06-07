module CalculateTree

/// Дерево разбора.
type Tree =
    | Add of Tree * Tree
    | Subtract of Tree * Tree
    | Multiply of Tree * Tree
    | Divide of Tree * Tree
    | Value of float


/// Задача 3: подсчет значения дерева разбора арифметического выражения. 
/// Непосредственно сам обход дерева и подсчет арифметического выражения.
let rec calculateParseTree tree = 
    match tree with
    | Add (left, right) -> (calculateParseTree left) + (calculateParseTree right)
    | Subtract (left, right) -> (calculateParseTree left) - (calculateParseTree right)
    | Multiply (left, right) -> (calculateParseTree left) * (calculateParseTree right)
    | Divide (left, right) -> (calculateParseTree left) / (calculateParseTree right)
    | Value a -> a