namespace Tests

open NUnit.Framework
open FsUnit

open Lazy

module ``Tests for SimpleLazy task`` =
    
    let random = System.Random()

    [<Test>]
    let ``test simple lazy`` () =
        let simpleLazy = LazyFactory.CreateSimpleLazy(fun() -> 20)
        
        simpleLazy.Get() |> should equal 20

    [<Test>]
    let ``getting result several times with simple lazy`` () =
        let simpleLazy = LazyFactory.CreateSimpleLazy(fun() -> random.Next(0, 100))
        let firstResult = simpleLazy.Get()
        
        for _ in 0 .. 5 do
            simpleLazy.Get() |> should equal firstResult


module ``Tests for ProtectedLazy task`` =
    
    let random = System.Random()

    [<Test>]
    let ``test protected lazy`` () =
        let protectedLazy = LazyFactory.CreateProtectedLazy(fun() -> 20)
        
        protectedLazy.Get() |> should equal 20

    [<Test>]
    let ``getting result several times with protected lazy`` () =
        let protectedLazy = LazyFactory.CreateProtectedLazy(fun() -> random.Next(0, 100))
        let firstResult = protectedLazy.Get()
        
        for _ in 0 .. 5 do
            protectedLazy.Get() |> should equal firstResult

    [<Test>]
    let ``test protected lazy with several threads`` () =
        let mutable value = 0
        let adding () = 
            value <- value + 1
            value
        
        let protectedLazy = LazyFactory.CreateProtectedLazy(adding)
        let thread () = 
            async { return protectedLazy.Get() }
        let results = Array.init 100 (fun _ -> thread()) |> Async.Parallel |> Async.RunSynchronously
        
        for result in results do
            result |> should equal 1


module ``Tests for LockFreeLazy task`` =
    
    let random = System.Random()

    [<Test>]
    let ``test lock-free lazy`` () =
        let lockFreeLazy = LazyFactory.CreateLockFreeLazy(fun() -> 20)
        
        lockFreeLazy.Get() |> should equal 20

    [<Test>]
    let ``getting result several times with lock-free lazy`` () =
        let lockFreeLazy = LazyFactory.CreateLockFreeLazy(fun() -> random.Next(0, 100))
        let firstResult = lockFreeLazy.Get()
        
        for _ in 0 .. 5 do
            lockFreeLazy.Get() |> should equal firstResult

    [<Test>]
    let ``test lock-free lazy with several threads`` () =
        let mutable value = 0
        let adding () = 
            value <- value + 1
            value
        
        let lockFreeLazy = LazyFactory.CreateLockFreeLazy(adding)
        let thread () = 
            async { return lockFreeLazy.Get() }
        let results = Array.init 100 (fun _ -> thread()) |> Async.Parallel |> Async.RunSynchronously
        
        for result in results do
            result |> should equal 1