using Microsoft.Extensions.Configuration;
using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Requests;
using PersonaVault.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Business.Services
{
    internal class EncryptionService : IEncryptionService
    {
        private readonly IConfiguration _configuration;
        private byte[] _encryptionKey;
        private byte[] _encryptionIv;

        public EncryptionService(IConfiguration configuration)
        {
            _configuration = configuration;
            _encryptionKey = Convert.FromBase64String(_configuration["Encryption:Key"]);
            _encryptionIv = Convert.FromBase64String(_configuration["Encryption:IV"]);
        }

        public byte[] EncryptStringToBytes(string plainText)
        {
            byte[] encrypted;
            
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _encryptionKey;
                aesAlg.IV = _encryptionIv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }

        private string DecryptBytesToString(byte[] cipherText)
        {
            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _encryptionKey;
                aesAlg.IV = _encryptionIv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

        public EncryptedPersonalDetailsDTO EncryptPersonalDetails(NewPersonalDetailsDTO data)
        {
            var encrypted = new EncryptedPersonalDetailsDTO
            {
                Name = data.Name,
                LastName = data.LastName,
                PersonalCode = EncryptStringToBytes(data.PersonalCode.ToString()),
                PhoneNumber = EncryptStringToBytes(data.PhoneNumber.ToString()),
                EmailAddress = EncryptStringToBytes(data.EmailAddress),
                Picture = data.Picture
            };
            return encrypted;
        }

        public EncryptedAddressDetailsDTO EncryptAddressDetails(CreateAddressDetailsRequest data)
        {
            var encrypted = new EncryptedAddressDetailsDTO
            {
                Country = EncryptStringToBytes(data.Country),
                City = EncryptStringToBytes(data.City),
                Street = EncryptStringToBytes(data.Street),
                HouseNumber = EncryptStringToBytes(data.HouseNumber),
                ApartamentNumber = EncryptStringToBytes(data.ApartamentNumber)
            };
            return encrypted;
        }

        public FullUserData DecrpytFullUserData(User user)
        {
            return new FullUserData
            {
                Username = user.Username,
                Role = user.Role.ToString(),
                Name = user.PersonalDetails.Name,
                LastName = user.PersonalDetails.LastName,
                PersonalCode = DecryptBytesToString(user.PersonalDetails.PersonalCode),
                PhoneNumber = DecryptBytesToString(user.PersonalDetails.PhoneNumber),
                EmailAddress = DecryptBytesToString(user.PersonalDetails.EmailAddress),
                Country = DecryptBytesToString(user.PersonalDetails.AddressDetails.Country),
                City = DecryptBytesToString(user.PersonalDetails.AddressDetails.City),
                Street = DecryptBytesToString(user.PersonalDetails.AddressDetails.Street),
                HouseNumber = DecryptBytesToString(user.PersonalDetails.AddressDetails.HouseNumber),
                ApartamentNumber = DecryptBytesToString(user.PersonalDetails.AddressDetails.ApartamentNumber),
            };
        }
        public FullUserData DecrpytUserAndPersonalDetails(User user)
        {
            return new FullUserData
            {
                Username = user.Username,
                Role = user.Role.ToString(),
                Name = user.PersonalDetails.Name,
                LastName = user.PersonalDetails.LastName,
                PersonalCode = DecryptBytesToString(user.PersonalDetails.PersonalCode),
                PhoneNumber = DecryptBytesToString(user.PersonalDetails.PhoneNumber),
                EmailAddress = DecryptBytesToString(user.PersonalDetails.EmailAddress)
            };
        }
        public FullUserData DecrpytUserData(User user)
        {
            return new FullUserData
            {
                Username = user.Username,
                Role = user.Role.ToString()
            };
        }
    }
}
