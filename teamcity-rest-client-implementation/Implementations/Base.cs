using System;
using System.Threading.Tasks;
using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Implementations
{
    abstract class Base<TDto> where TDto : IdDto
    {
        internal Base(TDto dto, bool isFullDto, TeamCityInstance instance)
        {
            this.Dto = dto ?? throw new ArgumentNullException("dto must not be null.");

            if (String.IsNullOrEmpty(dto.Id))
            {
                throw new InvalidOperationException($"{nameof(dto)}.Id should not be null");
            }
            this.IsFullDto = isFullDto;
            this.Instance = instance;
        }

        protected TeamCityInstance Instance { get; }
        protected ITeamCityService Service => Instance.Service;
        protected TDto Dto { get; set; }
        protected bool IsFullDto { get; set; }
        protected string IdString => this.Dto.Id;

        protected TDto FullDto 
            => GetFullDto().GetAwaiter().GetResult();

        protected async Task<TDto> FullDtoAsync()
            => await GetFullDto();

        private async Task<TDto> GetFullDto()
        {
            if (!this.IsFullDto)
            {
                this.Dto = await FetchFullDto();
                this.IsFullDto = true;
            }
            return this.Dto;
        }

        protected T NotNull<T>(Func<TDto, T> getter)
            => NotNullAsync(getter).GetAwaiter().GetResult();

        protected async Task<T> NotNullAsync<T>(Func<TDto, T> getter)
            => getter(this.Dto) 
            ?? getter(await this.GetFullDto()) 
            ?? throw new NullReferenceException();

        protected T Nullable<T>(Func<TDto, T> getter)
            => NullableAsync(getter).GetAwaiter().GetResult();

        protected async Task<T> NullableAsync<T>(Func<TDto, T> getter)
            => getter(this.Dto)
            ?? getter(await this.GetFullDto());

        protected abstract Task<TDto> FetchFullDto();

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