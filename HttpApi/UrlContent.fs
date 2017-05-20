namespace FunPlayground

module UrlContent =
    open System.Net
    open System.Net.Http
    open System.Threading.Tasks

    let GetContent (httpSend : HttpRequestMessage -> Task<HttpResponseMessage>) (url : string) : Task<string> =
        use request = new HttpRequestMessage(HttpMethod.Get, url)
        async {
            let! response = httpSend request |> Async.AwaitTask 
            let! responseText = response.Content.ReadAsStringAsync() |> Async.AwaitTask    
            return responseText
        } |> Async.StartAsTask