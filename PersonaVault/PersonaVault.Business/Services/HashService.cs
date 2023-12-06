using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Business.Services
{
    internal class HashService : IHashService
    {
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA256();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            var hmac = new HMACSHA256(passwordSalt);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return hash.SequenceEqual(passwordHash);
        }
    }
}
