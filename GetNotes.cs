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
        public static async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "getnotes/{partitionkey}")] HttpRequestData req,
            FunctionContext executionContext,
            [TableInput("Notes", partitionKey:"{partitionkey}", Connection ="AzureWebJobsStorage")] IEnumerable<NoteEntity> entries)
        {
            List<NoteEntity> selectedEntries =  entries.Where(x => x.Version == entries.Where(y => y.Lol == x.Lol).Max(z => z.Version)).ToList();
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync<IEnumerable<NoteEntity>>(selectedEntries);

            return response;
        }
    }
}
