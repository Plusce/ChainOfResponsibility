using System;
using Castle.Windsor;
using ChainOfResponsibility.Interfaces;

namespace ChainOfResponsibility.Middleware
{
    public class RequestProcessorMiddlewareResolver : IRequestProcessorMiddlewareResolver
    {
        private readonly IWindsorContainer container;

        public RequestProcessorMiddlewareResolver(IWindsorContainer container)
        {
            this.container = container;
        }

        public IMiddleware Resolve(Type type)
        {
            if (typeof(IMiddleware).IsAssignableFrom(type))
            {
                return (IMiddleware) container.Resolve(type);
            }

            return null;
        }
    }
}