using Microsoft.Extensions.Logging;
using Moq;
using PersonaVault.Api.Validators;
using PersonaVault.Api.Controllers;
using PersonaVault.Business.Managers;
using PersonaVault.Business.Services;
using PersonaVault.Contracts.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PersonaVault.Contracts.Requests;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Responses;

namespace PersonaVault.Api.Tests
{
    public class PersonalDetailsControllerTests
    {
        [Fact]
        public async void CreatePersonalDetails_ReturnsFailedToRetrieveUserId()
        {
            // Arrange
            var personalDetailsManager = new Mock<IPersonalDetailsManager>();
            var personalDetailsMapper = new Mock<IPersonalDetailsMapper>();
            var imageHandler = new Mock<IImageHandler>();
            var logger = new Mock<ILogger<PersonalDetailsController>>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new PersonalDetailsController(personalDetailsManager.Object, personalDetailsMapper.Object, imageHandler.Object, logger.Object, requestValidator.Object);

            // Invalid Guid in User Claims
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, "invalidId") };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            //Act
            var actualResponse = await controller.CreatePersonalDetails(It.IsAny<CreatePersonalDetailsRequest>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(500, statusCodeObject.StatusCode);
            Assert.Equal("Failed to retrieve User Id", statusCodeObject.Value);
        }
        [Fact]
        public async void CreatePersonalDetails_ReturnsSomeFieldsAreNotValid()
        {
            // Arrange
            var personalDetailsManager = new Mock<IPersonalDetailsManager>();
            var personalDetailsMapper = new Mock<IPersonalDetailsMapper>();
            var imageHandler = new Mock<IImageHandler>();
            var logger = new Mock<ILogger<PersonalDetailsController>>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new PersonalDetailsController(personalDetailsManager.Object, personalDetailsMapper.Object, imageHandler.Object, logger.Object, requestValidator.Object);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            requestValidator.Setup(service => service.IsRequestDataValid(It.IsAny<CreatePersonalDetailsRequest>())).Returns(false);

            //Act
            var actualResponse = await controller.CreatePersonalDetails(It.IsAny<CreatePersonalDetailsRequest>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(400, statusCodeObject.StatusCode);
            Assert.Equal("Some fields are not valid, please check input data", statusCodeObject.Value);
        }

        [Fact]
        public async void CreatePersonalDetails_ReturnsCreated()
        {
            // Arrange
            var personalDetailsManager = new Mock<IPersonalDetailsManager>();
            var personalDetailsMapper = new Mock<IPersonalDetailsMapper>();
            var imageHandler = new Mock<IImageHandler>();
            var logger = new Mock<ILogger<PersonalDetailsController>>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new PersonalDetailsController(personalDetailsManager.Object, personalDetailsMapper.Object, imageHandler.Object, logger.Object, requestValidator.Object);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            var expectedResponse = new ActionResponse(true, 201, "Personal Details created");

            requestValidator.Setup(service => service.IsRequestDataValid(It.IsAny<CreatePersonalDetailsRequest>())).Returns(true);

            imageHandler.Setup(service => service.DoesImageSizeMeetRequirements(It.IsAny<IFormFile>())).Returns(true);

            imageHandler.Setup(service => service.ConvertImageToByteArray(It.IsAny<IFormFile>())).Returns(It.IsAny<byte[]>());

            personalDetailsMapper.Setup(service => service.MapToPersonalDetailsDTO(It.IsAny<CreatePersonalDetailsRequest>(), It.IsAny<byte[]>())).Returns(It.IsAny<NewPersonalDetailsDTO>());

            personalDetailsManager.Setup(service => service.CreatePersonalDetails(It.IsAny<NewPersonalDetailsDTO>(), It.IsAny<Guid>())).ReturnsAsync(expectedResponse);

            //Act
            var actualResponse = await controller.CreatePersonalDetails(new CreatePersonalDetailsRequest());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(expectedResponse.StatusCode, statusCodeObject.StatusCode);
            Assert.Equal(expectedResponse.Message, statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateName_ReturnsFailedToRetrieveUserId()
        {
            // Arrange
            var personalDetailsManager = new Mock<IPersonalDetailsManager>();
            var personalDetailsMapper = new Mock<IPersonalDetailsMapper>();
            var imageHandler = new Mock<IImageHandler>();
            var logger = new Mock<ILogger<PersonalDetailsController>>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new PersonalDetailsController(personalDetailsManager.Object, personalDetailsMapper.Object, imageHandler.Object, logger.Object, requestValidator.Object);

            // Invalid Guid in User Claims
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, "invalidId") };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            //Act
            var actualResponse = await controller.UpdateName(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(500, statusCodeObject.StatusCode);
            Assert.Equal("Failed to retrieve User Id", statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateName_ReturnsFieldCannotBeEmpty()
        {
            // Arrange
            var personalDetailsManager = new Mock<IPersonalDetailsManager>();
            var personalDetailsMapper = new Mock<IPersonalDetailsMapper>();
            var imageHandler = new Mock<IImageHandler>();
            var logger = new Mock<ILogger<PersonalDetailsController>>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new PersonalDetailsController(personalDetailsManager.Object, personalDetailsMapper.Object, imageHandler.Object, logger.Object, requestValidator.Object);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            requestValidator.Setup(service => service.IsStringValid(It.IsAny<string>())).Returns(false);

            //Act
            var actualResponse = await controller.UpdateName(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(400, statusCodeObject.StatusCode);
            Assert.Equal("Name cannot be empty", statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateName_ReturnsCreated()
        {
            // Arrange
            var personalDetailsManager = new Mock<IPersonalDetailsManager>();
            var personalDetailsMapper = new Mock<IPersonalDetailsMapper>();
            var imageHandler = new Mock<IImageHandler>();
            var logger = new Mock<ILogger<PersonalDetailsController>>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new PersonalDetailsController(personalDetailsManager.Object, personalDetailsMapper.Object, imageHandler.Object, logger.Object, requestValidator.Object);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            var expectedResponse = new ActionResponse(true, 200, "Name successfully updated");

            requestValidator.Setup(service => service.IsStringValid(It.IsAny<string>())).Returns(true);

            personalDetailsManager.Setup(service => service.UpdateName(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(expectedResponse);

            //Act
            var actualResponse = await controller.UpdateName(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(expectedResponse.StatusCode, statusCodeObject.StatusCode);
            Assert.Equal(expectedResponse.Message, statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateLastName_ReturnsFailedToRetrieveUserId()
        {
            // Arrange
            var personalDetailsManager = new Mock<IPersonalDetailsManager>();
            var personalDetailsMapper = new Mock<IPersonalDetailsMapper>();
            var imageHandler = new Mock<IImageHandler>();
            var logger = new Mock<ILogger<PersonalDetailsController>>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new PersonalDetailsController(personalDetailsManager.Object, personalDetailsMapper.Object, imageHandler.Object, logger.Object, requestValidator.Object);

            // Invalid Guid in User Claims
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, "invalidId") };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            //Act
            var actualResponse = await controller.UpdateLastName(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(500, statusCodeObject.StatusCode);
            Assert.Equal("Failed to retrieve User Id", statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateLastName_ReturnsFieldCannotBeEmpty()
        {
            // Arrange
            var personalDetailsManager = new Mock<IPersonalDetailsManager>();
            var personalDetailsMapper = new Mock<IPersonalDetailsMapper>();
            var imageHandler = new Mock<IImageHandler>();
            var logger = new Mock<ILogger<PersonalDetailsController>>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new PersonalDetailsController(personalDetailsManager.Object, personalDetailsMapper.Object, imageHandler.Object, logger.Object, requestValidator.Object);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            requestValidator.Setup(service => service.IsStringValid(It.IsAny<string>())).Returns(false);

            //Act
            var actualResponse = await controller.UpdateLastName(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(400, statusCodeObject.StatusCode);
            Assert.Equal("Last Name cannot be empty", statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateLastName_ReturnsCreated()
        {
            // Arrange
            var personalDetailsManager = new Mock<IPersonalDetailsManager>();
            var personalDetailsMapper = new Mock<IPersonalDetailsMapper>();
            var imageHandler = new Mock<IImageHandler>();
            var logger = new Mock<ILogger<PersonalDetailsController>>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new PersonalDetailsController(personalDetailsManager.Object, personalDetailsMapper.Object, imageHandler.Object, logger.Object, requestValidator.Object);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            var expectedResponse = new ActionResponse(true, 200, "Last Name successfully updated");

            requestValidator.Setup(service => service.IsStringValid(It.IsAny<string>())).Returns(true);

            personalDetailsManager.Setup(service => service.UpdateLastName(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(expectedResponse);

            //Act
            var actualResponse = await controller.UpdateLastName(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(expectedResponse.StatusCode, statusCodeObject.StatusCode);
            Assert.Equal(expectedResponse.Message, statusCodeObject.Value);
        }

        [Fact]
        public async void UpdatePersonalCode_ReturnsFailedToRetrieveUserId()
        {
            // Arrange
            var personalDetailsManager = new Mock<IPersonalDetailsManager>();
            var personalDetailsMapper = new Mock<IPersonalDetailsMapper>();
            var imageHandler = new Mock<IImageHandler>();
            var logger = new Mock<ILogger<PersonalDetailsController>>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new PersonalDetailsController(personalDetailsManager.Object, personalDetailsMapper.Object, imageHandler.Object, logger.Object, requestValidator.Object);

            // Invalid Guid in User Claims
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, "invalidId") };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            //Act
            var actualResponse = await controller.UpdatePersonalCode(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(500, statusCodeObject.StatusCode);
            Assert.Equal("Failed to retrieve User Id", statusCodeObject.Value);
        }

        [Fact]
        public async void UpdatePersonalCode_ReturnsFieldCannotBeEmpty()
        {
            // Arrange
            var personalDetailsManager = new Mock<IPersonalDetailsManager>();
            var personalDetailsMapper = new Mock<IPersonalDetailsMapper>();
            var imageHandler = new Mock<IImageHandler>();
            var logger = new Mock<ILogger<PersonalDetailsController>>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new PersonalDetailsController(personalDetailsManager.Object, personalDetailsMapper.Object, imageHandler.Object, logger.Object, requestValidator.Object);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            requestValidator.Setup(service => service.IsStringValid(It.IsAny<string>())).Returns(false);

            //Act
            var actualResponse = await controller.UpdatePersonalCode(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(400, statusCodeObject.StatusCode);
            Assert.Equal("Personal Code should be valid", statusCodeObject.Value);
        }

        [Fact]
        public async void UpdatePersonalCode_ReturnsCreated()
        {
            // Arrange
            var personalDetailsManager = new Mock<IPersonalDetailsManager>();
            var personalDetailsMapper = new Mock<IPersonalDetailsMapper>();
            var imageHandler = new Mock<IImageHandler>();
            var logger = new Mock<ILogger<PersonalDetailsController>>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new PersonalDetailsController(personalDetailsManager.Object, personalDetailsMapper.Object, imageHandler.Object, logger.Object, requestValidator.Object);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            var expectedResponse = new ActionResponse(true, 200, "Personal Code successfully updated");

            requestValidator.Setup(service => service.IsLongValid(It.IsAny<string>())).Returns(true);

            personalDetailsManager.Setup(service => service.UpdatePersonalCode(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(expectedResponse);

            //Act
            var actualResponse = await controller.UpdatePersonalCode(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(expectedResponse.StatusCode, statusCodeObject.StatusCode);
            Assert.Equal(expectedResponse.Message, statusCodeObject.Value);
        }

        [Fact]
        public async void UpdatePhoneNumber_ReturnsFailedToRetrieveUserId()
        {
            // Arrange
            var personalDetailsManager = new Mock<IPersonalDetailsManager>();
            var personalDetailsMapper = new Mock<IPersonalDetailsMapper>();
            var imageHandler = new Mock<IImageHandler>();
            var logger = new Mock<ILogger<PersonalDetailsController>>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new PersonalDetailsController(personalDetailsManager.Object, personalDetailsMapper.Object, imageHandler.Object, logger.Object, requestValidator.Object);

            // Invalid Guid in User Claims
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, "invalidId") };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            //Act
            var actualResponse = await controller.UpdatePhoneNumber(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(500, statusCodeObject.StatusCode);
            Assert.Equal("Failed to retrieve User Id", statusCodeObject.Value);
        }

        [Fact]
        public async void UpdatePhoneNumber_ReturnsFieldCannotBeEmpty()
        {
            // Arrange
            var personalDetailsManager = new Mock<IPersonalDetailsManager>();
            var personalDetailsMapper = new Mock<IPersonalDetailsMapper>();
            var imageHandler = new Mock<IImageHandler>();
            var logger = new Mock<ILogger<PersonalDetailsController>>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new PersonalDetailsController(personalDetailsManager.Object, personalDetailsMapper.Object, imageHandler.Object, logger.Object, requestValidator.Object);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            requestValidator.Setup(service => service.IsStringValid(It.IsAny<string>())).Returns(false);

            //Act
            var actualResponse = await controller.UpdatePhoneNumber(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(400, statusCodeObject.StatusCode);
            Assert.Equal("Phone Number cannot be empty", statusCodeObject.Value);
        }

        [Fact]
        public async void UpdatePhoneNumber_ReturnsCreated()
        {
            // Arrange
            var personalDetailsManager = new Mock<IPersonalDetailsManager>();
            var personalDetailsMapper = new Mock<IPersonalDetailsMapper>();
            var imageHandler = new Mock<IImageHandler>();
            var logger = new Mock<ILogger<PersonalDetailsController>>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new PersonalDetailsController(personalDetailsManager.Object, personalDetailsMapper.Object, imageHandler.Object, logger.Object, requestValidator.Object);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            var expectedResponse = new ActionResponse(true, 200, "Phone Number successfully updated");

            requestValidator.Setup(service => service.IsLongValid(It.IsAny<string>())).Returns(true);

            personalDetailsManager.Setup(service => service.UpdatePhoneNumber(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(expectedResponse);

            //Act
            var actualResponse = await controller.UpdatePhoneNumber(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(expectedResponse.StatusCode, statusCodeObject.StatusCode);
            Assert.Equal(expectedResponse.Message, statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateEmail_ReturnsFailedToRetrieveUserId()
        {
            // Arrange
            var personalDetailsManager = new Mock<IPersonalDetailsManager>();
            var personalDetailsMapper = new Mock<IPersonalDetailsMapper>();
            var imageHandler = new Mock<IImageHandler>();
            var logger = new Mock<ILogger<PersonalDetailsController>>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new PersonalDetailsController(personalDetailsManager.Object, personalDetailsMapper.Object, imageHandler.Object, logger.Object, requestValidator.Object);

            // Invalid Guid in User Claims
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, "invalidId") };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            //Act
            var actualResponse = await controller.UpdateEmail(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(500, statusCodeObject.StatusCode);
            Assert.Equal("Failed to retrieve User Id", statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateEmail_ReturnsFieldCannotBeEmpty()
        {
            // Arrange
            var personalDetailsManager = new Mock<IPersonalDetailsManager>();
            var personalDetailsMapper = new Mock<IPersonalDetailsMapper>();
            var imageHandler = new Mock<IImageHandler>();
            var logger = new Mock<ILogger<PersonalDetailsController>>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new PersonalDetailsController(personalDetailsManager.Object, personalDetailsMapper.Object, imageHandler.Object, logger.Object, requestValidator.Object);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            requestValidator.Setup(service => service.IsStringValid(It.IsAny<string>())).Returns(false);

            //Act
            var actualResponse = await controller.UpdateEmail(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(400, statusCodeObject.StatusCode);
            Assert.Equal("Email Address cannot be empty", statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateEmail_ReturnsCreated()
        {
            // Arrange
            var personalDetailsManager = new Mock<IPersonalDetailsManager>();
            var personalDetailsMapper = new Mock<IPersonalDetailsMapper>();
            var imageHandler = new Mock<IImageHandler>();
            var logger = new Mock<ILogger<PersonalDetailsController>>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new PersonalDetailsController(personalDetailsManager.Object, personalDetailsMapper.Object, imageHandler.Object, logger.Object, requestValidator.Object);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            var expectedResponse = new ActionResponse(true, 200, "Email Address successfully updated");

            requestValidator.Setup(service => service.IsStringValid(It.IsAny<string>())).Returns(true);

            personalDetailsManager.Setup(service => service.UpdateEmailAddress(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(expectedResponse);

            //Act
            var actualResponse = await controller.UpdateEmail(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(expectedResponse.StatusCode, statusCodeObject.StatusCode);
            Assert.Equal(expectedResponse.Message, statusCodeObject.Value);
        }
    }
}
