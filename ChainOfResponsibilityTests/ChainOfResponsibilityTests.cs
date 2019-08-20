using System.Threading.Tasks;
using Castle.Windsor;
using ChainOfResponsibility.Interfaces;
using ChainOfResponsibility.IoC;
using ChainOfResponsibility.Middleware;
using FluentAssertions;
using Xunit;

namespace ChainOfResponsibility.Tests
{
    public class ChainOfResponsibilityTests
    {
        private static IWindsorContainer Container => WindsorContainerSingleton.Instance;

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
            var requestProcessor = Arrange_RequestProcessor();
            requestProcessor.AddStep<LessThan0Middleware>();
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
            var requestProcessor = Arrange_RequestProcessor();
            requestProcessor.AddStep<LessThan0Middleware>();
            requestProcessor.AddStep<LessThan5Middleware>();
            requestProcessor.AddStep<LessThan10Middleware>();
            var request = 3;

            // Act
            var response = await requestProcessor.Run(request);

            // Assert
            response.Should().Be(string.Format(Resources.RequestProperlyHandled2Args, request, nameof(LessThan5Middleware)));
        }

        [Fact]
        public async Task Run_WithThreeMiddleware_ShouldBeUnhandledByAllThree()
        {
            // Arrange
            var requestProcessor = Arrange_RequestProcessor();
            requestProcessor.AddStep<LessThan0Middleware>();
            requestProcessor.AddStep<LessThan5Middleware>();
            requestProcessor.AddStep<LessThan10Middleware>();
            var request = 11;

            // Act
            var response = await requestProcessor.Run(request);

            // Assert
            response.Should().Be(Resources.NoMiddlewareAbleToHandleRequest);
        }

        private IRequestProcessor Arrange_RequestProcessor() => Container.Resolve<IRequestProcessor>();
    }
}