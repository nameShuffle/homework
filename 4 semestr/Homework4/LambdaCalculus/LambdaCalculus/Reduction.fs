module Reduction

let possibleVariables = Set.ofList [ 'a' .. 'z' ]

type LambdaTerm = 
    | Variable of char
    | Abstraction of char * LambdaTerm
    | Application of LambdaTerm * LambdaTerm


/// Замена свободных вхождений переменной oldVariable на newVariable.
let rec alphaConversion term oldVariable newVariable = 
    match term with
    | Variable x -> 
        if x = oldVariable then Variable newVariable
        else term
    | Abstraction (x, internalTerm) ->
        if x = oldVariable then term
        else Abstraction (x, alphaConversion internalTerm oldVariable newVariable)
    | Application (internalTerm1, internalTerm2) -> 
        Application (alphaConversion internalTerm1 oldVariable newVariable, 
            alphaConversion internalTerm2 oldVariable newVariable)


/// Поиск всех свободных переменных терма.
let findFreeVariables term = 
    let rec findFree currentTerm currentBounded currentList = 
        match currentTerm with 
        | Variable var ->
            if List.contains var currentBounded then currentList
            else var :: currentList
        | Abstraction (var, internalTerm) ->
            findFree internalTerm (var :: currentBounded) currentList 
        | Application (internalTerm1, internalTerm2) ->
            let list1 = findFree internalTerm1 currentBounded currentList
            let list2 = findFree internalTerm2 currentBounded currentList
            List.append list1 list2 
    
    findFree term [] []


/// Выполнение подстановки в терм.
let rec substitute term variable termToSubstitute = 
    match term with 
    | Variable x ->
        if x = variable then termToSubstitute
        else term
    | Abstraction (x, internalTerm) ->
        if x = variable then term
        else 
            // находим все свободные переменные в терме для подстановки
            let freeList = findFreeVariables termToSubstitute

            if not (List.contains x freeList) then Abstraction (x, substitute internalTerm variable termToSubstitute)
            else 
                // если имеем проблемную ситуацию, необходимо найти новую переменную, замена на которую может быть произведена корректно,
                // и провести альфа-конверсию.
                let internalFreeList = findFreeVariables internalTerm
                let unsuitableVariables = Set.ofList (List.concat [ freeList; internalFreeList; [ variable ]; [ x ] ])
                let suitableVariables = Set.difference possibleVariables unsuitableVariables

                let newVariable = Set.minElement suitableVariables
                let newTerm = alphaConversion internalTerm x newVariable
                Abstraction (newVariable, substitute newTerm variable termToSubstitute)
    | Application (internalTerm1, internalTerm2) ->
        Application (substitute internalTerm1 variable termToSubstitute,
            substitute internalTerm2 variable termToSubstitute)


/// Выполнение бета-редукции на верхнем уровне по нормальной стратегии - убираются все внешние редэксы.
let rec externalReduction term =
    match term with 
    | Application (internalTerm1, internalTerm2) -> 
        match internalTerm1 with 
        | Variable _ -> term
        | Abstraction (x, internalTerm) -> substitute internalTerm x internalTerm2 |> externalReduction
        | Application _ -> Application (internalTerm1 |> externalReduction, internalTerm2) |> externalReduction
    | _ -> term


/// Непосредственно сама бета-редукция по шагам - сначала внешние редэксы, затем на уровень глубже и так далее до тех
/// пор, пока это возможно.
let rec betaReduction term = 
    let step = externalReduction term
    match step with 
    | Abstraction (x, internalTerm) -> Abstraction (x, betaReduction internalTerm) 
    | _ -> step
