namespace IDeliverable.Bits.Helpers
{
    public static class StringExtensions
    {
        public static string PascalToCamelCase(this string value)
        {
            if (value == null)
                return null;

            if (value.Length < 2)
                return value.ToLowerInvariant();

            return value.Substring(0, 1).ToLowerInvariant() + value.Substring(1);
        }
    }
}