using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Need.ApiGateway.Controllers;
using Need.ApiGateway.Database;
using Need.ApiGateway.Models;
using Xunit;

namespace Need.ApiGateway.Tests.Controllers
{
    public class ToiletControllerTests
    {
        private const string TestId = "some-id";

        private readonly ToiletController _controller;
        private readonly IRepository<Toilet> _repository;

        public ToiletControllerTests()
        {
            _repository = A.Fake<IRepository<Toilet>>();

            _controller = new ToiletController(_repository);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Get_GivenAnIncorrectId_ReturnsABadRequest(string id)
        {
            var response = await _controller.Get(id);

            response.Result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Get_WhenToiletNotFound_ReturnsNotFound()
        {
            A.CallTo(() => _repository.GetAsync(TestId)).Returns(Task.FromResult<Toilet>(null));

            var response = await _controller.Get(TestId);

            response.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Get_WhenToiletFound_ReturnsOkAndAccount()
        {
            var toilet = new Toilet()
            {
                Id = TestId
            };

            A.CallTo(() => _repository.GetAsync(TestId)).Returns(Task.FromResult(toilet));

            var response = await _controller.Get(TestId);

            response.Result.Should().BeOfType<OkObjectResult>();

            var okResult = (OkObjectResult)response.Result;

            okResult.Value.Should().Be(toilet);
        }

        [Fact]
        public async Task Post_GivenToilet_ReturnsCreatedAtRouteAndToilet()
        {
            var toilet = new Toilet()
            {
                Owner = "Pedro",
                Location = "Spain"
            };

            A.CallTo(() => _repository.AddAsync(toilet)).Returns(Task.FromResult(TestId));

            var response = await _controller.Post(toilet);

            response.Result.Should().BeOfType<CreatedAtRouteResult>();

            var createdResult = (CreatedAtRouteResult)response.Result;

            createdResult.Value.Should().Be(toilet);
            createdResult.RouteName.Should().Be("Get");
            createdResult.RouteValues["id"].Should().Be(TestId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Put_GivenAnIncorrectId_ReturnsABadRequest(string id)
        {
            var response = await _controller.Put(id, new Toilet());

            response.Result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Put_GivenIdAndToilet_ReturnsAcceptedAtRouteAndToilet()
        {
            var toilet = new Toilet()
            {
                Id = TestId,
                Owner = "Pedro",
                Location = "Spain"
            };

            A.CallTo(() => _repository.UpdateAsync(TestId, toilet)).Returns(Task.CompletedTask);

            var response = await _controller.Put(TestId, toilet);

            response.Result.Should().BeOfType<AcceptedAtRouteResult>();

            var createdResult = (AcceptedAtRouteResult)response.Result;

            createdResult.Value.Should().Be(toilet);
            createdResult.RouteName.Should().Be("Get");
            createdResult.RouteValues["id"].Should().Be(TestId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Delete_GivenAnIncorrectId_ReturnsABadRequest(string id)
        {
            var response = await _controller.Delete(id);

            response.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Delete_WhenGivenCorrectId_ReturnsNoContent()
        {
            var response = await _controller.Delete(TestId);

            response.Should().BeOfType<NoContentResult>();
        }
    }
}