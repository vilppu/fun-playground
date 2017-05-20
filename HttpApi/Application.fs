namespace FunPlayground

module Application =
    open System.Net
    open System.Net.Http
    open System.Threading.Tasks

    let GetUppercaseContent (httpSend : HttpRequestMessage -> Task<HttpResponseMessage>) (url : string) : Task<string> =
        let getContent = UrlContent.GetContent httpSend

        async {
            let! content = getContent url |> Async.AwaitTask
            return (content.ToUpper())
        } |> Async.StartAsTask