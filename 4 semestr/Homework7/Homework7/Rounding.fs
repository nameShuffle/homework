module Rounding

open System

// Workflow Builder, позволяющий выполнять вычисления с заданной точностью.
// Передаваемый параметр number - точность, с которой необходимо производить вычисления.
type RoundingBuilder(number : int) =
    member this.Bind(x : float, func) =
        Math.Round(x, number) |> func
    member this.Return(x : float) =
        Math.Round(x, number)