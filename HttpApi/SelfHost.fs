namespace FunPlayground

[<AutoOpen>]
module SelfHost = 
    open System
    open System.IO
    open System.Net.Http
    open System.Threading.Tasks    
    open Microsoft.AspNetCore.Builder
    open Microsoft.AspNetCore.Hosting
    open Microsoft.AspNetCore.Mvc
    open Microsoft.Extensions.Configuration
    open Microsoft.Extensions.DependencyInjection
    open Microsoft.Extensions.Logging
    open Microsoft.Extensions.Configuration
    open Newtonsoft.Json
    open Newtonsoft.Json.Serialization
    
    type Startup(environment : IHostingEnvironment) =
        
        member this.Configure(app : IApplicationBuilder, env : IHostingEnvironment, loggerFactory : ILoggerFactory, httpSend : HttpRequestMessage -> Task<HttpResponseMessage>) =             
            loggerFactory
                .AddConsole(LogLevel.Warning)
                .AddDebug()
                |> ignore

            app.UseMvc()
            |> ignore
            
        member this.ConfigureServices(services : IServiceCollection) =
            let configureJson (options : MvcJsonOptions) = 
                options.SerializerSettings.ContractResolver <- CamelCasePropertyNamesContractResolver()
            let configureJsonAction = new Action<MvcJsonOptions>(configureJson)
            services
                .AddMvc()
                .AddJsonOptions(configureJsonAction)
                |> ignore

            |> ignore

    let CreateHttpServer (httpSend : HttpRequestMessage -> Task<HttpResponseMessage>) = 

        let host = 
            WebHostBuilder()
                .ConfigureServices(fun services -> services.AddSingleton(httpSend) |> ignore)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls("http://localhost:12313/")
                .Build()
        host

    let StartHttpServer (httpSend : HttpRequestMessage -> Task<HttpResponseMessage>) : Task =
        let server = CreateHttpServer httpSend
        Task.Run(fun () -> server.Run())
