namespace FunPlayground

module Tests =

    open System
    open System.Net
    open System.Net.Http
    open System.Threading.Tasks
    open Xunit

    let getResult (url : string) =
        use client = new HttpClient()
        let response = client.GetStringAsync url
        response.Result

    let makeFakeResponse content =
        let response = new HttpResponseMessage()
        response.Content <- new StringContent(content)
        Task.FromResult(response)

    [<Fact>]
    let UrlContentIsConvertedToUppercase () =
        let expected = "RESPONSE FROM EXTERNAL URL"
        let fakeResponse = makeFakeResponse "Response from external URL"
        let httpSend : HttpRequestMessage -> Task<HttpResponseMessage> =
            fun request -> fakeResponse
        let sut = StartHttpServer httpSend

        let result = getResult "http://localhost:12313/api/upper" 

        Assert.Equal(expected, result)
 
