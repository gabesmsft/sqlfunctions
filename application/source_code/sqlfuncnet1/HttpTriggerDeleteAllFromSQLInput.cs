using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace sqlfuncnet1
{
    public static class HttpTriggerDeleteAllFromSQLInput
    {
        [FunctionName("HttpTriggerDeleteAllFromSQLInput")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "DeleteAllFromSQL")]
            HttpRequest req,
                  [Sql("DeleteToDo", CommandType = System.Data.CommandType.StoredProcedure, ConnectionStringSetting = "SqlConnectionString")]
                IEnumerable<ToDoItem> toDoItems)
        {
            return new OkObjectResult(toDoItems);
        }
    }
}
