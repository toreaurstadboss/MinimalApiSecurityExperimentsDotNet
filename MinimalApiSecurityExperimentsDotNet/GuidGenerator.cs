using System.Security.Cryptography;

namespace MinimalApiSecurityExperimentsDotNet
{
    
    public static class GuidGenerator
    {

        public static Guid CreateCryptographicallySecureGuid()
        {
            var data = RandomNumberGenerator.GetBytes(16);
            return new Guid(data);

        }

    }

}
