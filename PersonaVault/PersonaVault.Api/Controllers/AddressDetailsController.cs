using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonaVault.Api.Validators;
using PersonaVault.Business.Managers;
using PersonaVault.Contracts.Requests;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace PersonaVault.Api.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class AddressDetailsController : ControllerBase
    {
        private readonly ILogger<AddressDetailsController> _logger;
        private readonly IAddressDetailsManager _addressDetailsManager;
        private readonly IRequestDataValidator _requestValidator;

        public AddressDetailsController(
            ILogger<AddressDetailsController> logger, 
            IAddressDetailsManager addressDetailsManager,
            IRequestDataValidator requestValidator)
        {
            _logger = logger;
            _addressDetailsManager = addressDetailsManager;
            _requestValidator = requestValidator;
        }

        [Authorize]
        [HttpPost("CreateAddressDetails")]
        public async Task<IActionResult> CreateAddressDetails([FromBody] CreateAddressDetailsRequest request)
        {
            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value, out var userId))
            {
                _logger.LogError($"Failed to retreive User Id from JWT Token");
                return StatusCode(500, "Failed to retrieve User Id");
            }

            if (!_requestValidator.IsRequestDataValid(request))
                return StatusCode(400, "Some fields are not valid, please check input data");

            var response = await _addressDetailsManager.CreateAddressDetails(request, userId);

            return StatusCode(response.StatusCode, response.Message);
        }

        [Authorize]
        [HttpPut("UpdateCountry")]
        public async Task<IActionResult> UpdateCountry([FromBody][Required] string newCountry)
        {
            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value, out var userId))
            {
                _logger.LogError($"Failed to retreive User Id from JWT Token");
                return StatusCode(500, "Failed to retrieve User Id");
            }

            if (!_requestValidator.IsStringValid(newCountry))
                return StatusCode(400, "Country cannot be empty");

            var response = await _addressDetailsManager.UpdateCountry(newCountry, userId);

            return StatusCode(response.StatusCode, response.Message);
        }

        [Authorize]
        [HttpPut("UpdateCity")]
        public async Task<IActionResult> UpdateCity([FromBody][Required] string newCity)
        {
            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value, out var userId))
            {
                _logger.LogError($"Failed to retreive User Id from JWT Token");
                return StatusCode(500, "Failed to retrieve User Id");
            }

            if (!_requestValidator.IsStringValid(newCity))
                return StatusCode(400, "City cannot be empty");

            var response = await _addressDetailsManager.UpdateCity(newCity, userId);

            return StatusCode(response.StatusCode, response.Message);
        }

        [Authorize]
        [HttpPut("UpdateStreet")]
        public async Task<IActionResult> UpdateStreet([FromBody][Required] string newStreet)
        {
            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value, out var userId))
            {
                _logger.LogError($"Failed to retreive User Id from JWT Token");
                return StatusCode(500, "Failed to retrieve User Id");
            }

            if (!_requestValidator.IsStringValid(newStreet))
                return StatusCode(400, "Street cannot be empty");

            var response = await _addressDetailsManager.UpdateStreet(newStreet, userId);

            return StatusCode(response.StatusCode, response.Message);
        }

        [Authorize]
        [HttpPut("UpdateHouseNumber")]
        public async Task<IActionResult> UpdateHouseNumber([FromBody][Required] string newHouseNumber)
        {
            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value, out var userId))
            {
                _logger.LogError($"Failed to retreive User Id from JWT Token");
                return StatusCode(500, "Failed to retrieve User Id");
            }

            if (!_requestValidator.IsStringValid(newHouseNumber))
                return StatusCode(400, "HouseNumber cannot be empty");

            var response = await _addressDetailsManager.UpdateHouseNumber(newHouseNumber, userId);

            return StatusCode(response.StatusCode, response.Message);
        }

        [Authorize]
        [HttpPut("UpdateApartamentNumber")]
        public async Task<IActionResult> UpdateApartamentNumber([FromBody][Required] string newApartamentNumber)
        {
            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value, out var userId))
            {
                _logger.LogError($"Failed to retreive User Id from JWT Token");
                return StatusCode(500, "Failed to retrieve User Id");
            }

            if (!_requestValidator.IsStringValid(newApartamentNumber))
                return StatusCode(400, "ApartamentNumber cannot be empty");

            var response = await _addressDetailsManager.UpdateApartamentNumber(newApartamentNumber, userId);

            return StatusCode(response.StatusCode, response.Message);
        }
    }
}