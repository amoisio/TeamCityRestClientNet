using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamCityRestClientNet.FakeServer
{
    abstract class BaseRepository<T> where T : class
    {
        protected readonly Dictionary<string, T> _itemsById;
        protected readonly Func<T, string> _idResolver;
        public BaseRepository(Func<T, string> idResolver, params T[] items)
        {
            _idResolver = idResolver ?? throw new ArgumentNullException(nameof(idResolver));

            _itemsById = new Dictionary<string, T>();
            if (items != null)
            {
                foreach (var item in items)
                {
                    _itemsById.Add(_idResolver(item), item);
                }
            }
        }

        public T ById(string id) 
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            if (_itemsById.ContainsKey(id))
                return _itemsById[id];
            else 
                return null;
        }

        public List<T> AllItems() => _itemsById.Values.ToList();
    }
}