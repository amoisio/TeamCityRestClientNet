dotnet test ./TeamCityRestClientNet.Tests/TeamCityRestClientNet.Tests.csproj \
--filter="FullyQualifiedName~BuildAgents \
    |FullyQualifiedName~BuildAgentPools \
    |FullyQualifiedName~Changes \
    |FullyQualifiedName~Projects \
    |FullyQualifiedName~VcsRoots \
    |FullyQualifiedName~Users" -v n