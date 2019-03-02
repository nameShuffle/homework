module Mergesort

let rec mergesort (list:list<_>) = 
    let rec split list left right =
        match list with
        | [] -> (left, right)
        | [a] -> (a::left, right)
        | a::b::tail -> split tail (a::left) (b::right)

    let rec sort pairOfLists currentResult =
        match pairOfLists with
        | ([], list) -> List.rev currentResult @ list
        | (list, []) -> List.rev currentResult @ list
        | (h1::t1, h2::t2) -> if h1 <= h2 then sort (t1, h2::t2) (h1::currentResult)
                                else sort (h1::t1, t2) (h2::currentResult)
    
    match list with 
    | [] -> list
    | [x] -> list
    | _ -> match split list [] [] with (left, right) -> sort (mergesort left, mergesort right) []
             
            