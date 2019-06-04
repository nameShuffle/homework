namespace Homework6

open System

module Network = 

    open Computer
    
    /// Класс, позволяющий моделировать работу локальной сети. 
    /// Для создания новой сети необходимо передать массив компьютеров и матрицу смежности, которая отвечает
    /// за соединение компьютеров в данной сети. 
    type Network(computers : array<Computer>, net : int [] []) = 
        let numberOfComputers = Array.length computers

        let mutable net = net

        let random = new Random()

        do 
            if numberOfComputers <> net.Length then failwith "Некорректное задание сети"

        /// Изменение соединений в данной сети.
        member this.ChangeNetwork(network : int [] []) = 
            net <- network
        
        /// Получение списка компьютеров с операционной системой и состоянием каждого.
        member this.getStatus() = 
            let rec createList currentId currentList =
                if currentId = numberOfComputers then currentList
                    else
                        if computers.[currentId].IsInfected then
                            let newList = currentId.ToString() + " " + computers.[currentId].OS.Name + " - infected" :: currentList
                            createList (currentId + 1) newList
                        else 
                            let newList = currentId.ToString() + " " + computers.[currentId].OS.Name + " - not infected" :: currentList
                            createList (currentId + 1) newList
            createList 0 []
        
        /// Один шаг моделирования - заражаются компьтеры, ближайшие к зараженному.
        member this.Simulate() =
            let mutable infectedList = []
            for id in 0 .. (numberOfComputers - 1) do 
                if computers.[id].IsInfected then 
                    for idConnected in 0 .. (id - 1) do
                        if net.[id].[idConnected] = 1 then 
                            let chance = random.Next(0, 100)
                            if chance <= computers.[idConnected].OS.Chance then
                                infectedList <- computers.[idConnected] :: infectedList 
            for computer in infectedList do
                computer.IsInfected <- true




