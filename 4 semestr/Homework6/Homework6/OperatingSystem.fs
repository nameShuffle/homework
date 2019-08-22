namespace Homework6

module OperatingSystem = 

    /// Операционная система. Обладает своим именем и шансом заразиться.
    type OperatingSystem (name : string, chance : int) = 
        do 
            if chance > 100 || chance < 0 then failwith "Некорректный шанс"

        member val Name = name with get
        member val Chance = chance with get 