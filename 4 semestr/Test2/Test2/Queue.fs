module Queue

let rec getMaxValue list (currentMax, currentElement) =
    match list with
    | h :: t -> 
        match h with
        | (priority, _) -> 
            if priority > currentMax then
                getMaxValue t h
            else
                getMaxValue t (currentMax, currentElement)
    | [] -> currentElement

let rec getMinValue list (currentMin, currentElement) =
    match list with
    | h :: t -> 
        match h with
        | (priority, _) -> 
            if priority < currentMin then
                getMinValue t h
            else
                getMinValue t (currentMin, currentElement)
    | [] -> currentElement

type PriorityQueue() =
    let mutable list = []
    member this.Add priority element =
        list <- ((priority, element) :: list)
    member this.GetMaxValue () = 
        match list with
        | h :: t -> getMaxValue t h
        | [] -> failwith "queue is empty"
    member this.GetMinValue () =
        match list with
        | h :: t -> getMinValue t h
        | [] -> failwith "queue is empty"
    member this.IsEmty = List.isEmpty list



        
