using System.Security.Cryptography;

namespace MinimalApiSecurityExperimentsDotNet.Extensions
{

    public static class HmacSha256KeyGenerator
    {

        public static string GenerateStrongKey()
        {
            // Generate a 256-bit (32-byte) key
            byte[] key = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }

            // Convert to Base64 for storage or display
            string base64Key = Convert.ToBase64String(key);
            return base64Key;
            //Console.WriteLine("Generated HMAC-SHA-256 Key (Base64):");
            //Console.WriteLine(base64Key);
        }

    }

}
