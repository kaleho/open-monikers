using System;

namespace Open.Monikers
{
    /// <summary>
    ///     An interface that represents an reference identifier,
    ///     when using a Guid or number meaning gets lost because
    ///     a Guid for AccountId could accidentally be used as a
    ///     GroupId, this is similar to using AggregateIds in DDD
    ///     but less formal of a definition
    /// </summary>
    public interface IRefId
    {
        public const string AggregateSeparator = "#";
        public const string NameSeparator = "~";
        public const string UriSeparator = "/";
        public static Type Type = typeof(IRefId);

        string Name { get; }

        RefId ParentId { get; }

        string ToUri();
    }
}