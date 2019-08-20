using System;
using System.Threading.Tasks;
using ChainOfResponsibility.Interfaces;

namespace ChainOfResponsibility.Middleware
{
    public class LessThan5Middleware : IMiddleware
    {
        public const int BoundaryValue = 5;

        public async Task<object> Handle(Func<Task<object>> next, object request)
        {
            if (request is int valueToProcess)
            {
                if (BoundaryValue > valueToProcess)
                {
                    return string.Format(Resources.RequestProperlyHandled2Args, request, this.GetType().Name);
                }
                else
                {
                    return await next();
                }
            }
            else
            {
                return Resources.NoMiddlewareAbleToHandleRequest;
            }
        }
    }
}