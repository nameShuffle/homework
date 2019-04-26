namespace Tests

open NUnit.Framework
open FsUnit

module ``Tests for FindElement Task`` =
    open Queue

    let queue = PriorityQueue()
    queue.Add 2 1
    queue.Add 1 3

    [<Test>]
    let ``getting element with max priority`` () =
        queue.GetMaxValue() |> should equal 1
    
    [<Test>]
    let ``getting element with min priority`` () =
        queue.GetMinValue() |> should equal 3
    
    [<Test>]
    let ``empty queue`` () =
        let queue2 = PriorityQueue()
        queue2.IsEmty |> should equal true
    