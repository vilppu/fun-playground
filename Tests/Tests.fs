namespace FunPlayground

module Tests =

    open System
    open System.Net
    open System.Net.Http
    open System.Threading.Tasks
    open Xunit

    let private getResult (url : string) =
        use client = new HttpClient()
        let response = client.GetStringAsync url
        response.Result

    let private makeFakeResponse content =
        let response = new HttpResponseMessage()
        response.Content <- new StringContent(content)
        Task.FromResult(response)

    let private startServer fakeHttpSend =
        let server = CreateHttpServer fakeHttpSend
        server.Start()
        server

    [<Fact>]
    let UrlContentIsConvertedToUppercase () =
        let expected = "RESPONSE FROM EXTERNAL URL"
        let fakeResponse = makeFakeResponse "Response from external URL"
        let fakeHttpSend : HttpRequestMessage -> Task<HttpResponseMessage> =
            fun request -> fakeResponse
        use sut = startServer fakeHttpSend

        let result = getResult "http://localhost:12313/api/upper/http%3A%2F%2Fwww.example.com%2F" 

        Assert.Equal(expected, result)
 

    [<Fact>]
    let HttpRequestIsMadeToCorrectUrl () =
        let expected = "http://www.example.com/"
        let mutable actual = ""
        let fakeResponse = makeFakeResponse "Response from external URL"
        let fakeHttpSend : HttpRequestMessage -> Task<HttpResponseMessage> =
            fun request ->
                actual <- request.RequestUri.ToString()
                fakeResponse
        use sut = startServer fakeHttpSend

        getResult "http://localhost:12313/api/upper/http%3A%2F%2Fwww.example.com%2F" 
        |> ignore

        Assert.Equal(expected, actual)
