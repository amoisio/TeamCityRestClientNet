using System;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.Extensions;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Domain
{
    class Parameter : IParameter {
        private readonly ParameterDto _dto;

        public Parameter(ParameterDto dto)
        {
            _dto = dto;
        }

        public string Name => _dto.Name.SelfOrNullRef();
        public string Value => _dto.Value.SelfOrNullRef();
        public bool Own => _dto.Own.ValueOrNullRef();
    }
}