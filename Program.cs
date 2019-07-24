using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;

namespace BABsConsoleTemplate
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices(args);
            var app = serviceProvider.GetRequiredService<Application>();
            app.Execute();
            
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        public static IServiceProvider ConfigureServices(string[] args)
        {
            var configuration = BuildConfiguration(args);

            var services = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<Serilog.ILogger>(s =>
                {
                    return new LoggerConfiguration()
                        .ReadFrom.Configuration(s.GetRequiredService<IConfiguration>())
                        .CreateLogger();
                })
                .AddSingleton<Application>();

            NextGenServerOptions options = new NextGenServerOptions();
            configuration.GetSection("NextGenServerOptions").Bind(options);

            services.AddHttpClient()
                .AddHttpClient<MirthWebClient>(c =>
                {
                    c.BaseAddress = new Uri(options.BaseUri);
                    c.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                              Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", options.UserName, options.Password))));
                }).ConfigureHttpMessageHandlerBuilder(h =>
                    {
                        h.PrimaryHandler = new System.Net.Http.HttpClientHandler()
                        {
                            ServerCertificateCustomValidationCallback = delegate { return true; },
                        };
                    }
                    );
            return services.BuildServiceProvider();
        }

        public static IConfiguration BuildConfiguration(string[] args)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT");
            if (string.IsNullOrWhiteSpace(environment))
                throw new ArgumentNullException("Environment not found in ASPNETCORE_ENVIROMENT");

            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json")
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
        }
    }
}
