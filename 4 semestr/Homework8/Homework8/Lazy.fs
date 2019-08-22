module Lazy

open System.Threading

/// Интерфейс, предоставляющий ленивое вычисление.
type ILazy<'a> =
    /// Возвращает результат вычислений. При первом вызове значение вычисляется,
    /// в остальных случаях должно вернуться уже вычисленное.
    abstract member Get: unit -> 'a


/// Ленивое вычисление без гарантии корректной работы в многопоточном режиме.
type SimpleLazy<'a> (supplier : unit -> 'a) = 
    
    let mutable result : option<'a> = None

    interface ILazy<'a> with
        member this.Get() =
            match result with
            | None ->
                result <- Some(supplier())
                Option.get result
            | Some answer -> answer


/// Ленивое вычисление с гарантией корректной работы в многопоточном режиме с 
/// использованием блокировок.
type ProtectedLazy<'a> (supplier : unit -> 'a) = 

    let lockObject = obj()
    let mutable result : option<'a> = None

    interface ILazy<'a> with
        member this.Get() =
            match Volatile.Read(&result) with
            | None ->
                lock lockObject (fun() ->
                match Volatile.Read(&result) with
                | None ->
                    Volatile.Write(&result, Some(supplier()))
                    Option.get result
                | Some answer -> answer
                )
            | Some answer -> answer


/// Lock-free версия ленивого вычисления с гарантией корректной работы в многопоточном режиме.
type LockFreeLazy<'a> (supplier : unit -> 'a) = 

    let mutable result : option<'a> = None

    interface ILazy<'a> with
        member this.Get() =
            let newResult = Some(supplier())
            Interlocked.CompareExchange(&result, newResult, None) |> ignore
            Option.get result


/// Данный класс позволяет создавать объекты всех трех видов ленивый вычислений.
type LazyFactory = 

    /// Создание ленивого вычисления без гарантии корректной работы в многопоточном режиме.
    static member CreateSimpleLazy (supplier : unit -> 'a) =
        new SimpleLazy<'a>(supplier) :> ILazy<'a>

    /// Создание ленивого вычисления с гарантией корректной работы в многопоточном режиме.
    static member CreateProtectedLazy (supplier : unit -> 'a) =
        new ProtectedLazy<'a>(supplier) :> ILazy<'a>

    /// Создание lock-free версии ленивого вычисления с гарантией корректной работы 
    /// в многопоточном режиме.
    static member CreateLockFreeLazy (supplier : unit -> 'a) =
        new LockFreeLazy<'a>(supplier) :> ILazy<'a>


