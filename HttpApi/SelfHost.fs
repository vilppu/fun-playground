namespace FunPlayground

[<AutoOpen>]
module SelfHost = 
    open System
    open System.IO
    open System.Net
    open System.Net.Http
    open System.Threading
    open System.Threading.Tasks    
    open Microsoft.AspNetCore.Authorization
    open Microsoft.AspNetCore.Authentication.JwtBearer
    open Microsoft.AspNetCore.Builder
    open Microsoft.AspNetCore.Hosting
    open Microsoft.AspNetCore.Mvc
    open Microsoft.Extensions.Configuration
    open Microsoft.Extensions.DependencyInjection
    open Microsoft.Extensions.Logging
    open Microsoft.Extensions.Configuration
    open Microsoft.IdentityModel.Tokens
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

    let CreateHttpServer (httpSend : HttpRequestMessage -> Task<HttpResponseMessage>) : Task = 
        let url = Environment.GetEnvironmentVariable("YOG_BOT_BASE_URL")
        
        let url = 
            if String.IsNullOrWhiteSpace(url) then "http://localhost:12313/"
            else url

        let host = 
            WebHostBuilder()
                .ConfigureServices(fun services -> services.AddSingleton(httpSend) |> ignore)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls(url)
                .Build()

        Task.Run(fun () -> host.Run())
        
