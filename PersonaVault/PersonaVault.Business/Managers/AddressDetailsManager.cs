using Azure.Core;
using Microsoft.Extensions.Logging;
using PersonaVault.Business.Services;
using PersonaVault.Contracts.DTOs;
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
    internal class AddressDetailsManager : IAddressDetailsManager
    {
        private readonly ILogger<AddressDetailsManager> _logger;
        private readonly IAddressDetailsRepository _addressDetailsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IAddressDetailsRequirementsValidator _addressDetailsRequirementsValidator;

        public AddressDetailsManager(
            ILogger<AddressDetailsManager> logger, 
            IAddressDetailsRepository addressDetailsRepository,
            IUserRepository userRepository,
            IEncryptionService encryptionService,
            IAddressDetailsRequirementsValidator addressDetailsRequirementsValidator)
        {
            _logger = logger;
            _addressDetailsRepository = addressDetailsRepository;
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _addressDetailsRequirementsValidator = addressDetailsRequirementsValidator;
        }

        public async Task<ActionResponse> CreateAddressDetails(CreateAddressDetailsRequest request, Guid userId)
        {
            try
            {
                var user = await _userRepository.GetFullUserData(userId);

                var requirementsResponse = CreateAddressDetailsRequirementsValidationResponse(user);

                if (!requirementsResponse.IsSuccess) 
                    return requirementsResponse;

                await CreateAndAddEncryptedAddressDetailsToUser(request, user);

                _logger.LogInformation($"Address Details created for {user.Username}");

                return new ActionResponse(true, 201, $"Address Details added to {user.Username}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Interaction with database failed: " + ex.Message + "Stack trace: " + ex.StackTrace);

                return new ActionResponse(false, 500, "Internal errror, please contact support");
            }
        }

        private ActionResponse CreateAddressDetailsRequirementsValidationResponse(User user)
        {
            return _addressDetailsRequirementsValidator.ValidateRequirementsForAddressDetailsCreation(user);
        }

        private EncryptedAddressDetailsDTO GetEncryptedAddressDetails(CreateAddressDetailsRequest request)
        {
            return _encryptionService.EncryptAddressDetails(request);
        }

        private async Task CreateAndAddEncryptedAddressDetailsToUser(CreateAddressDetailsRequest data, User user)
        {
            var encryptedData = GetEncryptedAddressDetails(data);

            await _addressDetailsRepository.CreateAddressDetailsAndAddToUser(encryptedData, user);
        }

        private byte[] EncryptString(string data)
        {
            return _encryptionService.EncryptStringToBytes(data);
        }

        private ActionResponse UpdateFieldRequirementsValidationResponse(User user)
        {
            return _addressDetailsRequirementsValidator.ValidateRequirementsForFieldUpdate(user);
        }

        public async Task<ActionResponse> UpdateCountry(string newCountry, Guid userId)
        {
            try
            {
                var user = await _userRepository.GetFullUserData(userId);

                var udpateCountryRequirementsValidationResponse = UpdateFieldRequirementsValidationResponse(user);

                if (!udpateCountryRequirementsValidationResponse.IsSuccess)
                    return udpateCountryRequirementsValidationResponse;

                await EncryptAndUpdateCountry(newCountry, user);

                return new ActionResponse(true, 200, "Country successfully updated");
            }
            catch (Exception ex)
            {
                _logger.LogError("Interaction with database failed: " + ex.Message + "Stack trace: " + ex.StackTrace);

                return new ActionResponse(false, 500, "Internal errror, please contact support");
            }
        }

        private async Task EncryptAndUpdateCountry(string newCountry, User user)
        {
            var encryptedCountry = EncryptString(newCountry);

            await _addressDetailsRepository.UpdateCountry(encryptedCountry, user);
        }

        public async Task<ActionResponse> UpdateCity(string newCity, Guid userId)
        {
            try
            {
                var user = await _userRepository.GetFullUserData(userId);

                var udpateCityRequirementsValidationResponse = UpdateFieldRequirementsValidationResponse(user);

                if (!udpateCityRequirementsValidationResponse.IsSuccess)
                    return udpateCityRequirementsValidationResponse;

                await EncryptAndUpdateCity(newCity, user);

                return new ActionResponse(true, 200, "City successfully updated");
            }
            catch (Exception ex)
            {
                _logger.LogError("Interaction with database failed: " + ex.Message + "Stack trace: " + ex.StackTrace);

                return new ActionResponse(false, 500, "Internal errror, please contact support");
            }
        }

        private async Task EncryptAndUpdateCity(string newCity, User user)
        {
            var encryptedCity = EncryptString(newCity);

            await _addressDetailsRepository.UpdateCity(encryptedCity, user);
        }

        public async Task<ActionResponse> UpdateStreet(string newStreet, Guid userId)
        {
            try
            {
                var user = await _userRepository.GetFullUserData(userId);

                var udpateStreetRequirementsValidationResponse = UpdateFieldRequirementsValidationResponse(user);

                if (!udpateStreetRequirementsValidationResponse.IsSuccess)
                    return udpateStreetRequirementsValidationResponse;

                await EncryptAndUpdateStreet(newStreet, user);

                return new ActionResponse(true, 200, "Street successfully updated");
            }
            catch (Exception ex)
            {
                _logger.LogError("Interaction with database failed: " + ex.Message + "Stack trace: " + ex.StackTrace);

                return new ActionResponse(false, 500, "Internal errror, please contact support");
            }
        }

        private async Task EncryptAndUpdateStreet(string newStreet, User user)
        {
            var encryptedStreet = EncryptString(newStreet);

            await _addressDetailsRepository.UpdateStreet(encryptedStreet, user);
        }

        public async Task<ActionResponse> UpdateHouseNumber(string newHouseNumber, Guid userId)
        {
            try
            {
                var user = await _userRepository.GetFullUserData(userId);

                var udpateHouseNumberRequirementsValidationResponse = UpdateFieldRequirementsValidationResponse(user);

                if (!udpateHouseNumberRequirementsValidationResponse.IsSuccess)
                    return udpateHouseNumberRequirementsValidationResponse;

                await EncryptAndUpdateHouseNumber(newHouseNumber, user);

                return new ActionResponse(true, 200, "House Number successfully updated");
            }
            catch (Exception ex)
            {
                _logger.LogError("Interaction with database failed: " + ex.Message + "Stack trace: " + ex.StackTrace);

                return new ActionResponse(false, 500, "Internal errror, please contact support");
            }
        }

        private async Task EncryptAndUpdateHouseNumber(string newHouseNumber, User user)
        {
            var encryptedHouseNumber = EncryptString(newHouseNumber);

            await _addressDetailsRepository.UpdateHouseNumber(encryptedHouseNumber, user);
        }

        public async Task<ActionResponse> UpdateApartamentNumber(string newApartamentNumber, Guid userId)
        {
            try
            {
                var user = await _userRepository.GetFullUserData(userId);

                var udpateApartamentNumberRequirementsValidationResponse = UpdateFieldRequirementsValidationResponse(user);

                if (!udpateApartamentNumberRequirementsValidationResponse.IsSuccess)
                    return udpateApartamentNumberRequirementsValidationResponse;

                await EncryptAndUpdateApartamentNumber(newApartamentNumber, user);

                return new ActionResponse(true, 200, "Apartament Number successfully updated");
            }
            catch (Exception ex)
            {
                _logger.LogError("Interaction with database failed: " + ex.Message + "Stack trace: " + ex.StackTrace);

                return new ActionResponse(false, 500, "Internal errror, please contact support");
            }
        }

        private async Task EncryptAndUpdateApartamentNumber(string newApartamentNumber, User user)
        {
            var encryptedApartamentNumber = EncryptString(newApartamentNumber);

            await _addressDetailsRepository.UpdateApartamentNumber(encryptedApartamentNumber, user);
        }
    }
}
