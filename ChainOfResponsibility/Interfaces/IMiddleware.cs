using System;
using System.Threading.Tasks;

namespace ChainOfResponsibility.Interfaces
{
    public interface IMiddleware
    {
        Task<object> Handle(Func<Task<object>> next, object request);
    }
}