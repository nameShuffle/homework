let reverse (list:list<_>) =
    let rec reverseHelp (list:list<_>) (list2:list<_>) currentEl =
        if currentEl = -1 then list2
        else
            reverseHelp list (list2 @ [list.Item(currentEl)]) (currentEl - 1)

    if list.Length = 1 || list.Length = 0 then list
    else reverseHelp list [] (list.Length - 1)