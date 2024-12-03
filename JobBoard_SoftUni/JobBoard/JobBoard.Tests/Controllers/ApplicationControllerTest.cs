using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Data.Models;
using JobBoard.Controllers;
using Microsoft.AspNetCore.Mvc;
using JobBoard.Data.Interfaces;

namespace JobBoard.Tests.Controllers
{
    public class ApplicationControllerTest
    {
        [Fact]
        public async Task Apply_ValidJobAndUser_ReturnsRedirectToAction()
        {
            var jobId = new Guid();
            var mockupApplicationRepo = new Mock<IRepository<Applications>>();
            var mockupJobRepo = new Mock<IRepository<Job>>();
            var controller = new ApplicationController(mockupApplicationRepo.Object, mockupJobRepo.Object);

            var testApplication = new Applications
            {
                Id = new Guid(),
                UserId = Guid.NewGuid(),
                JobId = new Guid()
            };

            var result = await controller.Apply(jobId);

            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToAction.ActionName);
        }
    }
}
