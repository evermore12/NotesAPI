using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class GetNotes
    {
        [Function("GetNotes")]
        public static async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            FunctionContext executionContext,
            [TableInput("Notes", Connection ="AzureWebJobsStorage")] TableData[] entries,
            string name)
        {
            IEnumerable<TableData> userNotes = entries.Where(x => x.PartitionKey == name);
            var logger = executionContext.GetLogger("GetNotes");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync<IEnumerable<TableData>>(userNotes);

            return response;
        }
    }
}
