using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Threading.Tasks;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.RestApi;
using System.Xml.Linq;
using Nito.AsyncEx;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace TeamCityRestClientNet.Domain
{
    class Project : Base<ProjectDto>, IProject
    {
        private Project(ProjectDto dto, TeamCityServer instance)
            : base(dto, instance)
        {
            this.ChildProjects = new AsyncLazy<List<IProject>>(async () 
                => {
                    var tasks = dto.Projects.Items
                        .Select(proj => Project.Create(proj, false, instance));
                    var projects = await Task.WhenAll(tasks).ConfigureAwait(false);
                    return projects.ToList();
                });

            this.BuildTypes = new AsyncLazy<List<IBuildType>>(async ()
                => {
                    var tasks = dto.BuildTypes.Items
                        .Select(type => BuildType.Create(type.Id, instance));
                    var configs = await Task.WhenAll(tasks).ConfigureAwait(false);
                    return configs.ToList();                    
                });

        }

        public static async Task<IProject> Create(ProjectDto dto, bool isFullDto, TeamCityServer instance)
        {
            var fullDto = isFullDto
                ? dto
                : await instance.Service.Project(dto.Id).ConfigureAwait(false);
            return new Project(fullDto, instance);
        }

        public bool Archived => Dto.Archived ?? false;
        public Id? ParentProjectId
            => Dto.ParentProjectId.Let(id => new Id(Dto.ParentProjectId));
        public AsyncLazy<List<IProject>> ChildProjects { get; }
        public AsyncLazy<List<IBuildType>> BuildTypes { get; }
        public List<IParameter> Parameters
            => Dto.Parameters
                ?.Property
                .Select(prop => new Parameter(prop))
                .ToList<IParameter>();

        public async Task<IBuildType> CreateBuildType(string name, string sourceBuildTypeLocator = null, bool copyAllAssociatedSettings = false, bool shareVCSRoots = false)
        {
            var dto = new NewBuildTypeDescription
            {
                Name = name,
                SourceBuildTypeLocator = sourceBuildTypeLocator,
                CopyAllAssociatedSettings = copyAllAssociatedSettings,
                ShareVCSRoots = shareVCSRoots
            };
            return await CreateBuildType(dto);
        }
        
        private async Task<IBuildType> CreateBuildType(NewBuildTypeDescription dto)
        {
            var xml = new StringBuilder();
            using (var tw = new StringWriter(xml))
            {
                var serializer = new XmlSerializer(typeof(NewBuildTypeDescription));
                serializer.Serialize(tw, dto);
            }

            var buildTypeDto = await Service.CreateBuildType(xml.ToString()).ConfigureAwait(false);
            return await BuildType.Create(buildTypeDto.Id, Instance).ConfigureAwait(false);
        }

        /// <summary>
        /// Create a new child project.
        /// </summary>
        /// <param name="id">Id of the new project.</param>
        /// <param name="name">Name of the new project</param>
        /// <returns>The created project. Throws an ApiException is project cannot be created.</returns>
        public async Task<IProject> CreateProject(Id id, string name)
        {
            var xmlDto = new NewProjectDescriptionDto
            {
                Name = name,
                Id = id.StringId,
                ParentProject = new ProjectLocatorDto
                {
                    Locator = $"id:{Id}"
                }
            };

            var xml = new StringBuilder();
            using (var tw = new StringWriter(xml))
            {
                var serializer = new XmlSerializer(typeof(NewProjectDescriptionDto));
                serializer.Serialize(tw, xmlDto);
            }

            var projectDto = await Service.CreateProject(xml.ToString()).ConfigureAwait(false);
            return new Project(projectDto, Instance);
        }

        public async Task Delete()
        {
            await Service.DeleteProject($"id:{Id}").ConfigureAwait(false);
        }

        /// <summary>
        /// Create a new VCS root.
        /// </summary>
        /// <param name="id">Id of the vcs root.</param>
        /// <param name="name">Name of the vcs root.</param>
        /// <param name="type">Type of the vcs root (eg. Git).</param>
        /// <param name="properties">Vcs root properties.</param>
        /// <returns>Created VCS root.</returns>
        public async Task<IVcsRoot> CreateVcsRoot(
            Id id, string name, VcsRootType type, IDictionary<string, string> properties)
        {
            var xmlDto = new NewVcsRoot
            {
                Name = name,
                Id = id.StringId,
                VcsName = type.stringType,
                Project = new ReferenceDto
                {
                    Id = IdString
                },
                Properties = new PropertiesDto
                {
                    Property = properties?.Select(prop => new PropertyDto
                    {
                        Name = prop.Key,
                        Value = prop.Value
                    }).ToList() ?? new List<PropertyDto>()
                }
            };

            var xml = new StringBuilder();
            using (var tw = new StringWriter(xml))
            {
                var serializer = new XmlSerializer(typeof(NewVcsRoot));
                serializer.Serialize(tw, xmlDto);
            }

            var vcsRootDto = await Service.CreateVcsRoot(xml.ToString()).ConfigureAwait(false);
            return await VcsRoot.Create(vcsRootDto, true, Instance).ConfigureAwait(false);
        }

        public string GetHomeUrl(string branch = null)
            => Instance.GetUserUrlPage(
                "project.html",
                projectId: Id,
                branch: branch);

        public string GetTestHomeUrl(Id testId)
            => Instance.GetUserUrlPage(
                "project.html",
                projectId: Id,
                testNameId: testId,
                tab: "testDetails");

        public async Task SetParameter(string name, string value)
        {
            //         LOG.info("Setting parameter $name=$value in ProjectId=$idString")
            await Service.SetProjectParameter(Id.StringId, name, value).ConfigureAwait(false);
        }

        public override string ToString()
            => $"Project(id={IdString},name={Name})";
    }
}