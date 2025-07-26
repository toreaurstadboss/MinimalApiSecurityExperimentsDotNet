using System.Security.Cryptography;
using System.Text;

namespace MinimalApiSecurityExperimentsDotNet
{
    
    public static class OtpGenerator
    {

        /// <summary>
        /// Generates an OTP = One Time Password. Just a random number of given <paramref name="length"/>
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateOtp(int length)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                builder.Append(RandomNumberGenerator.GetInt32(0, 10));
            }
            return builder.ToString();
        }

    }

}
