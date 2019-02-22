let factorial x = 
    if x < 0 then failwith "переданное значение вне области определения функции"
    else
        if x = 0 then 1
        else
            let rec helpFactorial a b = 
                if b <= 1 then a
                else helpFactorial (a * b) (b - 1)
            helpFactorial x (x - 1)