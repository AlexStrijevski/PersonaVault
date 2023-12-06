using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PersonaVault.Contracts.Mappers
{
    internal class UserDataMapper : IUserDataMapper
    {
        public NewUserDTO MapToNewUserDTO(byte[] passwordHash, byte[] passwordSalt, RegisterUserRequest request)
        {
            return new NewUserDTO 
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
        }
    }
}
