module PrimeNumbers

///Функция проверки числа на простоту.
let isPrime x =
    let rec findDivider currentNumber = 
        match currentNumber with 
        | _ when currentNumber * currentNumber > x -> true
        | _ when x % currentNumber = 0 -> false
        | _ -> findDivider (currentNumber + 2)
    
    match x with 
    | 0 -> false
    | 1 -> false
    | _ when x % 2 = 0 -> false
    | _ -> findDivider 3

///Создание бесконечной последовательности простых чисел.
let infinitPrimes = Seq.initInfinite (fun n -> n) |> Seq.filter isPrime
