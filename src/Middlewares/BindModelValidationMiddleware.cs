using HttpTriggerModelBindingLab.Domains.Interfaces;
using HttpTriggerModelBindingLab.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace HttpTriggerModelBindingLab.Middlewares;
/// <summary>
/// Provides middleware to perform binding validation against Http Trigger's custom model
/// </summary>
public class BindModelValidationMiddleware : IFunctionsWorkerMiddleware
{
    private Func<string, IDeserializer> _deserializerAccessor;
    public BindModelValidationMiddleware(Func<string, IDeserializer> deserializerAccessor)
    {
        _deserializerAccessor = deserializerAccessor;
    }
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var httpContext = context.GetHttpContext();
        if (httpContext is not null)
        {
            httpContext.Request.EnableBuffering();
        }

        var req = await context.GetHttpRequestDataAsync();
        var errorResponseBody = new ErrorResponse();
        if (req is not null)
        {
            var requestPath = req.Url.AbsolutePath;
            var requestBody = await req.ReadAsStringAsync();

            if (requestBody is not null)
            {
                // Check if it can be deserialized.
                var deserializer = _deserializerAccessor(requestPath);
                object? model = null;
                try
                {
                    model = deserializer.Deserialize(requestBody);
                }
                catch (JsonException ex)
                {
                    errorResponseBody.Errors.Add(ex.Message);
                    context.GetInvocationResult().Value = this.CreateErrorResponse(req, errorResponseBody);
                    return;
                }

                // Validate the model
                if (model is not null)
                {
                    var validationResults = new List<ValidationResult>();
                    var validationContext = new ValidationContext(model);

                    if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
                    {
                        errorResponseBody.Errors.AddRange(validationResults.Select(_ => _.ErrorMessage).ToList()!);

                        context.GetInvocationResult().Value = this.CreateErrorResponse(req, errorResponseBody);
                        return;
                    }
                }
            }
            req.Body.Position = 0;
        }

        await next(context);
    }

    #region Local function
    async ValueTask<HttpResponseData> CreateErrorResponse(HttpRequestData req, ErrorResponse errors)
    {
        var response = req.CreateResponse(HttpStatusCode.BadRequest);
        await response.WriteAsJsonAsync(errors);

        return response;
    }
    #endregion
}
