using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.Service;
using System.Xml.Linq;

namespace TeamCityRestClientNet.Domain
{
    class Project : Base<ProjectDto>, IProject 
    {
        public Project(ProjectDto dto, bool isFullDto, TeamCityInstance instance)
            : base(dto, isFullDto, instance)
        {
            
        }

        public ProjectId Id => new ProjectId(IdString);

        public string Name => NotNull(dto => dto.Name);

        public bool Archived => Nullable(dto => dto.Archived) ?? false;

        public ProjectId? ParentProjectId 
            => Nullable(dto => dto.ParentProjectId)
                .Let(dto => new ProjectId(dto));

        public List<IProject> ChildProjects 
            => NotNull(dto => dto.Projects).Project
                .Select(project => new Project(project, false, Instance))
                .ToList<IProject>();
        public List<IBuildConfiguration> BuildConfigurations 
            => this.FullDto.BuildTypes
                ?.BuildType
                .Select(type => new BuildConfiguration(type, false, Instance))
                .ToList<IBuildConfiguration>()
                ?? throw new NullReferenceException();

        public List<IParameter> Parameters 
            => NotNull(dto => dto.Parameters?.Property)
                .Select(prop => new Parameter(prop))
                .ToList<IParameter>();

        public IBuildConfiguration CreateBuildConfiguration(string buildConfigurationDescriptionXml)
        {
            var dto = Service.CreateBuildType(buildConfigurationDescriptionXml).GetAwaiter().GetResult();
            return new BuildConfiguration(dto, false, Instance);
        }

        public IProject CreateProject(ProjectId id, string name)
        {
            var xml = new XElement("newProjectDescription",
                new XAttribute("name", name),
                new XAttribute("id", Id.stringId),
                new XElement("parentProject", 
                    new XAttribute("locator", $"id:{Id.stringId}")
                )
            );
            var projectDto = Service.CreateProject(xml.ToString()).GetAwaiter().GetResult();
            return new Project(projectDto, true, Instance);
        }

        public IVcsRoot CreateVcsRoot(VcsRootId id, string name, VcsRootType type, IDictionary<string, string> properties)
        {
            var propElement = new XElement("properties");
            foreach(var prop in properties.OrderBy(prop => prop.Key)) {
                propElement.Add(new XElement("property",
                    new XAttribute("name", prop.Key),
                    new XAttribute("value", prop.Value)));
            }

            var xml = new XElement("vcs-root", 
                new XAttribute("name", name),
                new XAttribute("id", Id.stringId),
                new XAttribute("vcsName", type.stringType),
                new XElement("project",
                    new XAttribute("id", IdString)
                ),
                propElement);

            var vcsRootDto = Service.CreateVcsRoot(xml.ToString()).GetAwaiter().GetResult();
            return new VcsRoot(vcsRootDto, true, Instance);
        }

        public string GetHomeUrl(string branch = null)
            => Instance.GetUserUrlPage(
                "project.html", 
                projectId: Id, 
                branch: branch);

        public string GetTestHomeUrl(TestId testId)
            => Instance.GetUserUrlPage(
                "project.html",
                projectId: Id,
                testNameId: testId,
                tab: "testDetails");

        public void SetParameter(string name, string value)
        {
        //         LOG.info("Setting parameter $name=$value in ProjectId=$idString")
            Service.SetProjectParameter(Id.stringId, name, value)
                .GetAwaiter().GetResult();
        }

        public override string ToString()
            => $"Project(id={IdString},name={Name})";

        protected override async Task<ProjectDto> FetchFullDto()
            => await Service.Project(Id.stringId);
    }
}