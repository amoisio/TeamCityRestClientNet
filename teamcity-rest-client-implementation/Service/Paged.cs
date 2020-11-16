using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

using TeamCityRestClientNet.Extensions;

namespace TeamCityRestClientNet.Service
{
    class Paged<TType, TDto> : IAsyncEnumerable<TType>
    {
        private readonly ITeamCityService _service;
        private readonly Func<Task<TDto>> _getFirst;
        private readonly Func<TDto, Task<Page<TType>>> _convertToPage;

        public Paged(
            ITeamCityService service,
            Func<Task<TDto>> getFirst,
            Func<TDto, Task<Page<TType>>> convertToPage)
        {
            this._service = service;
            this._getFirst = getFirst;
            this._convertToPage = convertToPage;
        }

        public async IAsyncEnumerator<TType> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            TDto dto = await this._getFirst();
            var page = await this._convertToPage(dto);
            foreach (var item in page.ItemsNotNull)
            {
                yield return item;
            }

            while (!String.IsNullOrEmpty(page.NextHref))
            {
                var path = page.NextHref.RemovePrefix($"{this._service.ServerUrlBase}/");
                dto = await this._service.Root<TDto>(path);
                page = await this._convertToPage(dto);
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