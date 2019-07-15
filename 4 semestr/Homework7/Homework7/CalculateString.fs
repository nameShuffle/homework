module CalculateString

// Workflow Builder, который позволяет выполнять вычисления с числами, записанными с помощью строк.
type CalculateStringBuilder() =
    member this.Bind(str : string, func) =
        match System.Int32.TryParse(str) with
        | (true, number) -> number |> func
        | _ -> None
    member this.Return(x) =
        Some x