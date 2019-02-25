let factorial x = 
    if x < 0 then failwith "переданное значение вне области определения функции"
    else
        if x = 0 then 1
        else
            let rec helpFactorial currentResult currentFactor = 
                if currentFactor <= 1 then currentResult
                else helpFactorial (currentResult * currentFactor) (currentFactor - 1)
            helpFactorial x (x - 1)


let fibonacci n = 
    if n < 1 then failwith "переданное значение вне области определения функции"
    else
        if n <= 2 then 1
        else
            let rec helpFibonacci fibonacciNumber1 fibonacciNumber2 step = 
                if step = 0 then fibonacciNumber1
                else helpFibonacci fibonacciNumber2 (fibonacciNumber1 + fibonacciNumber2) (step - 1)

            helpFibonacci 0 1 n


let reverse (list:list<_>) =
    let rec reverseHelp (list1:list<_>) (list2:list<_>) numberOfElement =
        if numberOfElement = list.Length then list2
        else
            reverseHelp list1.Tail (list1.Head :: list2) (numberOfElement + 1)

    if list.Length = 1 || list.Length = 0 then list
    else reverseHelp list [] 0


let createList n m = 
    if n < 0 || m < 0 then failwith "переданные значения не входят в область определения функции"

    let rec createListToM x list k iter = 
        if iter = k then reverse list
        else createListToM (x * 2) ((x * 2) :: list) k (iter + 1)

    let rec beforeN x n iter = 
        if iter = n then createListToM x ([x]) (m + n) n
        else beforeN (2 * x) n (iter + 1)

    beforeN 1 n 0

