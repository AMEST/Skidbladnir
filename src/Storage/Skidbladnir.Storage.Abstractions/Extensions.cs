namespace Skidbladnir.Storage.Abstractions
{
    public static class Extensions
    {
        public static string EscapePath(this string path)
        {
            return path.Replace("..\\", "").Replace("../","");
        }

        public static string StripPath(this string path)
        {
            if (path.StartsWith("\\") || path.StartsWith("/"))
                return path.Substring(1);
            return path;
        }
    }
}