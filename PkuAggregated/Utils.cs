using System.Security.Cryptography;
using System.Text;

namespace PkuAggregated
{
    public static class Utils
    {
        public static bool VerifyToken(string token, DateTime requestTime)
        {
            if (Math.Abs((DateTime.Now - requestTime).TotalSeconds) > 20)
                return false;
            string currentDate = requestTime.ToString("yyyyMMddHHmmss");
            string combinedString = currentDate + Params.TokenGeneratorSource;
            var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(combinedString));
            var sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("X2"));
            }
            return token.Equals(sb.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
