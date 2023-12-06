using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Requests;
using PersonaVault.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PersonaVault.Business.Requirements
{
    internal class UserDataRequirements : IUserDataRequirements
    {
        private readonly IUserRepository _userRepository;
        public UserDataRequirements(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> CheckIfUserDetailsMeetRequirements(RegisterUserRequest request)
        {
            if (!await DoesUsernameMeetRequirements(request.Username) ||
                !DoesPasswordMeetRequirements(request.Password))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DoesUsernameMeetRequirements(string username)
        {
            if (await _userRepository.UserExists(username)) return false;
            if (username.Length < 6) return false;
            return true;
        }

        public bool DoesPasswordMeetRequirements(string password)
        {
            if (password.Length < 8) return false;
            if (!Regex.IsMatch(password, @"\d") ||
                !Regex.IsMatch(password, @"[^\w\s]") ||
                !Regex.IsMatch(password, @"[a-z]") ||
                !Regex.IsMatch(password, @"[A-Z]")
                )
            {
                return false;
            }
            return true;
        }
    }
}
