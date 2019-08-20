using System.Reflection;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace ChainOfResponsibility.IoC
{
    public static class WindsorContainerSingleton
    {
        public static IWindsorContainer Instance;

        static WindsorContainerSingleton()
        {
            Instance = new WindsorContainer();
            Instance.Install(FromAssembly.InThisApplication(Assembly.GetExecutingAssembly()));
        }
    }
}