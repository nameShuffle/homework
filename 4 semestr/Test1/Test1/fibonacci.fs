module fibonacci

///Вычисление n-го числа Фибоначчи
let fibonacci n = 
    if n < 1 then failwith "переданное значение вне области определения функции"
    else
        if n <= 2 then 1
        else
            let rec helpFibonacci fibonacciNumber1 fibonacciNumber2 step = 
                if step = 0 then fibonacciNumber1
                else helpFibonacci fibonacciNumber2 (fibonacciNumber1 + fibonacciNumber2) (step - 1)

            helpFibonacci 0 1 n

///Вычисление суммы четных чисел Фибоначчи, не превосходящих 1000000
let fibonacciSum =
    let rec helpFibonacciSum n currentSum =
        let currentFibonacciNumber = fibonacci n
        match currentFibonacciNumber with
        | _ when currentFibonacciNumber > 1000000 -> currentSum
        | _ when currentFibonacciNumber % 2 = 0 -> helpFibonacciSum  (n + 1) (currentSum + currentFibonacciNumber)
        | _ -> helpFibonacciSum (n + 1) currentSum

    helpFibonacciSum 1 0 