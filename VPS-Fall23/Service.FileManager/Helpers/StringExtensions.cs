using System.Security.Cryptography;
using System.Text;

namespace VPS.Helper
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string value) { return string.IsNullOrWhiteSpace(value); }
        public static bool IsNullOrEmpty(this string value) { return string.IsNullOrEmpty(value); }
        public static string Format(this string value, params object?[] args) { return string.Format(value, args); }

        public static string GenerateRandomString(int length)
        {
            const string validCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            using RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

            byte[] randomBytes = new byte[length];
            rngCsp.GetBytes(randomBytes);

            StringBuilder sb = new StringBuilder(length);
            foreach (byte b in randomBytes)
            {
                sb.Append(validCharacters[b % validCharacters.Length]);
            }

            return sb.ToString();
        }
    }
}