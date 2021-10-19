using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class PostNote
    {
        [Function("PostNote")]
        [TableOutput("Notes", Connection ="AzureWebJobsStorage")]
        public static async Task<NoteEntity> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route ="postnote/{partitionkey}")] HttpRequestData req,
            FunctionContext executionContext, [TableInput("Notes", partitionKey:"{partitionkey}", Connection = "AzureWebJobsStorage")] IEnumerable<NoteEntity> noteEntities)
        {
            NoteEntity entity = await req.ReadFromJsonAsync<NoteEntity>();

            if(entity.Version == 0)
            {
                entity.Lol = noteEntities.Max(x => x.Lol) + 1;
            }
            else
            {
                entity.Lol = noteEntities.Max(x => x.Lol);
            }

            entity.RowKey = Guid.NewGuid().ToString();

            HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);
            
            return entity;
        }
    }
}
