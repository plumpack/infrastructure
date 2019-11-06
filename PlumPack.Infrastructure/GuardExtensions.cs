using System;

namespace PlumPack.Infrastructure
{
    public static class GuardExtensions
    {
        public static void NotNullOrEmpty(this string value, string name = null)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(name ?? "string");
            }
        }

        public static void NotNull(this object value, string name = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name ?? "object");
            }
        }
    }
}