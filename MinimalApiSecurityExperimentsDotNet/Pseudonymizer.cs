using System.Text;

namespace MinimalApiSecurityExperimentsDotNet
{
   
    public class Pseudonymizer
    {
        private readonly byte[] _key;

        public Pseudonymizer(string key)
        {
            _key = Encoding.UTF8.GetBytes(key);
        }

        /// <summary>
        /// Outputs a pseudonymized version of the input string using HMAC SHA256.       
        /// </summary>
        /// <param name="input"></param>
        /// <param name="outputToHexString">Output to hex string if true. If false, base-64 string is returned</param>
        /// <returns></returns>
        public string Pseudonymize(string input, bool outputToHexString = true)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            using var hmac = new System.Security.Cryptography.HMACSHA256(_key);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
            return outputToHexString ? Convert.ToHexString(hash) : Convert.ToBase64String(hash);
        }

    }

}
