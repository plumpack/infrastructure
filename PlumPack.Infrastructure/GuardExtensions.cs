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
    }
}