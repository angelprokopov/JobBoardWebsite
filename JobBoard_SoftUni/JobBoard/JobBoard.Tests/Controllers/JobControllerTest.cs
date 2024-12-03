using JobBoard.Data.Models;
using JobBoard.Controllers;
using JobBoard.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using JobBoard.Data.Interfaces;


namespace JobBoard.Tests.Controllers
{
    public class JobControllerTest
    {
        [Fact]
        public async Task Index_ReturnsViewResult_WithAListOfJobs()
        {
            // Arrange
            var mockupJobRepo = new Mock<IRepository<Job>>();
            var mockupApplicationRepo = new Mock<IRepository<Applications>>();

            // Mocking the GetAllAsync method to return a list of jobs
            mockupJobRepo.Setup(repo => repo.GetAllAsync())
             .ReturnsAsync(new List<Job>
             {
                new Job { Id = Guid.NewGuid(), Title = "Software Engineer" },
                new Job { Id = Guid.NewGuid(), Title = "Data Analyst" }
             });

            var controller = new JobController(mockupJobRepo.Object, mockupApplicationRepo.Object);

            // Act
            var result = await controller.All("Engineer",1,10);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Job>>(viewResult.Model);    
            Assert.Equal(1, model.Count()); 
        }
    }
}