using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Services.EncryptionService
{
    public interface IEncryptionService
    {
        public string Encrypt(string stringToEncode);
        public string Decrypt(string stringToDecode);
    }
}
