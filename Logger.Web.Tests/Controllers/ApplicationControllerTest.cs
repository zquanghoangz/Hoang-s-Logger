using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Logger.Cross.Requests;
using Logger.Cross.Responses;
using Logger.Service.Interfaces;
using Logger.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Logger.Web.Tests.Controllers
{
    [TestClass]
    public class ApplicationControllerTest
    {
        private ApplicationController _controller;
        private Mock<IApplicationService> _mock;

        [TestInitialize]
        public void TestInit()
        {
            _mock = new Mock<IApplicationService>();
            _controller = new ApplicationController(_mock.Object);
        }

        #region RegisterEndpoint Test

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "400 Bad Request")]
        public async Task RegisterEndpoint_NullRequest()
        {
            // Arrange
            //N/A

            // Act
            var response = await _controller.RegisterEndpoint(null);

            // Assert
            //N/A
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "403 Forbidden")]
        public async Task RegisterEndpoint_NullResponse()
        {
            // Arrange
            RegisterEndpointRequest request = new RegisterEndpointRequest();
            _mock.Setup(x => x.Register("application display name")).ReturnsAsync(null);

            // Act
            var response = await _controller.RegisterEndpoint(request);

            // Assert
            //N/A
        }

        [TestMethod]
        public async Task RegisterEndpoint_Success()
        {
            // Arrange
            var request = new RegisterEndpointRequest { DisplayName = "application display name" };
            _mock.Setup(x => x.Register(request.DisplayName)).ReturnsAsync(new RegisterEndpointResponse());

            // Act
            var response = await _controller.RegisterEndpoint(request);

            // Assert
            Assert.IsNotNull(response);
        }

        #endregion

        #region Authorization Test

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "400 Bad Request")]
        public async Task Authorization_NoHeader()
        {
            // Arrange
            _controller.Request = new HttpRequestMessage();

            // Act
            var response = await _controller.Authorization();

            // Assert
            //N/A
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "401 Unauthorized")]
        public async Task Authorization_NullResponse()
        {
            // Arrange
            _controller.Request = new HttpRequestMessage();
            _controller.Request.Headers.Add("application_id", new List<string> { "application id" });
            _controller.Request.Headers.Add("application_secret", new List<string> { "application secret" });
            _mock.Setup(x => x.GetAccessToken("applicationId", "applicationSecret")).ReturnsAsync(null);

            // Act
            var response = await _controller.Authorization();

            // Assert
            //N/A
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException), "406 Not Acceptable. Only one active session per application is allowed")]
        public async Task Authorization_CannotSave()
        {
            // Arrange
            _controller.Request = new HttpRequestMessage();
            _controller.Request.Headers.Add("application_id", new List<string> { "applicationId" });
            _controller.Request.Headers.Add("application_secret", new List<string> { "applicationSecret" });
            _mock.Setup(x => x.GetAccessToken("applicationId", "applicationSecret")).ReturnsAsync(new AuthorizationResponse());
            _mock.Setup(x => x.SaveAccessToken("token", "applicationId")).ReturnsAsync(false);

            // Act
            var response = await _controller.Authorization();

            // Assert
            //N/A
        }

        [TestMethod]
        public async Task Authorization_Success()
        {
            // Arrange
            _controller.Request = new HttpRequestMessage();
            _controller.Request.Headers.Add("application_id", new List<string> { "applicationId" });
            _controller.Request.Headers.Add("application_secret", new List<string> { "applicationSecret" });

            var expectResponse = new AuthorizationResponse { AccessToken = "token" };
            var applicationId = "applicationId";
            var applicationSecret = "applicationSecret";
            _mock.Setup(x => x.GetAccessToken(applicationId, applicationSecret)).ReturnsAsync(expectResponse);
            _mock.Setup(x => x.SaveAccessToken("token", "applicationId")).ReturnsAsync(true);
            
            // Act
            var response = await _controller.Authorization();

            // Assert
            _mock.VerifyAll();
            Assert.IsNotNull(response);
        }

        #endregion

    }
}
