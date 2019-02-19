let fib n = 
    if n <= 2 then 1
    else
        let rec helpFib a b n = 
            if n = 0 then a
            else helpFib b (a+b) (n-1)
        helpFib 0 1 n


