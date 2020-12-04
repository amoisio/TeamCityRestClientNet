using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Domain;

namespace TeamCityRestClientNet.Console
{
    class Program
    {
        readonly static string _serverUrl = "http://localhost:5000/";
        readonly static string _username = "amoisio";
        readonly static string _password = "Testi#123";
        readonly static string _token = "eyJ0eXAiOiAiVENWMiJ9.Tkp4RUN4RGpWbl8wNy1KVG5EbmxsZXpWaDIw.ZTRmYTc3NDUtYTQ3OS00ZmMzLWJkMTAtMTU0OTE1YWVlOGI4";
        static void Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("TeamCityRestClientNet.Console.Program", LogLevel.Debug)
                    .AddConsole();

            });
            // ILogger logger = loggerFactory.CreateLogger<Program>();

            var teamCity = TeamCityInstanceFactory.TokenAuth(_serverUrl, _token, loggerFactory);            
            // var teamCity = TeamCityInstanceFactory.GuestAuth(_serverUrl, loggerFactory);
            System.Console.WriteLine("Getting users");
            Task.Run(async () =>
            {
                await foreach (var user in teamCity.Users.All())
                {
                    System.Console.WriteLine(user);
                }
            }).GetAwaiter().GetResult();

            System.Console.ReadKey();
        }
    }
}
