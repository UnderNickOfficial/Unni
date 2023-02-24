namespace Unni.Infrastructure.Database.Extensions
{
    public static class StringExtension
    {
        public static string FirstCharToUpper(this string str)
        {
            str.ToLower();
            return string.Concat(str[0].ToString().ToUpper(), str.AsSpan(1));
        }
    }
}
