using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;

namespace sqlfuncnet1
{
    public static class HttpTriggerWriteToSQLOutput
    {
        [Function("HttpTriggerWriteToSQLOutput")]

        [SqlOutput("dbo.ToDo", connectionStringSetting: "SqlConnectionString")]
        public static async Task<ToDoItem> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "WriteToSQL")] HttpRequest req)
        {
            string title = req.Query["title"];
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

            return toDoItem;
        }
    }
}
