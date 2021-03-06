using System;
using TeamCityRestClientNet.Api;
using TeamCityRestClientNet.RestApi;

namespace TeamCityRestClientNet.Domain
{
    abstract class Base<TDto> where TDto : IdDto
    {
        protected Base(TDto fullDto, TeamCityServer instance)
        {
            this.Dto = fullDto ?? throw new ArgumentNullException($"{nameof(fullDto)} must not be null.");

            if (String.IsNullOrWhiteSpace(fullDto.Id))
                throw new InvalidOperationException($"{nameof(fullDto)}.Id must not be null.");

            this.Instance = instance ?? throw new ArgumentNullException("Instance must not be null.");
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
        protected string IdString => this.Dto.Id;
        public Id Id => new Id(IdString);
        public virtual string Name => this.Dto.Name;
        public virtual string Href => this.Dto.Href;
        public virtual string WebUrl => this.Dto.WebUrl;

        protected T NotNull<T>(Func<TDto, T> getter)
            => getter(this.Dto)
            ?? throw new NullReferenceException();

        protected T Nullable<T>(Func<TDto, T> getter)
            => getter(this.Dto);

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