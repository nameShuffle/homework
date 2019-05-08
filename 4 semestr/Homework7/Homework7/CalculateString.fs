module CalculateString

type CalculateStringBuilder() =
    member this.Bind(str : string, func) =
        match System.Int32.TryParse(str) with
        | (true, number) -> number |> func
        | _ -> None
    member this.Return(x) =
        Some x