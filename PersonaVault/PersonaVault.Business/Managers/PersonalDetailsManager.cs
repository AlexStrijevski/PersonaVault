using PersonaVault.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonaVault.Database.Repositories;
using Microsoft.Extensions.Logging;
using PersonaVault.Business.Services;
using PersonaVault.Contracts.DTOs;
using PersonaVault.Database.Models;
using PersonaVault.Business.Requirements;
using Azure.Core;

namespace PersonaVault.Business.Managers
{
    internal class PersonalDetailsManager : IPersonalDetailsManager
    {
        private readonly IPersonalDetailsRepository _personalDetailsRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<PersonalDetailsManager> _logger;
        private readonly IEncryptionService _encryptionService;
        private readonly IPersonalDetailsRequirements _personalDetailsRequirements;
        private readonly IPersonalDetailsRequirementsValidator _personalDetailsRequirementsValidator;

        public PersonalDetailsManager(
            ILogger<PersonalDetailsManager> logger, 
            IPersonalDetailsRepository personalDetailsRepository, 
            IUserRepository userRepository,
            IEncryptionService encryptionService,
            IPersonalDetailsRequirements personalDetailsRequirements,
            IPersonalDetailsRequirementsValidator personalDetailsRequirementsValidator)
        {
            _personalDetailsRepository = personalDetailsRepository;
            _userRepository = userRepository;
            _logger = logger;
            _encryptionService = encryptionService;
            _personalDetailsRequirements = personalDetailsRequirements;
            _personalDetailsRequirementsValidator = personalDetailsRequirementsValidator;
        }

        public async Task<ActionResponse> CreatePersonalDetails(NewPersonalDetailsDTO request, Guid userId)
        {
            try
            {
                var user = await _userRepository.GetUserWithPersonalDetails(userId);

                var createPersonalDertailsRequirementsResponse = CreatePersonalDetailsRequirementsValidationResponse(request, user);

                if (!createPersonalDertailsRequirementsResponse.IsSuccess)
                    return createPersonalDertailsRequirementsResponse;

                await CreateAndAddEncryptedPersonalDetailsToUser(request, user);

                _logger.LogInformation($"Personal Details record created for {user.Username}");

                return new ActionResponse(true, 201, $"Personal Details added to {user.Username}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Interaction with database failed: " + ex.Message + "Stack trace: " + ex.StackTrace);

                return new ActionResponse(false, 500, "Internal error, please contact support");
            }
        }

        private EncryptedPersonalDetailsDTO GetEncryptedPersonalDetails(NewPersonalDetailsDTO request)
        {
            return _encryptionService.EncryptPersonalDetails(request);
        }

        private ActionResponse CreatePersonalDetailsRequirementsValidationResponse(NewPersonalDetailsDTO data, User user)
        {
            return _personalDetailsRequirementsValidator.ValidateRequirementsForPersonalDetailsCreation(data, user);
        }

        private async Task CreateAndAddEncryptedPersonalDetailsToUser(NewPersonalDetailsDTO request, User user)
        {
            var encryptedData = GetEncryptedPersonalDetails(request);
            await _personalDetailsRepository.CreateAndAddPersonalDetailsToUser(encryptedData, user);
        }

        public async Task<ActionResponse> UpdateName(string newName, Guid userId)
        {
            try
            {
                var user = await _userRepository.GetUserWithPersonalDetails(userId);

                var updateNameRequirementsResponse = UpdateNameRequirementsValidationResponse(user);

                if (!updateNameRequirementsResponse.IsSuccess)
                    return updateNameRequirementsResponse;

                await _personalDetailsRepository.UpdateName(newName, user);

                return new ActionResponse(true, 200, "Name successfully updated");
            }
            catch (Exception ex)
            {
                _logger.LogError("Interaction with database failed: " + ex.Message + "Stack trace: " + ex.StackTrace);

                return new ActionResponse(false, 500, "Internal error, please contact support");
            }
        }

        private ActionResponse UpdateNameRequirementsValidationResponse(User user)
        {
            return _personalDetailsRequirementsValidator.ValidateRequirementsForNameUpdate(user);
        }

        public async Task<ActionResponse> UpdateLastName(string newLastName, Guid userId)
        {
            try
            {
                var user = await _userRepository.GetUserWithPersonalDetails(userId);

                var updateLastNameRequirementsResponse = UpdateLastNameRequirementsValidationResponse(user);

                if (!updateLastNameRequirementsResponse.IsSuccess)
                    return updateLastNameRequirementsResponse;

                await _personalDetailsRepository.UpdateLastName(newLastName, user);

                return new ActionResponse(true, 200, "Last Name successfully updated");
            }
            catch (Exception ex)
            {
                _logger.LogError("Interaction with database failed: " + ex.Message + "Stack trace: " + ex.StackTrace);

                return new ActionResponse(false, 500, "Internal errror, please contact support");
            }
        }

