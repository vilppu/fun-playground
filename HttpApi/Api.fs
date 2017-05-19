namespace FunPlayground

open System
open System.Net
open System.Net.Http
open System.Threading.Tasks
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc

[<Route("api")>]
type ApiController(httpSend : HttpRequestMessage -> Task<HttpResponseMessage>) = 
    inherit Controller()
    
    [<Route("yeah")>]
    [<HttpGet>]
    member this.Yeah() : Task<string> = Task.FromResult("yeah!")

