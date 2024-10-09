using HttpTriggerModelBindingLab.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace HttpTriggerModelBindingLab
{
    public class Function2
    {
        private readonly ILogger<Function2> _logger;

        public Function2(ILogger<Function2> logger)
        {
            _logger = logger;
        }

        [Function("Function2")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, [FromBody] Book book)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(book);
            return response;
        }
    }
}
