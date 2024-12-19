using System.Reflection;

using Ninject;

using SkyBro.RequestHandler;
using SkyBro.Authentication;

namespace SkyBro;

public static class DependencyResolver
{
    private static IKernel Kernel { get; set; }

    public static T Resolve<T>() => Kernel.Get<T>();

    public static void SetKernel(IKernel kernel) => Kernel = kernel;

    static DependencyResolver()
    {
        Kernel = new StandardKernel();
        Kernel.Load(Assembly.GetExecutingAssembly());
        ConfigureBindings();
    }

    public static void ResetKernel()
    {
        Kernel = new StandardKernel();
        Kernel.Load(Assembly.GetExecutingAssembly());
        ConfigureBindings();
    }

    private static void ConfigureBindings()
    {
        Kernel.Bind<ISkillRequestDispatcher>().To<SkillRequestDispatcher>();
        Kernel.Bind<IAuthenticator>().To<APIKeyAuthenticator>()
            .WhenInjectedInto<CheckWXClient>()
            .WithConstructorArgument("X-API-Key", "c16c30d8736041dfb52ef4dc2c");

        Kernel.Bind<HttpClient>()
            .ToMethod(context => new HttpClient());
        Kernel.Bind<HttpClient>()
            .ToMethod(context => new HttpClient())
            .WhenInjectedInto<CheckWXClient>()
            .InSingletonScope();
        
        Kernel.Bind<CheckWXClient>().ToSelf();
    }
}