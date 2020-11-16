using System;
using System.Threading.Tasks;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Implementations
{
    abstract class Base<TDto> where TDto : IdDto
    {
        private TDto _dto;
        private bool _isFullDto;
        internal Base(TDto dto, bool isFullBean, TeamCityInstance instance)
        {
            this._dto = dto ?? throw new ArgumentNullException("dto must not be null.");

            if (String.IsNullOrEmpty(dto.Id))
            {
                throw new InvalidOperationException($"{nameof(dto)}.Id should not be null");
            }
            this._isFullDto = isFullBean;
            this.Instance = instance;
        }

        internal TeamCityInstance Instance { get; }
        protected string IdString => this._dto.Id;

        internal async Task<TDto> FullDto()
        {
            if (!this._isFullDto)
            {
                this._dto = await FetchFullDto();
                this._isFullDto = true;
            }
            return this._dto;
        }

        protected async Task<T> NotNull<T>(Func<TDto, T> getter)
            => getter(this._dto) 
            ?? getter(await this.FullDto()) 
            ?? throw new NullReferenceException();

        protected async Task<T> Nullable<T>(Func<TDto, T> getter)
            => getter(this._dto)
            ?? getter(await this.FullDto());

        internal abstract Task<TDto> FetchFullDto();

        public abstract override string ToString();

        public override bool Equals(object obj)
        {
            if (this == obj) return true;

            var other = obj as Base<TDto>;
            return IdString == other.IdString
                && this.Instance == other.Instance;
        }

        public override int GetHashCode() => this.IdString.GetHashCode();
    }
}