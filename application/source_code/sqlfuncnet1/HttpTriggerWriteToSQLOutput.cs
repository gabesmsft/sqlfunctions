using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace sqlfuncnet1
{
    public static class HttpTriggerWriteToSQLOutput
    {
        [FunctionName("HttpTriggerWriteToSQLOutput")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "WriteToSQL")] HttpRequest req,
            ILogger log,
            [Sql("dbo.ToDo", ConnectionStringSetting = "SqlConnectionString")] IAsyncCollector<ToDoItem> toDoItems)
        {
            string title = req.Query["title"];
            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            string item = $"{{\"order\": 1, \"title\": \"{title}\", \"url\": \"fakeurl\", \"completed\": true}}";
            ToDoItem toDoItem = JsonConvert.DeserializeObject<ToDoItem>(item);

            // generate a new id for the todo item
            toDoItem.Id = Guid.NewGuid();

            // set Url from env variable ToDoUri
            toDoItem.url = Environment.GetEnvironmentVariable("ToDoUri") + "?id=" + toDoItem.Id.ToString();

            // if completed is not provided, default to false
            if (toDoItem.completed == null)
            {
                toDoItem.completed = false;
            }

            await toDoItems.AddAsync(toDoItem);
            await toDoItems.FlushAsync();
            List<ToDoItem> toDoItemList = new List<ToDoItem> { toDoItem };

            return new OkObjectResult(toDoItemList);
        }
    }
}
