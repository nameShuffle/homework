module CountOfSymbols

open System.Net
open System.IO
open System.Text.RegularExpressions

/// Данная функция ищет все ссылки, по которым можно перейти с заданной странице и возвращает список с 
/// элементами в формате "ссылка --- количество символов на странице".
let checkAllUrls parentUrl = 

    /// Формирует async, который возвращает пару ( содержимое страницы * ответ в требуемом формате ),\
    /// либо None, если такой страницы не существует. 
    let getNumberOfSymbols (url : string) =
        async {
            try 
                let request = WebRequest.Create(url)
                use! response = request.AsyncGetResponse()
                use stream = response.GetResponseStream() 
                use reader = new StreamReader(stream)
                let html = reader.ReadToEnd()
                return Some ( html, url + " --- " + html.Length.ToString())
            with
                | _ -> return None
        }
    
    /// Данная функция находит все ссылки на переданной странице. Завершается с ошибкой, если 
    /// такой страницы не существует или возвращает пару (количество символов на переданной странице * массив ссылок).
    let getUrls (parentUrl : string) = 
        let pattern = "<a\shref=\"(https?:\/\/.*?)\".*?>"
        let result = parentUrl |> getNumberOfSymbols |> Async.RunSynchronously
        match result with
        | Some ( html, parentAnswer) -> 
            let text = html
            let regex = new Regex(pattern)
            let matches = regex.Matches(text)
            let urls = [ for url in matches -> url.Groups.[1].ToString() ]
            ( parentAnswer, urls )
        | None -> failwith "Проверьте корретность введенных данных"
    
    /// Функция расшифровки полученного результата. Выбирает из пары ответ в нужном формате, либо
    /// возвращает сообщение о некорретности url.
    let interpretResult result =
        match result with 
        | Some (html, answer) -> answer
        | None -> "incorrect url"
        
    let parentContent = getUrls parentUrl

    match parentContent with 
    | ( parentAnswer, urls ) ->
        let result = urls |> List.map (fun url -> url |> getNumberOfSymbols) |> Async.Parallel |> Async.RunSynchronously
        let answers = result |> Array.map interpretResult |> Array.toList
        parentAnswer :: answers

 /// Получение ответа в нужном формате и вывод на экран.
let printAllAnswers url =
    let result = checkAllUrls url
    List.iter (fun a -> printfn "%s" a) result

printAllAnswers "https://www.google.ru/"