using System;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Domain
{
    [Obsolete("Delete if not needed once client library is done.")]
    class NameValueProperty {
        private readonly NameValuePropertyDto _dto;

        public NameValueProperty(NameValuePropertyDto dto)
        {
            this._dto = dto;
        }

        public string Name 
            => this._dto.Name ?? throw new NullReferenceException();

        public string Value => this._dto.Value;
    }
}