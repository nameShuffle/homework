namespace Tests

open NUnit.Framework
open FsUnit

module ``Tests for LambdaCalculus Task`` =
    open Reduction

    [<Test>]
    let ``test alpha conversion`` () =
        let term1 = Abstraction ('x', Application (Variable 'x', Variable 'a'))
        let term2 = Application (Variable 'x', Variable 'y')

        let expectedAnswer = Abstraction ('b', Application (Variable 'b', Application (Variable 'x', Variable 'y')))

        substitute term1 'a' term2 |> should equal expectedAnswer

    [<Test>]
    let ``test S K K = I`` () =
        let brackets1 = Abstraction ('x', Abstraction ('y', Abstraction ('z', Application (Application (Variable 'x', Variable 'z'), Application(Variable 'y', Variable 'z')))))
        let brackets2 = Abstraction ('x', Abstraction ('y', Variable 'x'))
        let lambda = Application (Application (brackets1, brackets2), brackets2)

        let expectedAnswer = Abstraction ('z', Variable 'z')

        betaReduction lambda |> should equal expectedAnswer

    [<Test>]
    let ``test K I = K*`` () =
        let brackets1 = Abstraction ('x', Abstraction ('y', Variable 'x'))
        let brackets2 = Abstraction ('x', Variable 'x')
        let lambda = Application (brackets1, brackets2)

        let expectedAnswer = Abstraction ('y', Abstraction ('x', Variable 'x'))

        betaReduction lambda |> should equal expectedAnswer
        
    [<Test>]
    let ``test (l_x.y) ((l_x.x x x) (l_x.x x x))`` () =
        let brackets1 = Abstraction ('x', Variable 'y')
        let brackets2 = Abstraction ('x', Application (Application (Variable 'x', Variable 'x'), Variable 'x'))
        let lambda = Application (brackets1, Application (brackets2, brackets2))

        let expectedAnswer = Variable 'y'

        betaReduction lambda |> should equal expectedAnswer

    [<Test>]
    let ``test term without normal form`` () =
        let brackets1 = Abstraction ('y', Application (Variable 'x', Variable 'y'))
        let brackets2 = Application (Variable 'x', Variable 'y')
        let lambda = Application (brackets1, brackets2)

        let expectedAnswer = Application (Variable 'x', Application (Variable 'x', Variable 'y'))

        betaReduction lambda |> should equal expectedAnswer