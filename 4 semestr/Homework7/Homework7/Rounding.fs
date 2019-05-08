module Rounding

open System

type RoundingBuilder(number : int) =
    member this.Bind(x : float, func) =
        x |> func
    member this.Return(x : float) =
        Math.Round(x, number)