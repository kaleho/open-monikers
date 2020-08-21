using System;

namespace Open.Monikers
{
    public interface IRefId
    {
        public const string AggregateSeparator = "#";
        public const string NameSeparator = "~";
        public const string UriSeparator = "/";
        public static Type Type = typeof(IRefId);

        string Name { get; }

        RefId ParentId { get; }
    }
}