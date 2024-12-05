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
using Microsoft.AspNetCore.Http;
using JobBoard.Data.Interfaces;
using System.Linq.Expressions;
using System.Security.Claims;

namespace JobBoard.Tests.Controllers
{
    public class FavoriteControllerTest
    {
        [Fact]
        public async Task AddToFavorite_ValidJobId_RedirectsToIndex()
        {
            var userId = Guid.NewGuid();
            var jobId = Guid.NewGuid();

            var mockupFavoriteRepo = new Mock<IRepository<Favorite>>();
            var mockupJobRepo = new Mock<IRepository<Job>>();

            mockupFavoriteRepo.Setup(repo => repo.GetAllAsync(
                It.IsAny<Expression<Func<Favorite, bool>>>(),
                It.IsAny<Expression<Func<Favorite, object>>[]>()
            ))
            .ReturnsAsync(new List<Favorite>());

            mockupJobRepo.Setup(repo => repo.GetByIdAsync(jobId))
        .ReturnsAsync(new Job { Id = jobId, Title = "Test Job" });

            // Set up the controller with the mock repositories
            var controller = new FavoriteController(mockupFavoriteRepo.Object, mockupJobRepo.Object);

            // Create a claims principal representing the logged-in user
            var user = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity(new[]
            {
                new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await controller.AddToFavorites(jobId);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }
    }
}
