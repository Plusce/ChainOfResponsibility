using System;

namespace ChainOfResponsibility.Interfaces
{
    public interface IRequestProcessorMiddlewareResolver
    {
        IMiddleware Resolve(Type type);
    }
}