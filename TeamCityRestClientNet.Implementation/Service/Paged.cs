using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamCityRestClientNet.RestApi;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.Api;

namespace TeamCityRestClientNet.Service
{
    class Paged2<TTeamCityEntity, TTeamCityDto, TTeamCityListDto> : IAsyncEnumerable<TTeamCityEntity>
        where TTeamCityEntity : IIdentifiable
        where TTeamCityDto : IdDto
        where TTeamCityListDto : ListDto<TTeamCityDto>
    {
        private readonly TeamCityServer _instance;
        /// <summary>
        /// Service for communicating with the backing TeamCity server.
        /// </summary>
        private readonly ITeamCityService _service;
        /// <summary>
        /// Method used for loading the first data item from the TeamCity server.
        /// </summary>
        private readonly Func<Task<TTeamCityListDto>> _getFirst;
        /// <summary>
        /// Method used for converting loaded data items into pages of team city entities.
        /// </summary>
        private readonly Func<TTeamCityDto, Task<TTeamCityEntity>> _convertToPage;

        public Paged2(
            TeamCityServer instance,
            Func<Task<TTeamCityListDto>> getFirst,
            Func<TTeamCityDto, Task<TTeamCityEntity>> convertToPage)
        {
            this._instance = instance;
            this._service = instance.Service;
            this._getFirst = getFirst;
            this._convertToPage = convertToPage;
        }

        public async IAsyncEnumerator<TTeamCityEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            string nextHref = null;
            do 
            {
                var dtoListTask = String.IsNullOrEmpty(nextHref)
                    ? this._getFirst()
                    : this._service.Root<TTeamCityListDto>(nextHref.RemovePrefix($"{this._instance.ServerUrlBase}/"));

                var dtoList = await dtoListTask.ConfigureAwait(false);
                if (dtoList.Items.Count == 0)
                    yield break;

                var entityTasks = dtoList.Items.Select(dto => this._convertToPage(dto));
                var entities = await Task.WhenAll(entityTasks).ConfigureAwait(false);
                foreach (var entity in entities) 
                {
                    yield return entity;
                }
                nextHref = dtoList.NextHref;

            } while (!String.IsNullOrEmpty(nextHref));
        }
    }

    /// <summary>
    /// Asynchnorously enumerate pages of TeamCity entities.
    /// </summary>
    /// <typeparam name="TTeamCityEntity">TeamCity domain entity type whose instances are contained in each page.</typeparam>
    /// <typeparam name="TTeamCityDto">TeamCity dto type used for communicating with the backing TeamCity server.</typeparam>
    class Paged<TTeamCityEntity, TTeamCityDto> : IAsyncEnumerable<TTeamCityEntity>
    {
        private readonly TeamCityServer _instance;
        /// <summary>
        /// Service for communicating with the backing TeamCity server.
        /// </summary>
        private readonly ITeamCityService _service;
        /// <summary>
        /// Method used for loading the first data item from the TeamCity server.
        /// </summary>
        private readonly Func<Task<TTeamCityDto>> _getFirst;
        /// <summary>
        /// Method used for converting loaded data items into pages of team city entities.
        /// </summary>
        private readonly Func<TTeamCityDto, Task<Page<TTeamCityEntity>>> _convertToPage;

        public Paged(
            TeamCityServer instance,
            Func<Task<TTeamCityDto>> getFirst,
            Func<TTeamCityDto, Task<Page<TTeamCityEntity>>> convertToPage)
        {
            this._instance = instance;
            this._service = instance.Service;
            this._getFirst = getFirst;
            this._convertToPage = convertToPage;
        }

        public async IAsyncEnumerator<TTeamCityEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            TTeamCityDto dto = await this._getFirst().ConfigureAwait(false);
            var page = await this._convertToPage(dto).ConfigureAwait(false);
            foreach (var item in page.ItemsNotNull)
            {
                yield return item;
            }

            while (!String.IsNullOrEmpty(page.NextHref))
            {
                var path = page.NextHref.RemovePrefix($"{this._instance.ServerUrlBase}/");
                dto = await this._service.Root<TTeamCityDto>(path).ConfigureAwait(false);
                page = await this._convertToPage(dto).ConfigureAwait(false);
                foreach (var item in page.ItemsNotNull)
                {
                    yield return item;
                }
            }
        }
    }

    class Page<T>
    {
        public Page(T[] items, string nextHref)
        {
            this.Items = items ?? new T[0];
            this.NextHref = nextHref;
        }
        public T[] Items { get; }
        public T[] ItemsNotNull => this.Items.Where(item => item != null).ToArray();
        public string NextHref { get; }
    }
}