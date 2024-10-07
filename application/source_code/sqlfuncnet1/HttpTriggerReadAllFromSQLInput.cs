using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;

namespace sqlfuncnet1
{
    public static class HttpTriggerReadAllFromSQLInput
    {
        [Function("HttpTriggerReadAllFromSQLInput")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "ReadAllFromSQL")]
            HttpRequest req,
            [SqlInput(commandText: "select [Id], [order], [title], [url], [completed] from dbo.ToDo",
                commandType: System.Data.CommandType.Text,
                parameters: "",
                connectionStringSetting: "SqlConnectionString")]
            IEnumerable<ToDoItem> toDoItems)
        {
            return new OkObjectResult(toDoItems);
        }
    }
}
