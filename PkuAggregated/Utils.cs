using System.Security.Cryptography;
using System.Text;

namespace PkuAggregated
{
    public static class Utils
    {
        public static string GenerateVerificationToken()
        {
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            string combinedString = currentDate + Params.TokenGeneratorSource;
            using (SHA256 sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedString));
                var sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString().Substring(0, 16);
            }
        }
    }
}
