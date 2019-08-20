using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ChainOfResponsibility.Interfaces;
using ChainOfResponsibility.Middleware;

namespace ChainOfResponsibility.IoC
{
    public class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IWindsorContainer>().Instance(container));
            container.Register(Component.For<IRequestProcessor>().ImplementedBy<RequestProcessor>());
            container.Register(Component.For<IRequestProcessorMiddlewareResolver>().ImplementedBy<RequestProcessorMiddlewareResolver>());
            container.Register(Classes.FromAssemblyInThisApplication(Assembly.GetExecutingAssembly())
                .BasedOn<IMiddleware>());
        }
    }
}