        private ActionResponse UpdateLastNameRequirementsValidationResponse(User user)
        {
            return _personalDetailsRequirementsValidator.ValidateRequirementsForLastNameUpdate(user);
        }

        public async Task<ActionResponse> UpdatePersonalCode(string newPersonalCode, Guid userId)
        {
            try
            {
                var user = await _userRepository.GetUserWithPersonalDetails(userId);

                var updatePersonalCodeRequirementsResponse = UpdatePersonalCodeRequirementsValidationResponse(newPersonalCode, user);

                if (!updatePersonalCodeRequirementsResponse.IsSuccess)
                    return updatePersonalCodeRequirementsResponse;

                await EncryptAndUpdatePersonalCode(newPersonalCode, user);

                return new ActionResponse(true, 200, "Personal Code successfully updated");
            }
            catch (Exception ex)
            {
                _logger.LogError("Interaction with database failed: " + ex.Message + "Stack trace: " + ex.StackTrace);

                return new ActionResponse(false, 500, "Internal errror, please contact support");
            }
        }

        private ActionResponse UpdatePersonalCodeRequirementsValidationResponse(string newPersonalCode, User user)
        {
            return _personalDetailsRequirementsValidator.ValidateRequirementsForPersonalCodeUpdate(newPersonalCode, user);
        }

        private async Task EncryptAndUpdatePersonalCode(string newPersonalCode, User user)
        {
            var encryptedPersonalCode = EncryptString(newPersonalCode);

            await _personalDetailsRepository.UpdatePersonalCode(encryptedPersonalCode, user);
        }

        public async Task<ActionResponse> UpdatePhoneNumber(string newPhoneNumber, Guid userId)
        {
            try
            {
                var user = await _userRepository.GetUserWithPersonalDetails(userId);

                var updatePhoneNumberRequirementsResponse = UpdatePhoneNumberRequirementsValidationResponse(newPhoneNumber, user);

                if (!updatePhoneNumberRequirementsResponse.IsSuccess)
                    return updatePhoneNumberRequirementsResponse;

                await EncryptAndUpdatePhoneNumber(newPhoneNumber, user);

                return new ActionResponse(true, 200, "Phone Number successfully updated");
            }
            catch (Exception ex)
            {
                _logger.LogError("Interaction with database failed: " + ex.Message + "Stack trace: " + ex.StackTrace);

                return new ActionResponse(false, 500, "Internal errror, please contact support");
            }
        }

        private ActionResponse UpdatePhoneNumberRequirementsValidationResponse(string newPhoneNumber, User user)
        {
            return _personalDetailsRequirementsValidator.ValidateRequirementsForPhoneNumberUpdate(newPhoneNumber, user);
        }

        private async Task EncryptAndUpdatePhoneNumber(string newPhoneNumber, User user)
        {
            var encryptedPhoneNumber = EncryptString(newPhoneNumber);

            await _personalDetailsRepository.UpdatePhoneNumber(encryptedPhoneNumber, user);
        }

        public async Task<ActionResponse> UpdateEmailAddress(string newEmail, Guid userId)
        {
            try
            {
                var user = await _userRepository.GetUserWithPersonalDetails(userId);

                var updateEmailRequirementsResponse = UpdateEmailRequirementsValidationResponse(newEmail, user);

                if (!updateEmailRequirementsResponse.IsSuccess)
                    return updateEmailRequirementsResponse;

                await EncryptAndUpdateEmail(newEmail, user);

                return new ActionResponse(true, 200, "Email Address successfully updated");
            }
            catch (Exception ex)
            {
                _logger.LogError("Interaction with database failed: " + ex.Message + "Stack trace: " + ex.StackTrace);

                return new ActionResponse(false, 500, "Internal errror, please contact support");
            }
        }

        private ActionResponse UpdateEmailRequirementsValidationResponse(string newEmail, User user)
        {
            return _personalDetailsRequirementsValidator.ValidateRequirementsForEmailUpdate(newEmail, user);
        }

        private async Task EncryptAndUpdateEmail(string newEmail, User user)
        {
            var encryptedEmail = EncryptString(newEmail);

            await _personalDetailsRepository.UpdateEmail(encryptedEmail, user);
        }

        private byte[] EncryptString(string data)
        {
            return _encryptionService.EncryptStringToBytes(data);
        }
    }
}