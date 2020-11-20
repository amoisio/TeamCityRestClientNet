using System;
using System.Threading.Tasks;

using TeamCityRestClientNet.Service;
using TeamCityRestClientNet.Extensions;
namespace TeamCityRestClientNet.Domain
{
    abstract class Base<TDto> where TDto : IdDto
    {
        protected Base(TDto dto, bool isFullDto, TeamCityInstance instance)
        {
            this.Dto = dto  ?? throw new ArgumentNullException("dto must not be null.");

            if (String.IsNullOrWhiteSpace(dto.Id))
                throw new InvalidOperationException($"{nameof(dto)}.Id must not be null.");

            this.IsFullDto = isFullDto;
            this.Instance = instance ?? throw new ArgumentNullException("Instance must not be null.");
        }

        protected TeamCityInstance Instance { get; }
        protected ITeamCityService Service => Instance.Service;
        protected TDto Dto { get; set; }
        protected bool IsFullDto { get; set; }
        protected string IdString => this.Dto.Id;

        protected async Task<TDto> FullDto() 
            => await GetFullDto().ConfigureAwait(false);

        protected TDto FullDtoSync 
            => GetFullDto().GetAwaiter().GetResult();

        private async Task<TDto> GetFullDto()
        {
            if (!this.IsFullDto)
            {
                this.Dto = await FetchFullDto().ConfigureAwait(false);
                this.IsFullDto = true;
            }
            return this.Dto;
        }

        protected T NotNullSync<T>(Func<TDto, T> getter)
            => NotNull(getter).GetAwaiter().GetResult();

        protected async Task<T> NotNull<T>(Func<TDto, T> getter)
            => getter(this.Dto) 
            ?? getter(await this.GetFullDto().ConfigureAwait(false)) 
            ?? throw new NullReferenceException();

        protected T NullableSync<T>(Func<TDto, T> getter)
            => Nullable(getter).GetAwaiter().GetResult();

        protected async Task<T> Nullable<T>(Func<TDto, T> getter)
            => getter(this.Dto)
            ?? getter(await this.GetFullDto().ConfigureAwait(false));

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