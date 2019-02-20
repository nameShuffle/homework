let reverse (list:list<_>) =
    let rec reverseHelp (list:list<_>) (list2:list<_>) currentEl =
        if currentEl = -1 then list2
        else
            reverseHelp list (list2 @ [list.Item(currentEl)]) (currentEl - 1)
    match list.Length with
    | 1 -> list
    | _ -> reverseHelp list [] (list.Length - 1)

reverse ['a';'b']