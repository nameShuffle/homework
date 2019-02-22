let fibonacci n = 
    if n < 1 then failwith "переданное значение вне области определения функции"
    else
        if n <= 2 then 1
        else
            let rec helpFibonacci a b n = 
                if n = 0 then a
                else helpFibonacci b (a + b) (n - 1)
            helpFibonacci 0 1 n
