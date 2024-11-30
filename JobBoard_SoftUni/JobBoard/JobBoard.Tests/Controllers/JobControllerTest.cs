using JobBoard.Data.Models;
using JobBoard.Controllers;
using JobBoard.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using JobBoard.Interfaces;


namespace JobBoard.Tests.Controllers
{
    public class JobControllerTest
    {
        [Fact]
        public async Task Index_ReturnsViewResult_WithAListOfJobs()
        {
            var mockupRepo = new Mock<IRepository<Job>>();
            mockupRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Job>
                {
                    new Job {Id = new Guid(), Title = ""},
                    new Job {Id = new Guid(), Title = "" }
                });

            var controller = new JobController(mockupRepo.Object);

            var result = await controller.All();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Job>>(viewResult.ViewData.Model);

            Assert.Equal(1, model.Count());
        }

        private List<Job> GetTestJobs()
        {
            return new List<Job>
            {
                new Job {Id = new Guid(), Title = "" },
                new Job {Id = new Guid(), Title = "" }
            };
        }
    }
}