﻿using System;
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

            var logger = loggerFactory.CreateLogger("TeamCity.Console");
            var teamCity = TeamCityInstanceFactory.TokenAuth(_serverUrl, _token, logger);            

            Task.Run(async () =>
            {
                System.Console.WriteLine("Getting users as entities.");
                await foreach (var user in teamCity.Users.All())
                {
                    System.Console.WriteLine(user);
                }
            }).GetAwaiter().GetResult();

            System.Console.ReadKey();
        }
    }
}
