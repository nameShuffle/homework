module PointFree

open NUnit.Framework
open FsCheck
open FsUnit
open PointFree

module ``Tests for PointFree Task`` =
    let checkFunc'1 x list= 
        func'1 x list = func x list

    let checkFunc'2 x list = 
        func'2 x list = func x list

    let checkFunc'3 x list = 
        func'3 x list = func x list

    let checkFunc'4 x list = 
        func'4 x list = func x list

    [<Test>]
    let ``test func'1`` () =
        Check.QuickThrowOnFailure checkFunc'1
    
    [<Test>]
    let ``test func'2`` () =
        Check.QuickThrowOnFailure checkFunc'2

    [<Test>]
    let ``test func'3`` () =
        Check.QuickThrowOnFailure checkFunc'3

    [<Test>]
    let ``test func'4`` () =
        Check.QuickThrowOnFailure checkFunc'4

    
