using System.Text;

namespace MinimalApiSecurityExperimentsDotNet
{
   
    public class Pseudonymizer
    {
        private readonly byte[] _key;

        public Pseudonymizer(string key)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                _key = Encoding.UTF8.GetBytes(key);
            }
        }

        /// <summary>
        /// Outputs a pseudonymized version of the input string using HMAC SHA256.       
        /// </summary>
        /// <param name="message"></param>
        /// <param name="outputToHexString">Output to hex string if true. If false, base-64 string is returned</param>
        /// <returns></returns>
        public string? Pseudonymize(string? message, bool outputToHexString = true)
        {
            if (string.IsNullOrWhiteSpace(message) || _key == null)
            {
                return message;
            }
            using var hmac = new System.Security.Cryptography.HMACSHA256(_key);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            return outputToHexString ? Convert.ToHexString(hash) : Convert.ToBase64String(hash);
        }

        public bool Verify(string input, string pseudonym)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(pseudonym))
            {
                return false;
            }
            var computedPseudonym = Pseudonymize(input);
            return computedPseudonym == pseudonym;
        }

    }

}
