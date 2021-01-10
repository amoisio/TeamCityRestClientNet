using Xunit;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Tests;

namespace TeamCityRestClientNet.BuildTypes
{
    public class BuildTypes : TestsBase
    {
        [Fact]
        public async Task Can_be_retrieved_by_GETting_the_buildTypes_end_point()
        {
            await _teamCity.BuildTypes.All().ToListAsync();

            AssertApiCall(HttpMethod.Get, "/app/rest/buildTypes");
        }
    }

    public class NewBuildType : TestsBase
    {
        [Fact]
        public async Task Can_be_created_by_POSTing_to_buildTypes_end_point_with_build_type_xml_string()
        {
            var project = await _teamCity.Projects.ById("_Root");

            await project.CreateBuildType("BuildType");

            AssertApiCall(HttpMethod.Post, "/app/rest/buildTypes",
                apiCall => {
                    var body = apiCall.XmlContentAs<NewBuildTypeDescription>();
                    Assert.Equal("BuildType", body.Name);
                });
        }
    }

    public class ExistingBuildType : TestsBase
    {
        [Fact]
        public async Task Can_be_retrieved_by_GETting_the_buildType_end_point_with_id()
        {
            await _teamCity.BuildTypes.ById("TeamCityRestClientNet_RestClient");

            AssertApiCall(HttpMethod.Get, "/app/rest/buildTypes/TeamCityRestClientNet_RestClient");
        }

        [Fact]
        public async Task Tags_can_be_retrieved_by_GETting_the_buildTypes_tags_end_point_with_id()
        {
            var type = await _teamCity.BuildTypes.ById("TeamCityRestClientNet_RestClient");

            await type.BuildTags;

            AssertApiCall(HttpMethod.Get, "/app/rest/buildTypes/TeamCityRestClientNet_RestClient/buildTags");
        }

        [Fact]
        public async Task Triggers_can_be_retrieved_by_GETting_the_buildTypes_triggers_end_point_with_id()
        {
            var type = await _teamCity.BuildTypes.ById("TeamCityRestClientNet_RestClient");

            await type.FinishBuildTriggers;

            AssertApiCall(HttpMethod.Get, "/app/rest/buildTypes/TeamCityRestClientNet_RestClient/triggers");
        }

        [Fact]
        public async Task Artifact_dependencies_can_be_retrieved_by_GETting_the_buildTypes_artifact_dependencies_end_point_with_id()
        {
            var type = await _teamCity.BuildTypes.ById("TeamCityRestClientNet_RestClient");

            await type.ArtifactDependencies;

            AssertApiCall(HttpMethod.Get, "/app/rest/buildTypes/TeamCityRestClientNet_RestClient/artifact-dependencies");
        }

        [Fact]
        public async Task Parameters_can_be_set_by_PUTting_the_buildTypes_parameters_end_point_with_id_name_and_value()
        {
            var type = await _teamCity.BuildTypes.ById("TeamCityRestClientNet_RestClient");

            await type.SetParameter("parameter1", "value1");

            AssertApiCall(HttpMethod.Put, "/app/rest/buildTypes/TeamCityRestClientNet_RestClient/parameters/parameter1",
                apiCall => Assert.Equal("value1", apiCall.Content));

        }
    }
}
