using System.Text.RegularExpressions;

namespace Budgets.Common
{
    public static class Validator
    {

        private static Regex EmailRegex = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
        private static Regex ColorRegex = new Regex("^#[\\w\\d]{6}$"); // hex format
        private static Regex IconRegex = new Regex("^[\\w\\d_][\\w\\d_\\\\/]*\\.(svg|ico)$");

        private static string[] avFileExts = { "txt", "doc", "jpg", "png" };
        public static Regex FileRegex = new Regex($"^\\w+\\.({string.Join('|',avFileExts)})$");

        
        public static bool ValidateEmail(string email)
        {
            return EmailRegex.IsMatch(email);
        }

        public static bool ValidateColor(string color)
        {
            return ColorRegex.IsMatch(color);
        }

        public static bool ValidateIcon(string iconPath)
        {
            return IconRegex.IsMatch(iconPath);
        }
        public static bool ValidateFile(string filePath)
        {
            return FileRegex.IsMatch(filePath);
        }
    }
}
