using System;
using Logger.Service;
using Logger.Service.Interfaces;
using Logger.Web.Controllers;
using Logger.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Logger.Web.Tests
{
    [TestClass]
    public class LimitExceededTest
    {
        private LogController _controller;
        [TestInitialize]
        public void TestInit()
        {
           Mock mock = new Mock<DbContext>();
            ILogService logService = new LogService();

            _controller = new LogController(_mock.Object);
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
