using System;

namespace Open.Monikers
{
    public static class RefIdStringExtensions
    {
        public static string AsId(
            this Type type,
            string seed = null)
        {
            var name =
                type.Name.EndsWith("Id")
                    ? type.Name[..^2]
                    : type.Name;

            var suffix =
                !string.IsNullOrWhiteSpace(seed)
                    ? seed.Replace(IRefId.NameSeparator, "")
                    : Guid.NewGuid().ToString("N");

            return $"{name}{IRefId.AggregateSeparator}{suffix}".ToLowerInvariant();
        }

        /// <summary>
        ///     Parses a <see cref="string" /> into a <see cref="TRefId" /> based
        ///     on the first location of the <see cref="IRefId" />.NameSeparator
        /// </summary>
        /// <typeparam name="TRefId"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TRefId Parse<TRefId>(
            this string value)
        {
            var nameType = typeof(TRefId);

            var (name, id) = value.SplitToTuple(IRefId.NameSeparator);

            var instance =
                (TRefId)Activator.CreateInstance(
                    nameType,
                    id,
                    new RefId(name));

            return instance;
        }
    }
}