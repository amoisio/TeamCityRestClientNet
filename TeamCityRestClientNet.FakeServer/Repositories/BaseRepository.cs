using System;
using System.Collections.Generic;
using System.Linq;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.FakeServer
{
    abstract class BaseRepository<TDto, TListDto> 
        where TDto : IdDto 
        where TListDto : ListDto<TDto>, new()
    {
        protected readonly Dictionary<string, TDto> _itemsById = new Dictionary<string, TDto>();
        public void Add(TDto item) => _itemsById.Add(item.Id, item);
        public TListDto All() => new TListDto() { Items = _itemsById.Values.ToList() };
        public TDto ById(string id) 
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            if (_itemsById.ContainsKey(id))
                return _itemsById[id];
            else 
                return null;
        }

        public virtual TDto Delete(string id)
        {
            var root = ById(id);
            if (root != null)
            {
                _itemsById.Remove(id);
                return root;
            }
            else
                throw new ArgumentException($"Item with id {id} not found.");
        }
    }
}