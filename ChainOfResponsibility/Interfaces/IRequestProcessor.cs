using System.Threading.Tasks;

namespace ChainOfResponsibility.Interfaces
{
    public interface IRequestProcessor
    {
        void AddStep<T>() where T : IMiddleware;

        Task<object> Run(object request);
    }
}