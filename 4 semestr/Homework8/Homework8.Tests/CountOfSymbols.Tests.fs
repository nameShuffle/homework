namespace Tests

open NUnit.Framework
open FsUnit

module ``Tests for CountOfSymbols task`` =
    open CountOfSymbols

    [<Test>]
    let ``test incorrect url address`` () =
        (fun () -> checkAllUrls "https://ww.google.ru/" |> ignore) |> should 
            (throwWithMessage "Incorrect parent url") typeof<System.Exception>

    [<Test>]
    let ``test count of urls`` () =
        let result = checkAllUrls "https://www.google.ru/" 
        result.Length |> should equal 3


   
