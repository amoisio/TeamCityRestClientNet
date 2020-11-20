using System;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Domain
{
    class Parameter : IParameter {
        private readonly ParameterDto _dto;

        public Parameter(ParameterDto dto)
        {
            this._dto = dto;
        }

        public string Name 
            => String.IsNullOrEmpty(this._dto.Name)
                ? throw new NullReferenceException()
                : this._dto.Name;

        public string Value
            => String.IsNullOrEmpty(this._dto.Value)
                ? throw new NullReferenceException()
                : this._dto.Value;

        public bool Own => this._dto.Own ?? throw new NullReferenceException();
    }
}