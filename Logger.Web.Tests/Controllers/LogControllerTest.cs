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
    public class LogControllerTest
    {
        private LogController _controller;
        private Mock<ILogService> _mock;

        [TestInitialize]
        public void TestInit()
        {
            _mock = new Mock<ILogService>();
            _controller = new LogController(_mock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof (HttpResponseException), "400 Bad Request")]
        public async Task LoggingEndpoint_NullRequest()
        {
            // Arrange
            //N/A

            // Act
            var response = await _controller.LoggingEndpoint(null);

            // Assert
            //N/A
        }

        [TestMethod]
        [ExpectedException(typeof (HttpResponseException), "400 Bad Request")]
        public async Task LoggingEndpoint_NoHeader()
        {
            // Arrange
            var request = new LoggingEndpointRequest();
            _controller.Request = new HttpRequestMessage();

            // Act
            var response = await _controller.LoggingEndpoint(request);

            // Assert
            //N/A
        }

        [TestMethod]
        [ExpectedException(typeof (HttpResponseException), "403 Forbidden")]
        public async Task LoggingEndpoint_InvalidToken()
        {
            // Arrange
            var request = new LoggingEndpointRequest();
            _controller.Request = new HttpRequestMessage();
            _controller.Request.Headers.Add("access_token", new List<string> {"token"});
            _mock.Setup(x => x.CheckAccessToken("token")).Returns(false);

            // Act
            var response = await _controller.LoggingEndpoint(request);

            // Assert
            //N/A
        }

        [TestMethod]
        [ExpectedException(typeof (HttpResponseException), "403 Forbidden")]
        public async Task LoggingEndpoint_LimitExceeded()
        {
            // Arrange
            var request = new LoggingEndpointRequest();
            _controller.Request = new HttpRequestMessage();
            _controller.Request.Headers.Add("access_token", new List<string> {"token"});
            _mock.Setup(x => x.CheckAccessToken("token")).Returns(true);
            _mock.Setup(x => x.UpdateNCheckLimitExceeded("token")).Returns(true);

            // Act
            var response = await _controller.LoggingEndpoint(request);

            // Assert
            //N/A
        }

        [TestMethod]
        [ExpectedException(typeof (HttpResponseException), "403 Forbidden")]
        public async Task LoggingEndpoint_NullResponse()
        {
            // Arrange
            var request = new LoggingEndpointRequest();
            _controller.Request = new HttpRequestMessage();
            _controller.Request.Headers.Add("access_token", new List<string> {"token"});
            _mock.Setup(x => x.CheckAccessToken("token")).Returns(true);
            _mock.Setup(x => x.UpdateNCheckLimitExceeded("token")).Returns(false);
            _mock.Setup(x => x.AddLog(request)).ReturnsAsync(null);

            // Act
            var response = await _controller.LoggingEndpoint(request);

            // Assert
            //N/A
        }

        [TestMethod]
        public async Task LoggingEndpoint_Success()
        {
            // Arrange
            var request = new LoggingEndpointRequest();
            _controller.Request = new HttpRequestMessage();
            _controller.Request.Headers.Add("access_token", new List<string> {"token"});
            _mock.Setup(x => x.CheckAccessToken("token")).Returns(true);
            _mock.Setup(x => x.UpdateNCheckLimitExceeded("token")).Returns(false);
            _mock.Setup(x => x.AddLog(request)).ReturnsAsync(new LoggingEndpointResponse());

            // Act
            var response = await _controller.LoggingEndpoint(request);

            // Assert
            Assert.IsNotNull(response);
        }
    }
}