using System;
using System.Threading.Tasks;
using ChainOfResponsibility.Interfaces;

namespace ChainOfResponsibility.Middleware
{
    public class LessThan0Middleware : IMiddleware
    {
        public const int BoundaryValue = 0;

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
                    try
                    {
                        return await next();
                    }
                    catch
                    {
                        return Resources.NoMiddlewareAbleToHandleRequest;
                    }
                }
            }
            else
            {
                return Resources.NoMiddlewareAbleToHandleRequest;
            }
        }
    }
}