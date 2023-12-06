using Microsoft.Extensions.Logging;
using PersonaVault.Business.Requirements;
using PersonaVault.Business.Services;
using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Mappers;
using PersonaVault.Contracts.Requests;
using PersonaVault.Contracts.Responses;
using PersonaVault.Database.Models;
using PersonaVault.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Business.Managers
{
    internal class UserManager : IUserManager
    {
        private readonly IUserDataRequirements _userDataRequirements;
        private readonly IUserRepository _userRepository;
        private readonly IHashService _hashService;
        private readonly ILogger<UserManager> _logger;
        private readonly IUserDataMapper _newUserMapper;
        private readonly IEncryptionService _encryptionService;

        public UserManager(
            IUserRepository userRepository, 
            IUserDataRequirements newUserRequirements, 
            IHashService hashService, 
            ILogger<UserManager> logger,
            IUserDataMapper registerUserRequestMapper,
            IEncryptionService encryptionService)
        {
            _userDataRequirements = newUserRequirements;
            _hashService = hashService;
            _logger = logger;
            _userRepository = userRepository;
            _newUserMapper = registerUserRequestMapper;
            _encryptionService = encryptionService;
        }
        public async Task<ActionResponse> RegisterUser(RegisterUserRequest request)
        {
            try
            {
                if (!await _userDataRequirements.CheckIfUserDetailsMeetRequirements(request))
                {
                    return new ActionResponse(false, 400, "Validation failed, some fields are not meeting the requirements, please check input data");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Interaction with Database failed: " + ex.Message);
                return new ActionResponse(false, 500, "Internal error, please contact support");
            }
            return await CreatUserInDatabase(request);
        }
        public async Task<ActionResponse> CreatUserInDatabase(RegisterUserRequest request)
        {
            _hashService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var newUserDto = _newUserMapper.MapToNewUserDTO(passwordHash, passwordSalt, request);

            try
            {
                await _userRepository.CreateUser(newUserDto);
                _logger.LogInformation($"User {request.Username} was successfully created");
            }
            catch (Exception ex)
            {
                _logger.LogError("Interaction with Database failed: " + ex.Message + "Stack trace: " + ex.StackTrace);
                return new ActionResponse(false, 500, "Internal error, please cotnact support");
            }
            return new ActionResponse(true, 201, "New user successfully created");
        }

        private async Task<User> GetUser(string username)
        {
            return await _userRepository.GetUser(username);
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            try
            {
                var user = await GetUser(request.Username);

                if (!DoesUserExist(user)) 
                    return new LoginResponse(false, 404, "User not found");

                if(!VerifyUserPassword(user, request.Password)) 
                    return new LoginResponse(false, 401, "Invalid credentials");

                return new LoginResponse(true, user.Id, user.Role);
            }
            catch(Exception ex )
            {
                _logger.LogError("Interaction with Database failed: " + ex.Message + "Stack trace: " + ex.StackTrace);
                return new LoginResponse(false, 500, "Something went wrong, please contac support");
            }
        }

        private bool DoesUserExist(User user)
        {
            return user != null;
        }

        private bool VerifyUserPassword(User user, string password)
        {
            return _hashService.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
        }

        public async Task<FullUserData> GetFullUserData(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetFullUserData(userId);

                if (DoesUserHavePersonalDetails(user))
                {
                    if (DoesUserHaveAddressDetails(user))
                    {
                        return GetFullUserDataAndDecryptToString(user);
                    }
                    else
                    {
                        return GetUserAndPersonalDetailsAndDecryptToString(user);
                    }
                }
                else
                {
                    return GetUserDataAndDecryptToString(user);
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get and decrpyt full user data: " + ex.Message + "Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        private FullUserData GetFullUserDataAndDecryptToString(User user)
        {
            return _encryptionService.DecrpytFullUserData(user);
        }
        private FullUserData GetUserAndPersonalDetailsAndDecryptToString(User user)
        {
            return _encryptionService.DecrpytUserAndPersonalDetails(user);
        }
        private FullUserData GetUserDataAndDecryptToString(User user)
        {
            return _encryptionService.DecrpytUserData(user);
        }

        private bool DoesUserHavePersonalDetails(User user)
        {
            if(user.PersonalDetails == null)
                return false;

            return true;
        }

        private bool DoesUserHaveAddressDetails(User user)
        {
            if (user.PersonalDetails.AddressDetails == null)
                return false;

            return true;
        }

        public async Task<ActionResponse> DeleteUser(string username)
        {
            try
            {
                var user = await _userRepository.GetFullUserData(username);

                if (!DoesUserExist(user))
                    return new ActionResponse(false, 404, "User not found");

                await _userRepository.DeleteUser(user);

                return new ActionResponse(true, 200, $"User {user.Username} deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError("Interaction with Database failed: " + ex.Message + "Stack trace: " + ex.StackTrace);
                return new ActionResponse(false, 500, "Something went wrong, please contac support");
            }
        }
    }
}
