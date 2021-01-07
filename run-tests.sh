dotnet test ./TeamCityRestClientNet.Tests/TeamCityRestClientNet.Tests.csproj \
--filter="FullyQualifiedName~BuildAgents \
    |FullyQualifiedName~BuildAgentPools \
    |FUllyQualifiedNAme~BuildQueue \
    |FullyQualifiedName~Changes \
    |FullyQualifiedName~Projects \
    |FullyQualifiedName~VcsRoots \
    |FullyQualifiedName~Users" -v n