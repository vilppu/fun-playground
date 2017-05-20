namespace FunPlayground

open System
open System.Net.Http
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc

[<Route("api")>]
type ApiController(httpSend : HttpRequestMessage -> Task<HttpResponseMessage>) = 
    inherit Controller()


    let getUppercaseContent = Application.GetUppercaseContent httpSend
    
    [<Route("upper")>]
    [<HttpGet>]
    member this.Upper (url : string) : Task<string> =
        getUppercaseContent url
