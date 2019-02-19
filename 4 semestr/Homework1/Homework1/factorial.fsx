let factorial x = 
    if x <= 0 then 1
    else
        let rec helpFact a b = 
            if b <= 1 then a
            else helpFact (a*b) (b-1)
        helpFact x (x-1)

