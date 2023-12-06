using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PersonaVault.Api.Controllers;
using PersonaVault.Business.Managers;
using PersonaVault.Business.Security;
using PersonaVault.Contracts.DTOs;
using PersonaVault.Contracts.Requests;
using PersonaVault.Contracts.Responses;
using System.Security.Claims;
using Xunit;

namespace PersonaVault.Api.Tests
{
    public class UserControllerTests
    {
        [Fact]
        public async void RegisterUser_ReturnsCorrectObjectResult()
        {
            // Arrange
            var userManager = new Mock<IUserManager>();
            var jwtService = new Mock<IJwtService>();
            var logger = new Mock<ILogger<UserController>>();

            userManager.Setup(service => service.RegisterUser(It.IsAny<RegisterUserRequest>())).ReturnsAsync(new ActionResponse(true, 201, "Successfully created"));

            var controller = new UserController(userManager.Object, jwtService.Object, logger.Object);

            // Act
            var actualResponse = await controller.Register(new RegisterUserRequest());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(201, statusCodeObject.StatusCode);
            Assert.Equal("Successfully created", statusCodeObject.Value);
        }

        [Fact]
        public async void Login_ReturnsCorrectObjectResult()
        {
            // Arrange
            var userManager = new Mock<IUserManager>();
            var jwtService = new Mock<IJwtService>();
            var logger = new Mock<ILogger<UserController>>();

            userManager.Setup(service => service.Login(It.IsAny<LoginRequest>())).ReturnsAsync(new LoginResponse(false, 400, "Invalid Credentials"));

            var controller = new UserController(userManager.Object, jwtService.Object, logger.Object);

            // Act
            var actualResponse = await controller.Login(new LoginRequest());

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(400, statusCodeObject.StatusCode);
            Assert.Equal("Invalid Credentials", statusCodeObject.Value);
        }

        [Fact]
        public async void GetFullUserData_ReturnsFailedToGetUserData()
        {
            // Arrange
            var userManager = new Mock<IUserManager>();
            var jwtService = new Mock<IJwtService>();
            var logger = new Mock<ILogger<UserController>>();

            userManager.Setup(service => service.GetFullUserData(It.IsAny<Guid>())).ReturnsAsync(null as FullUserData);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var controller = new UserController(userManager.Object, jwtService.Object, logger.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            // Act
            var actualResponse = await controller.GetFullUserData();

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(500, statusCodeObject.StatusCode);
            Assert.Equal("Failed to get and decrpyt full user data", statusCodeObject.Value);
        }

        [Fact]
        public async void GetFullUserData_ReturnsUserData()
        {
            // Arrange
            var expectedUserData = new FullUserData
            {
                Username = "test",
                Role = "User",
                Name = "test",
                LastName = "test",
                PersonalCode = "test",
                PhoneNumber = "test",
                EmailAddress = "test",
                Country = "test",
                City = "test",
                Street = "test",
                HouseNumber = "test",
                ApartamentNumber = "test",
            };
            var userManager = new Mock<IUserManager>();
            var jwtService = new Mock<IJwtService>();
            var logger = new Mock<ILogger<UserController>>();

            userManager.Setup(service => service.GetFullUserData(It.IsAny<Guid>())).ReturnsAsync(expectedUserData);

            // Valid Guid in User Claims
            var userId = Guid.NewGuid();
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var controller = new UserController(userManager.Object, jwtService.Object, logger.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            // Act
            var actualResponse = await controller.GetFullUserData();

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(200, statusCodeObject.StatusCode);
            Assert.Equal(expectedUserData, statusCodeObject.Value);
        }

        [Fact]
        public async void GetFullUserData_ReturnsFailedToRetrieveUserId()
        {
            //Arrange
            var userManager = new Mock<IUserManager>();
            var jwtService = new Mock<IJwtService>();
            var logger = new Mock<ILogger<UserController>>();

            // Invalid Guid in User Claims
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, "invalidId") };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var controller = new UserController(userManager.Object, jwtService.Object, logger.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };

            // Act
            var actualResponse = await controller.GetFullUserData();

            // Assert
            Assert.IsType<ObjectResult>(actualResponse);
            var statusCodeObject = actualResponse as ObjectResult;
            Assert.Equal(500, statusCodeObject.StatusCode);
            Assert.Equal("Failed to retrieve User Id", statusCodeObject.Value);
        }
    }
}