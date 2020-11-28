using System;
using Nito.AsyncEx;

namespace TeamCityRestClientNet.Api
{
    public interface IParameter
    {
        string Name { get; }
        string Value { get; }
        bool Own { get; }
    }
}