using HttpTriggerModelBindingLab.Domains;
using HttpTriggerModelBindingLab.Domains.Interfaces;
using HttpTriggerModelBindingLab.Middlewares;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication((IFunctionsWorkerApplicationBuilder builder) =>
    {
        builder.UseMiddleware<BindModelValidationMiddleware>();
    })
    .ConfigureServices(services => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddSingleton<ItemDeserializer>();
        services.AddSingleton<BookDeserializer>();
        services.AddSingleton<Func<string, IDeserializer>>(provider => key =>
        {
           return key switch
            {
                "/api/Function1" => provider.GetService<ItemDeserializer>()!,
                "/api/Function2" => provider.GetService<BookDeserializer>()!,
                _ => throw new KeyNotFoundException()
            };
        });
    })
    .Build();

host.Run();
