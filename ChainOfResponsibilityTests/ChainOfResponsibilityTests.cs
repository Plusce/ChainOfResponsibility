using System.Threading.Tasks;
using ChainOfResponsibility.Interfaces;
using ChainOfResponsibility.Middleware;
using FluentAssertions;
using Xunit;

namespace ChainOfResponsibility.Tests
{
    public class ChainOfResponsibilityTests
    {
        [Fact]
        public async Task Run_WithoutMiddleware_ShouldInformThatNoMiddlewareIsSet()
        {
            // Arrange
            var requestProcessor = Arrange_RequestProcessor();
            var request = 5;

            // Act
            var response = await requestProcessor.Run(request);

            // Assert
            response.Should().Be(Resources.NoMiddlewareIsSet);
        }

        [Fact]
        public async Task Run_WithSingleMiddleware_ShouldNotUnhandled()
        {
            // Arrange
            var (lessThan0Middleware, _) = Arrange_LessThan0Middleware();
            var requestProcessor = Arrange_RequestProcessor();
            requestProcessor.AddStep(lessThan0Middleware);
            var request = 5;

            // Act
            var response = await requestProcessor.Run(request);

            // Assert
            response.Should().Be(Resources.NoMiddlewareAbleToHandleRequest);
        }

        [Fact]
        public async Task Run_WithThreeMiddleware_ShouldBeHandledBySecondOne()
        {
            // Arrange
            var (lessThan0Middleware, _) = Arrange_LessThan0Middleware();
            var (lessThan5Middleware, lessThan5MiddlewareName) = Arrange_LessThan5Middleware();
            var (lessThan10Middleware, _) = Arrange_LessThan10Middleware();
            var requestProcessor = Arrange_RequestProcessor();
            requestProcessor.AddStep(lessThan0Middleware);
            requestProcessor.AddStep(lessThan5Middleware);
            requestProcessor.AddStep(lessThan10Middleware);
            var request = 3;

            // Act
            var response = await requestProcessor.Run(request);

            // Assert
            response.Should().Be(string.Format(Resources.RequestProperlyHandled2Args, request, lessThan5MiddlewareName));
        }

        [Fact]
        public async Task Run_WithThreeMiddleware_ShouldBeUnhandledByAllThree()
        {
            // Arrange
            var (lessThan0Middleware, _) = Arrange_LessThan0Middleware();
            var (lessThan5Middleware, _) = Arrange_LessThan5Middleware();
            var (lessThan10Middleware, _) = Arrange_LessThan10Middleware();
            var requestProcessor = Arrange_RequestProcessor();
            requestProcessor.AddStep(lessThan0Middleware);
            requestProcessor.AddStep(lessThan5Middleware);
            requestProcessor.AddStep(lessThan10Middleware);
            var request = 11;

            // Act
            var response = await requestProcessor.Run(request);

            // Assert
            response.Should().Be(Resources.NoMiddlewareAbleToHandleRequest);
        }

        private (IMiddleware, string) Arrange_LessThan0Middleware() => 
            (new LessThan0Middleware(), nameof(LessThan0Middleware));

        private (IMiddleware, string) Arrange_LessThan5Middleware() =>
            (new LessThan5Middleware(), nameof(LessThan5Middleware));

        private (IMiddleware, string) Arrange_LessThan10Middleware() =>
            (new LessThan10Middleware(), nameof(LessThan10Middleware));

        private IRequestProcessor Arrange_RequestProcessor() => new RequestProcessor();
    }
}