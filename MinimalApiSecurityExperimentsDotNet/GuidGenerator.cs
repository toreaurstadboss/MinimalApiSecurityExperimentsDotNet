using System.Security.Cryptography;

namespace MinimalApiSecurityExperimentsDotNet
{
    
    public static class GuidGenerator
    {

        /// <summary>
        /// Creates a RFC 4122-compliant UUIDv4 generator
        /// </summary>
        /// <returns></returns>
        public static Guid CreateCryptographicallySecureGuid()
        {
            var data = RandomNumberGenerator.GetBytes(16);

            // Set the version to 4 (UUIDv4)
            data[6] = (byte)((data[6] & 0x0F) | 0x40);

            // Set the variant to RFC 4122
            data[8] = (byte)((data[8] & 0x3F) | 0x80);


            return new Guid(data);

        }

    }

}
