namespace FunPlayground

module Program = 
    open System
    open System.Net.Http
    open SelfHost

    [<EntryPoint>]
    let main argv = 
        use httpClient = new HttpClient()
        let server = CreateHttpServer httpClient.SendAsync
        server.Wait()
        0