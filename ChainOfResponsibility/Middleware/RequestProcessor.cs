using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainOfResponsibility.Interfaces;

namespace ChainOfResponsibility.Middleware
{
    public class RequestProcessor : IRequestProcessor
    {
        private readonly IList<IMiddleware> middlewareList;
        private readonly IRequestProcessorMiddlewareResolver middlewareResolver;
        private readonly IList<Type> middlewareTypes;

        public RequestProcessor(IRequestProcessorMiddlewareResolver middlewareResolver)
        {
            middlewareList = new List<IMiddleware>();
            middlewareTypes = new List<Type>();
            this.middlewareResolver = middlewareResolver;
        }

        public void AddStep<T>() where T : IMiddleware
        {
            middlewareTypes.Add(typeof(T));
        }

        public async Task<object> Run(object request)
        {
            InitMiddlewareList();
            if (middlewareList.Any())
            {
                var startIndex = 0;
                return await middlewareList[startIndex].Handle(CreateNextFunc(++startIndex), request);
            }

            return Resources.NoMiddlewareIsSet;

            Func<Task<object>> CreateNextFunc(int index)
            {
                if (index + 1 < middlewareList.Count)
                {
                    return async () => await middlewareList[index].Handle(CreateNextFunc(++index), request);
                }

                return async () => await middlewareList[index].Handle(async () => Resources.NoMiddlewareAbleToHandleRequest,
                    request);
            }

            void InitMiddlewareList()
            {
                if (middlewareTypes.Any())
                {
                    foreach (var middlewareType in middlewareTypes)
                    {
                        middlewareList.Add(middlewareResolver.Resolve(middlewareType));
                    }
                }
            }
        }
    }
}