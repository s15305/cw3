using System;
using Microsoft.AspNetCore.DataProtection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Services.EncryptionService
{
    public class EncryptionService : IEncryptionService
    {
        private readonly string _secret = Environment.GetEnvironmentVariable("PASSWORD_SECRET");
        private readonly IDataProtectionProvider _provider;

        public EncryptionService(IDataProtectionProvider provider)
        {
            _provider = provider;
        }

        public string Encrypt(string stringToEncode)
        {
            return _provider.CreateProtector(_secret).Protect(stringToEncode);
        }

        public string Decrypt(string stringToDecode)
        {
            return _provider.CreateProtector(_secret).Unprotect(stringToDecode);
        }
    }
}
