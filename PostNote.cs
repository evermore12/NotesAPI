using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class TableData
    {
        public string Note { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }
    public class NoteData
    {
        public string username { get; set; }
        public string note { get; set; }
    }
    public static class PostNote
    {
        [Function("PostNote")]
        [TableOutput("Notes", Connection = "AzureWebJobsStorage")]
        public static async Task<TableData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            NoteData json = await req.ReadFromJsonAsync<NoteData>();
            return new TableData{
                Note = json.note,
                PartitionKey = json.username,
                RowKey = "1",
            };
        }
    }
}
