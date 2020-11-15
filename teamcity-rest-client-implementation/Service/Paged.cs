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
    internal class Paged<T> : IAsyncEnumerable<T>
    {
        private readonly ITeamCityService _service;
        private readonly Func<Task<IEnumerable<T>>> _getFirst;
        private readonly Func<IEnumerable<T>, Task<Page<T>>> _convertToPage;

        public Paged(
            ITeamCityService service,
            Func<Task<IEnumerable<T>>> getFirst,
            Func<IEnumerable<T>, Task<Page<T>>> convertToPage)
        {
            this._service = service;
            this._getFirst = getFirst;
            this._convertToPage = convertToPage;
        }

        public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            IEnumerable<T> items = await this._getFirst();
            var page = await this._convertToPage(items);
            foreach (var item in page.ItemsNotNull)
            {
                yield return item;
            }

            while (!String.IsNullOrEmpty(page.NextHref))
            {
                var path = page.NextHref.RemovePrefix($"{this._service.ServerUrlBase}/");
                items = await this._service.Root<T>(path);
                page = await this._convertToPage(items);
                foreach (var item in page.ItemsNotNull)
                {
                    yield return item;
                }
            }
        }
    }

    internal class Page<T>
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