using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PersonaVault.Api.Controllers;
using PersonaVault.Api.Validators;
using PersonaVault.Business.Managers;
using PersonaVault.Business.Security;
using PersonaVault.Contracts.Requests;
using PersonaVault.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PersonaVault.Api.Tests
{
    public class AddressDetailsControllerTests
    {
        [Fact]
        public async void CreateAddressDetails_ReturnsFailedRetreiveUserId()
        {
            // Arrange
            var logger = new Mock<ILogger<AddressDetailsController>>();
            var addressDetailsManager = new Mock<IAddressDetailsManager>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new AddressDetailsController(logger.Object, addressDetailsManager.Object, requestValidator.Object);

            // Invalid Guid in User Claims
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, "invalidId") };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            // Act
            var actualResponse = await controller.CreateAddressDetails(It.IsAny<CreateAddressDetailsRequest>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(500, statusCodeObject.StatusCode);
            Assert.Equal("Failed to retrieve User Id", statusCodeObject.Value);
        }

        [Fact]
        public async void CreateAddressDetails_ReturnsSomeFieldAreNotValid()
        {
            // Arrange
            var logger = new Mock<ILogger<AddressDetailsController>>();
            var addressDetailsManager = new Mock<IAddressDetailsManager>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new AddressDetailsController(logger.Object, addressDetailsManager.Object, requestValidator.Object);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            requestValidator.Setup(service => service.IsRequestDataValid(It.IsAny<CreateAddressDetailsRequest>())).Returns(false);

            // Act
            var actualResponse = await controller.CreateAddressDetails(It.IsAny<CreateAddressDetailsRequest>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(400, statusCodeObject.StatusCode);
            Assert.Equal("Some fields are not valid, please check input data", statusCodeObject.Value);
        }

        [Fact]
        public async void CreateAddressDetails_ReturnsCreated()
        {
            // Arrange
            var logger = new Mock<ILogger<AddressDetailsController>>();
            var addressDetailsManager = new Mock<IAddressDetailsManager>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var expectedResponse = new ActionResponse(true, 201, "Created");

            var controller = new AddressDetailsController(logger.Object, addressDetailsManager.Object, requestValidator.Object);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            requestValidator.Setup(service => service.IsRequestDataValid(It.IsAny<CreateAddressDetailsRequest>())).Returns(true);

            addressDetailsManager.Setup(service => service.CreateAddressDetails(It.IsAny<CreateAddressDetailsRequest>(), It.IsAny<Guid>())).ReturnsAsync(expectedResponse);

            // Act
            var actualResponse = await controller.CreateAddressDetails(It.IsAny<CreateAddressDetailsRequest>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(expectedResponse.StatusCode, statusCodeObject.StatusCode);
            Assert.Equal(expectedResponse.Message, statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateCountry_ReturnsFailedToRetrieveUserId()
        {
            // Arrange
            var logger = new Mock<ILogger<AddressDetailsController>>();
            var addressDetailsManager = new Mock<IAddressDetailsManager>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new AddressDetailsController(logger.Object, addressDetailsManager.Object, requestValidator.Object);

            // Invalid Guid in User Claims
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, "invalidId") };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            // Act
            var actualResponse = await controller.UpdateCountry(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(500, statusCodeObject.StatusCode);
            Assert.Equal("Failed to retrieve User Id", statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateCountry_ReturnsFieldCannotBeEmpty()
        {
            // Arrange
            var logger = new Mock<ILogger<AddressDetailsController>>();
            var addressDetailsManager = new Mock<IAddressDetailsManager>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new AddressDetailsController(logger.Object, addressDetailsManager.Object, requestValidator.Object);

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

            // Act
            var actualResponse = await controller.UpdateCountry(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(400, statusCodeObject.StatusCode);
            Assert.Equal("Country cannot be empty", statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateCountry_ReturnsUpdated()
        {
            // Arrange
            var logger = new Mock<ILogger<AddressDetailsController>>();
            var addressDetailsManager = new Mock<IAddressDetailsManager>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var expectedResponse = new ActionResponse(true, 200, "Updated");

            var controller = new AddressDetailsController(logger.Object, addressDetailsManager.Object, requestValidator.Object);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            requestValidator.Setup(service => service.IsStringValid(It.IsAny<string>())).Returns(true);

            addressDetailsManager.Setup(service => service.UpdateCountry(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(expectedResponse);

            // Act
            var actualResponse = await controller.UpdateCountry(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(expectedResponse.StatusCode, statusCodeObject.StatusCode);
            Assert.Equal(expectedResponse.Message, statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateCity_ReturnsFailedToRetrieveUserId()
        {
            // Arrange
            var logger = new Mock<ILogger<AddressDetailsController>>();
            var addressDetailsManager = new Mock<IAddressDetailsManager>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new AddressDetailsController(logger.Object, addressDetailsManager.Object, requestValidator.Object);

            // Invalid Guid in User Claims
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, "invalidId") };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            // Act
            var actualResponse = await controller.UpdateCity(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(500, statusCodeObject.StatusCode);
            Assert.Equal("Failed to retrieve User Id", statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateCity_ReturnsFieldCannotBeEmpty()
        {
            // Arrange
            var logger = new Mock<ILogger<AddressDetailsController>>();
            var addressDetailsManager = new Mock<IAddressDetailsManager>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new AddressDetailsController(logger.Object, addressDetailsManager.Object, requestValidator.Object);

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

            // Act
            var actualResponse = await controller.UpdateCity(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(400, statusCodeObject.StatusCode);
            Assert.Equal("City cannot be empty", statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateCity_ReturnsUpdated()
        {
            // Arrange
            var logger = new Mock<ILogger<AddressDetailsController>>();
            var addressDetailsManager = new Mock<IAddressDetailsManager>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var expectedResponse = new ActionResponse(true, 200, "Updated");

            var controller = new AddressDetailsController(logger.Object, addressDetailsManager.Object, requestValidator.Object);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            requestValidator.Setup(service => service.IsStringValid(It.IsAny<string>())).Returns(true);

            addressDetailsManager.Setup(service => service.UpdateCity(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(expectedResponse);

            // Act
            var actualResponse = await controller.UpdateCity(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(expectedResponse.StatusCode, statusCodeObject.StatusCode);
            Assert.Equal(expectedResponse.Message, statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateStreet_ReturnsFailedToRetrieveUserId()
        {
            // Arrange
            var logger = new Mock<ILogger<AddressDetailsController>>();
            var addressDetailsManager = new Mock<IAddressDetailsManager>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new AddressDetailsController(logger.Object, addressDetailsManager.Object, requestValidator.Object);

            // Invalid Guid in User Claims
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, "invalidId") };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            // Act
            var actualResponse = await controller.UpdateStreet(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(500, statusCodeObject.StatusCode);
            Assert.Equal("Failed to retrieve User Id", statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateStreet_ReturnsFieldCannotBeEmpty()
        {
            // Arrange
            var logger = new Mock<ILogger<AddressDetailsController>>();
            var addressDetailsManager = new Mock<IAddressDetailsManager>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new AddressDetailsController(logger.Object, addressDetailsManager.Object, requestValidator.Object);

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

            // Act
            var actualResponse = await controller.UpdateStreet(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(400, statusCodeObject.StatusCode);
            Assert.Equal("Street cannot be empty", statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateStreet_ReturnsUpdated()
        {
            // Arrange
            var logger = new Mock<ILogger<AddressDetailsController>>();
            var addressDetailsManager = new Mock<IAddressDetailsManager>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var expectedResponse = new ActionResponse(true, 200, "Updated");

            var controller = new AddressDetailsController(logger.Object, addressDetailsManager.Object, requestValidator.Object);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            requestValidator.Setup(service => service.IsStringValid(It.IsAny<string>())).Returns(true);

            addressDetailsManager.Setup(service => service.UpdateStreet(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(expectedResponse);

            // Act
            var actualResponse = await controller.UpdateStreet(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(expectedResponse.StatusCode, statusCodeObject.StatusCode);
            Assert.Equal(expectedResponse.Message, statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateHouseNumber_ReturnsFailedToRetrieveUserId()
        {
            // Arrange
            var logger = new Mock<ILogger<AddressDetailsController>>();
            var addressDetailsManager = new Mock<IAddressDetailsManager>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new AddressDetailsController(logger.Object, addressDetailsManager.Object, requestValidator.Object);

            // Invalid Guid in User Claims
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, "invalidId") };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            // Act
            var actualResponse = await controller.UpdateHouseNumber(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(500, statusCodeObject.StatusCode);
            Assert.Equal("Failed to retrieve User Id", statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateHouseNumber_ReturnsFieldCannotBeEmpty()
        {
            // Arrange
            var logger = new Mock<ILogger<AddressDetailsController>>();
            var addressDetailsManager = new Mock<IAddressDetailsManager>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new AddressDetailsController(logger.Object, addressDetailsManager.Object, requestValidator.Object);

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

            // Act
            var actualResponse = await controller.UpdateHouseNumber(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(400, statusCodeObject.StatusCode);
            Assert.Equal("HouseNumber cannot be empty", statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateHouseNumber_ReturnsUpdated()
        {
            // Arrange
            var logger = new Mock<ILogger<AddressDetailsController>>();
            var addressDetailsManager = new Mock<IAddressDetailsManager>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var expectedResponse = new ActionResponse(true, 200, "Updated");

            var controller = new AddressDetailsController(logger.Object, addressDetailsManager.Object, requestValidator.Object);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            requestValidator.Setup(service => service.IsStringValid(It.IsAny<string>())).Returns(true);

            addressDetailsManager.Setup(service => service.UpdateHouseNumber(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(expectedResponse);

            // Act
            var actualResponse = await controller.UpdateHouseNumber(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(expectedResponse.StatusCode, statusCodeObject.StatusCode);
            Assert.Equal(expectedResponse.Message, statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateApartamentNumber_ReturnsFailedToRetrieveUserId()
        {
            // Arrange
            var logger = new Mock<ILogger<AddressDetailsController>>();
            var addressDetailsManager = new Mock<IAddressDetailsManager>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new AddressDetailsController(logger.Object, addressDetailsManager.Object, requestValidator.Object);

            // Invalid Guid in User Claims
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, "invalidId") };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            // Act
            var actualResponse = await controller.UpdateApartamentNumber(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(500, statusCodeObject.StatusCode);
            Assert.Equal("Failed to retrieve User Id", statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateApartamentNumber_ReturnsFieldCannotBeEmpty()
        {
            // Arrange
            var logger = new Mock<ILogger<AddressDetailsController>>();
            var addressDetailsManager = new Mock<IAddressDetailsManager>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var controller = new AddressDetailsController(logger.Object, addressDetailsManager.Object, requestValidator.Object);

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

            // Act
            var actualResponse = await controller.UpdateApartamentNumber(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(400, statusCodeObject.StatusCode);
            Assert.Equal("ApartamentNumber cannot be empty", statusCodeObject.Value);
        }

        [Fact]
        public async void UpdateApartamentNumber_ReturnsUpdated()
        {
            // Arrange
            var logger = new Mock<ILogger<AddressDetailsController>>();
            var addressDetailsManager = new Mock<IAddressDetailsManager>();
            var requestValidator = new Mock<IRequestDataValidator>();

            var expectedResponse = new ActionResponse(true, 200, "Updated");

            var controller = new AddressDetailsController(logger.Object, addressDetailsManager.Object, requestValidator.Object);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            requestValidator.Setup(service => service.IsStringValid(It.IsAny<string>())).Returns(true);

            addressDetailsManager.Setup(service => service.UpdateApartamentNumber(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(expectedResponse);

            // Act
            var actualResponse = await controller.UpdateApartamentNumber(It.IsAny<string>());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(expectedResponse.StatusCode, statusCodeObject.StatusCode);
            Assert.Equal(expectedResponse.Message, statusCodeObject.Value);
        }
    }
}
