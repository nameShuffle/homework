// Learn more about F# at http://fsharp.org

open System.Net
open System.IO
open System.Text.RegularExpressions

[<EntryPoint>]
let main argv =
    let pattern = "<a\shref=\"(https?://.*?)\">"
    let regex = new Regex(pattern)
    let text = File.ReadAllText("hello.txt")
    let get = regex.Matches(text)
    0 // return an integer exit code
