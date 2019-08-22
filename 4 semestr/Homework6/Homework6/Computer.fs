namespace Homework6

module Computer =

    open OperatingSystem

    /// Компьютер. Имеет свою операционную систему и статус заражения.
    type Computer(OS : OperatingSystem, isInfected : bool) = 
        member val OS = OS with get
        member val IsInfected = isInfected with get, set