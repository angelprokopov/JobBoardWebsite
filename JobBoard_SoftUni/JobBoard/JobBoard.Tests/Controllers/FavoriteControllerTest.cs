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

            mockupFavoriteRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Favorite>().AsQueryable());

            var controller = new FavoriteController(mockupFavoriteRepo.Object,  mockupJobRepo.Object);

            var user = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity(new[]
            {
                new System.Security.Claims.Claim("UserId", userId.ToString())
            }));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User =  user }   
            };

            var result = await controller.AddToFavorites(jobId, userId);
        }
    }
}
