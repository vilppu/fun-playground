namespace FunPlayground

module Program = 
    open System
    open System.Net.Http
    open System.Threading.Tasks

    [<EntryPoint>]
    let main argv = 
        use httpClient = new HttpClient()
        let server = StartHttpServer httpClient.SendAsync
        server.Wait()
        0