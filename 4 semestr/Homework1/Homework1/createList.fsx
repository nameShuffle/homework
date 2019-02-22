let createList n m = 
    if n < 0 || m < 0 then failwith "переданные значения не входят в область определения функции"

    let rec createListToM x list k iter = 
        if iter = k then list
        else createListToM (x * 2) (list @ [x * 2]) k (iter + 1)

    let rec beforeN x n iter = 
        if iter = n then createListToM x ([x]) (m + n) n
        else beforeN (2 * x) n (iter + 1)

    beforeN 1 n 0