using System;
using System.Threading.Tasks;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using ChainOfResponsibility.Interfaces;
using ChainOfResponsibility.IoC;
using ChainOfResponsibility.Middleware;

namespace ChainOfResponsibility
{
    class Program
    {
        private static IWindsorContainer Container => WindsorContainerSingleton.Instance;

        static async Task Main(string[] args)
        {
            using (Container.BeginScope())
            {
                var requestProcessor = Container.Resolve<IRequestProcessor>();
                requestProcessor.AddStep<LessThan0Middleware>();
                requestProcessor.AddStep<LessThan5Middleware>();
                requestProcessor.AddStep<LessThan10Middleware>();
                Console.WriteLine(await requestProcessor.Run(7) as string);
                Console.ReadLine();
            }
        }
    }
}
