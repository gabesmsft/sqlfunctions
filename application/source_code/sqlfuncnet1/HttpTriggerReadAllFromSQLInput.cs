using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace sqlfuncnet1
{
    public static class HttpTriggerReadAllFromSQLInput
    {
        [FunctionName("HttpTriggerReadAllFromSQLInput")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ReadAllFromSQL")]
            HttpRequest req,
            [Sql("select [Id], [order], [title], [url], [completed] from dbo.ToDo",
                CommandType = System.Data.CommandType.Text,
                ConnectionStringSetting = "SqlConnectionString")]
            IEnumerable<ToDoItem> toDoItems)
        {
            return new OkObjectResult(toDoItems);
        }
    }
}
