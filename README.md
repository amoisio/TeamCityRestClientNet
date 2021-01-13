# TeamCity REST Api Client for .NET

Client for TeamCity REST Api for .NET, which lets you manage TeamCity and its assets programmatically 
from a .NET application. Inspired by JetBrain's Kotlin based client [teamcity-rest-client](https://github.com/JetBrains/teamcity-rest-client).

Built on .NET Standard 2.1. Internally the library uses the [Refit](https://github.com/reactiveui/refit) library to 
simplify working with the TeamCity REST Api and Stephen Cleary's [AsyncEx](https://github.com/StephenCleary/AsyncEx) 
async/await helper library to provide "by lazy" like TeamCity entity properties.

Developed against the REST Api of, and known to work on, TeamCity version 2020.1.5. 

## Preliminary steps

Set up an [access token](https://www.jetbrains.com/help/teamcity/managing-your-user-account.html#Managing+Access+Tokens) in TeamCity. 
The client uses bearer tokens when authenticating against the TeamCity REST Api. 

## Constructing the client

At minimum, the TeamCity base url and the access token need to be provided.

```csharp
var serverUrl = "https://localhost:5000";
var token     = "eyJ0eXAiOiAiVENWMiJ9.Tkp4RUN4R...";
var teamCity  = new TeamCityServerBuilder()
    .WithServerUrl(serverUrl)
    .WithBearerAuthentication(token)
    .Build();
```

### Other builder options

| Method             | Description |
| ------------------ | ----------- |
| .WithQueryTimeout  | Sets the value after which REST calls timeout and returns an error. (not yet implemented) |
| .WithCSRF          | Sets the CSRF token for calls which are supposed to change system state such as POST, PUT and DELETE calls. (not yet implemented) |
| .WithRefitSettings | Overrides the default Refit settings. |
| .WithLogging       | Sets a Microsoft.Extensions.Logging.ILogger implementation used for logging. |
| .WithHandlers      | Adds additional HttpRequestMessage handlers to the REST Api call handling pipeline. |

## Supported operations

TODO

## Examples

TODO