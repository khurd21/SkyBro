using System.Reflection;

using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

using Ninject;

using SkyBro.Authentication;
using SkyBro.RequestHandler;

namespace SkyBro;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public static class DependencyResolver
{
    private static IKernel Kernel { get; set; }

    private static bool IsConfigured { get; set; } = false;

    private static string? CheckWXApiKey { get; set; }

    public static T Resolve<T>()
    {
        if (!IsConfigured)
        {
            ConfigureBindings();
        }
        return Kernel.Get<T>();
    }

    public static void UseTestKernel(IKernel kernel)
    {
        Kernel = kernel;
        IsConfigured = true;
    }

    static DependencyResolver()
    {
        Kernel = new StandardKernel();
        Kernel.Load(Assembly.GetExecutingAssembly());
        IsConfigured = false;
    }

    private static void ConfigureBindings()
    {
        if (IsConfigured)
        {
            return;
        }
        Kernel.Bind<ISkillRequestDispatcher>().To<SkillRequestDispatcher>();
        Kernel.Bind<IAuthenticator>().To<APIKeyAuthenticator>()
            .WhenInjectedInto<CheckWXClient>()
            .WithConstructorArgument("X-API-Key", GetCheckWXApiKey());

        Kernel.Bind<HttpClient>()
            .ToMethod(context => new HttpClient());

        Kernel.Bind<CheckWXClient>().ToSelf();
        IsConfigured = true;
    }

    private static string GetCheckWXApiKey()
    {
        if (!string.IsNullOrEmpty(CheckWXApiKey))
        {
            return CheckWXApiKey;
        }
        try
        {
            var ssmClient = new AmazonSimpleSystemsManagementClient();
            CheckWXApiKey = ssmClient.GetParameterAsync(new GetParameterRequest
            {
                Name = "/SkyBro/CheckWXClient/ApiKey",
                WithDecryption = true
            }).ConfigureAwait(false).GetAwaiter().GetResult().Parameter.Value;
        }
        catch (AggregateException ex) when (ex.InnerException is AmazonSimpleSystemsManagementException)
        {
            CheckWXApiKey = string.Empty;
        }
        return CheckWXApiKey;
    }
}