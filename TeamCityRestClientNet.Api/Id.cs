using System;

namespace TeamCityRestClientNet.Api
{
    public interface IIdentifiable
    {
        Id Id { get; }
    }

    readonly public struct Id : IEquatable<Id>
    {
        public Id(string stringId)
        {
            this.StringId = stringId;
        }

        public string StringId { get; }

        public override string ToString() => this.StringId;

        public static bool operator ==(Id id1, Id id2) => id1.Equals(id2);

        public static bool operator !=(Id id1, Id id2) => !id1.Equals(id2);

        public bool Equals(Id other) => String.Equals(StringId, other.StringId);

        public override bool Equals(object obj)
            => obj is Id ? this.Equals((Id)obj) : false;

        public override int GetHashCode() => StringId?.GetHashCode() ?? 0;
    }
}