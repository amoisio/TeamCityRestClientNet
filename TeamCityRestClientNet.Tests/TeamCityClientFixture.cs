using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.FakeServer;
using TeamCityRestClientNet.RestApi;
using Xunit;

namespace TeamCityRestClientNet.Tests
{

    [CollectionDefinition("TeamCity Collection")]
    public class TeamCityCollection : ICollectionFixture<TeamCityFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    public class TeamCityFixture
    {
        public readonly string serverUrl = "http://localhost:5000";
        readonly static string _token = "eyJ0eXAiOiAiVENWMiJ9.Tkp4RUN4RGpWbl8wNy1KVG5EbmxsZXpWaDIw.ZTRmYTc3NDUtYTQ3OS00ZmMzLWJkMTAtMTU0OTE1YWVlOGI4";

        public TeamCityFixture()
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("TeamCity.Tests", LogLevel.Trace)
                    .AddConsole();
            });

            var guid = Guid.NewGuid().ToString().Substring(0,6);
            Logger = loggerFactory.CreateLogger($"TeamCity.Tests.{guid}");

            this.Handler = new RedirectToFakeServer(new FakeServer.FakeServer(Logger));
            this.TeamCity = new TeamCityServerBuilder()
              .WithServerUrl(serverUrl)
              .WithBearerAuthentication(_token)
              .WithLogging(Logger)
              .WithHandlers((innerHandler) => this.Handler)
              .Build();
        }

        public TeamCity TeamCity { get; }

        public RedirectToFakeServer Handler { get; }

        public ILogger Logger { get; }
    }
}