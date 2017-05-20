namespace FunPlayground

module Program = 
    open System
    open System.Net.Http

    [<EntryPoint>]
    let main argv = 
        use httpClient = new HttpClient()
        let server = StartHttpServer httpClient.SendAsync
        server.Wait()
        0