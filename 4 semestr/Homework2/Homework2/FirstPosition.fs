module FirstPosition

let firstPosition element list =
    let rec helpFirstPosition list iter = 
        match list with 
        | h::t -> if element = h then Some(iter) else helpFirstPosition t (iter + 1)
        | _ -> None

    helpFirstPosition list 0