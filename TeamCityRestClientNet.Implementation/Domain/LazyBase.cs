using System;
using System.Threading.Tasks;
using Nito.AsyncEx;

using TeamCityRestClientNet.Service;

namespace TeamCityRestClientNet.Domain
{
    abstract class LazyBase<TDto> where TDto : IdDto
    {
        protected LazyBase(TDto dto, bool isFullDto, TeamCityServer instance)
        {
            this.Dto = dto ?? throw new ArgumentNullException("dto must not be null.");

            if (String.IsNullOrWhiteSpace(dto.Id))
                throw new InvalidOperationException($"{nameof(dto)}.Id must not be null.");

            this.IsFullDto = isFullDto;
            this.Instance = instance ?? throw new ArgumentNullException("Instance must not be null.");

            this.FullDto = new AsyncLazy<TDto>(async () =>
            {
                if (!this.IsFullDto)
                {
                    this.Dto = await FetchFullDto().ConfigureAwait(false);
                    this.IsFullDto = true;
                }
                return this.Dto;
            });
        }

        /// <summary>
        /// The backing TeamCity instance.
        /// </summary>
        protected TeamCityServer Instance { get; }
        /// <summary>
        /// The service used for communicating with the backing TeamCity instance.
        /// </summary>
        protected ITeamCityService Service => Instance.Service;
        /// <summary>
        /// DTO used for querying the backing TeamCity instance and storing the
        /// received data.
        /// </summary>
        protected TDto Dto { get; private set; }
        /// <summary>
        /// Does Dto contains the full dto?
        /// </summary>
        protected bool IsFullDto { get; private set; }
        protected string IdString => this.Dto.Id;
        protected AsyncLazy<TDto> FullDto { get; }

        protected TDto FullDtoSync
            => this.FullDto.GetAwaiter().GetResult();

        protected T NotNullSync<T>(Func<TDto, T> getter)
            => NotNull(getter).GetAwaiter().GetResult();

        protected async Task<T> NotNull<T>(Func<TDto, T> getter)
            => getter(this.Dto)
            ?? getter(await this.FullDto.ConfigureAwait(false))
            ?? throw new NullReferenceException();

        protected T NullableSync<T>(Func<TDto, T> getter)
            => Nullable(getter).GetAwaiter().GetResult();

        protected async Task<T> Nullable<T>(Func<TDto, T> getter)
            => getter(this.Dto)
            ?? getter(await this.FullDto.ConfigureAwait(false));

        protected abstract Task<TDto> FetchFullDto();

        public abstract override string ToString();

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            var other = obj as LazyBase<TDto>;
            return IdString == other.IdString
                && this.Instance == other.Instance;
        }

        public override int GetHashCode() => this.IdString.GetHashCode();
    }
}