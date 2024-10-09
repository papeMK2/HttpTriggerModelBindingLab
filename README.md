# HttpTriggerModelBindingLab
Experiments with model validation for custom models bound by Http trigger in Azure Functions.

# How to use

Add BindModelValidationMIddleware in ConfigureFunctionsWebApplication().
```cs
.ConfigureFunctionsWebApplication((IFunctionsWorkerApplicationBuilder builder) =>
{
    builder.UseMiddleware<BindModelValidationMiddleware>();
})
```

Add custom model deserializer in ConfigureServices().

The key is the path that the Http trigger listens on.

```cs
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
```

When defining a custom model, implement the Interface of IBindModel.

```cs
public class Book : IBindModel
{
    public required string Author {  get; set; }
}
```