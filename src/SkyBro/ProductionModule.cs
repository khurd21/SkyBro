using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

using Ninject;
using Ninject.Modules;

using SkyBro.Authentication;
using SkyBro.Metar;
using SkyBro.RequestHandler;

namespace SkyBro;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
class ProductionModule : NinjectModule
{
    private static string CheckWXApiKey { get; set; } = string.Empty;

    public override void Load()
    {
        Bind<IAuthenticator>().To<APIKeyAuthenticator>()
            .WhenInjectedInto<CheckWXClient>()
            .InSingletonScope()
            .WithConstructorArgument("key", GetCheckWXApiKey())
            .WithConstructorArgument("headerName", "X-API-Key");

        Bind<HttpClient>()
            .ToMethod(context => new HttpClient())
            .WhenInjectedInto<CheckWXClient>()
            .InSingletonScope();

        Bind<IMetarEndpoint>().To<CheckWXClient>();

        Bind<ISkillRequestDispatcher>().To<SkillRequestDispatcher>();
        Bind<ISkillRequestHandler>().To<LaunchRequestHandler>();
        Bind<ISkillRequestHandler>().To<SessionEndedRequestHandler>();
        Bind<ISkillRequestHandler>().To<CancelIntentHandler>();
        Bind<ISkillRequestHandler>().To<HelpIntentHandler>();
        Bind<ISkillRequestHandler>().To<WeatherObservationsIntentHandler>();
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