using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;

namespace sqlfuncnet1
{
    public static class HttpTriggerDeleteAllFromSQLInput
    {
        [Function("HttpTriggerDeleteAllFromSQLInput")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "DeleteAllFromSQL")]
            HttpRequest req,
                  [SqlInput("DeleteToDo", commandType: System.Data.CommandType.StoredProcedure,  parameters: "", connectionStringSetting: "SqlConnectionString")]
                IEnumerable<ToDoItem> toDoItems)
        {
            return new OkObjectResult(toDoItems);
        }
    }
}

