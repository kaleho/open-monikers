using System;

namespace Open.Monikers
{
    public static class StringParsingExtensions
    {
        public static (string, string) SplitToTuple(
            this string value,
            string separator)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            var parts =
                new[]
                {
                    value.Substring(0, value.IndexOf(separator, StringComparison.OrdinalIgnoreCase)),
                    value.Substring(value.IndexOf(separator, StringComparison.OrdinalIgnoreCase) + 1)
                };

            if (parts.Length != 2)
            {
                throw new Exception(
                    $"The provided string [{value}] could not " +
                    $"be split into two parts by the separator [{separator}].");
            }

            return (parts[0], parts[1]);
        }

        public static bool TrySplitToTuple(
            this string value,
            string separator,
            out (string, string) values)
        {
            bool returnValue;

            try
            {
                values = value.SplitToTuple(separator);

                returnValue = true;
            }
            catch
            {
                values = (null, null);

                returnValue = false;
            }

            return returnValue;
        }
    }
}