using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using Castle.Windsor.Installer;
using ChainOfResponsibility.Interfaces;
using ChainOfResponsibility.Middleware;

namespace ChainOfResponsibility
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var container = new WindsorContainer();
            using (container.BeginScope())
            {
                container.Install(FromAssembly.InThisApplication(Assembly.GetExecutingAssembly()));
                var requestProcessor = container.Resolve<IRequestProcessor>();

                requestProcessor.AddStep<LessThan0Middleware>();
                requestProcessor.AddStep<LessThan5Middleware>();
                requestProcessor.AddStep<LessThan10Middleware>();
                Console.WriteLine(await requestProcessor.Run(7) as string);
                Console.ReadLine();
            }
        }
    }
}
