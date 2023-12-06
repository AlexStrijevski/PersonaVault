using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonaVault.Api.Validators;
using PersonaVault.Business.Managers;
using PersonaVault.Business.Services;
using PersonaVault.Contracts.Mappers;
using PersonaVault.Contracts.Requests;
using PersonaVault.Contracts.Responses;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace PersonaVault.Api.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class PersonalDetailsController : ControllerBase
    {
        private readonly IPersonalDetailsManager _personalDetailsManager;
        private readonly IPersonalDetailsMapper _personalDetailsMapper;
        private readonly IImageHandler _imageHandler;
        private readonly ILogger<PersonalDetailsController> _logger;
        private readonly IRequestDataValidator _requestValidator;

        public PersonalDetailsController(
            IPersonalDetailsManager personalDetailsManager, 
            IPersonalDetailsMapper personalDetailsMapper, 
            IImageHandler imageConverter, 
            ILogger<PersonalDetailsController> logger,
            IRequestDataValidator requestValidator)
        {
            _personalDetailsManager = personalDetailsManager;
            _personalDetailsMapper = personalDetailsMapper;
            _imageHandler = imageConverter;
            _logger = logger;
            _requestValidator = requestValidator;
        }

        [Authorize]
        [HttpPost("CreatePersonalDetails")]
        public async Task<IActionResult> CreatePersonalDetails([FromForm] CreatePersonalDetailsRequest request)
        {
            if(!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value, out var userId))
            {
                _logger.LogError($"Failed to retreive User Id from JWT Token");
                return StatusCode(500, "Failed to retrieve User Id");
            }

            if (!_requestValidator.IsRequestDataValid(request))
                return StatusCode(400, "Some fields are not valid, please check input data");

            byte[] picture;

            if(_imageHandler.DoesImageSizeMeetRequirements(request.Picture))
            {
                picture = _imageHandler.ConvertImageToByteArray(request.Picture);
            }
            else
            {
                picture = _imageHandler.ResizeImageAndConvertToByteArray(request.Picture);
            }

            var personalDetailsDto = _personalDetailsMapper.MapToPersonalDetailsDTO(request, picture);

            var response = await _personalDetailsManager.CreatePersonalDetails(personalDetailsDto, userId);

            return StatusCode(response.StatusCode, response.Message);
        }

        [Authorize]
        [HttpPut("UpdateName")]
        public async Task<IActionResult> UpdateName([FromBody][Required] string newName)
        {
            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value, out var userId))
            {
                _logger.LogError($"Failed to retreive User Id from JWT Token");
                return StatusCode(500, "Failed to retrieve User Id");
            }

            if (!_requestValidator.IsStringValid(newName))
                return StatusCode(400, "Name cannot be empty");

            var response = await _personalDetailsManager.UpdateName(newName, userId);

            return StatusCode(response.StatusCode, response.Message);
        }

        [Authorize]
        [HttpPut("UpdateLastName")]
        public async Task<IActionResult> UpdateLastName([FromBody][Required] string newLastName)
        {
            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value, out var userId))
            {
                _logger.LogError($"Failed to retreive User Id from JWT Token");
                return StatusCode(500, "Failed to retrieve User Id");
            }

            if (!_requestValidator.IsStringValid(newLastName))
                return StatusCode(400, "Last Name cannot be empty");

            var response = await _personalDetailsManager.UpdateLastName(newLastName, userId);

            return StatusCode(response.StatusCode, response.Message);
        }

        [Authorize]
        [HttpPut("UpdatePersonalCode")]
        public async Task<IActionResult> UpdatePersonalCode([FromBody][Required] string newPersonalCode)
        {
            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value, out var userId))
            {
                _logger.LogError($"Failed to retreive User Id from JWT Token");
                return StatusCode(500, "Failed to retrieve User Id");
            }

            if (!_requestValidator.IsLongValid(newPersonalCode))
                return StatusCode(400, "Personal Code should be valid");

            var response = await _personalDetailsManager.UpdatePersonalCode(newPersonalCode, userId);

            return StatusCode(response.StatusCode, response.Message);
        }

        [Authorize]
        [HttpPut("UpdatePhoneNumber")]
        public async Task<IActionResult> UpdatePhoneNumber([FromBody][Required] string newPhoneNumber)
        {
            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value, out var userId))
            {
                _logger.LogError($"Failed to retreive User Id from JWT Token");
                return StatusCode(500, "Failed to retrieve User Id");
            }

            if (!_requestValidator.IsLongValid(newPhoneNumber))
                return StatusCode(400, "Phone Number cannot be empty");

            var response = await _personalDetailsManager.UpdatePhoneNumber(newPhoneNumber, userId);

            return StatusCode(response.StatusCode, response.Message);
        }

        [Authorize]
        [HttpPut("UpdateEmail")]
        public async Task<IActionResult> UpdateEmail([FromBody][Required] string newEmail)
        {
            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value, out var userId))
            {
                _logger.LogError($"Failed to retreive User Id from JWT Token");
                return StatusCode(500, "Failed to retrieve User Id");
            }

            if (!_requestValidator.IsStringValid(newEmail))
                return StatusCode(400, "Email Address cannot be empty");

            var response = await _personalDetailsManager.UpdateEmailAddress(newEmail, userId);

            return StatusCode(response.StatusCode, response.Message);
        }
    }
}
