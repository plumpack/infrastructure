using System.IO;

namespace PlumPack.Infrastructure
{
    public static class InfraHelpers
    {
        public static string ResolvePathRelativeToDirectory(string path, string directory)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }
            
            if (Path.IsPathRooted(path))
            {
                return path;
            }

            path = Path.Combine(directory, path);

            return Path.GetFullPath(path);
        }
    }
}