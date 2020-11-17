using TeamCityRestClientNet.Api;

namespace TeamCityRestClientNet.Implementations 
{
    struct Branch : IBranch
    {
        internal Branch(string name, bool isDefault)
        {
            this.Name = name;
            this.IsDefault = isDefault;
        }

        public string Name { get; }

        public bool IsDefault { get; }
    }
}
