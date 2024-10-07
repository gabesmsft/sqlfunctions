using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;

namespace sqlfuncnet1
{
    public static class HttpTriggerReadItemFromSQLInput
    {
        [Function("HttpTriggerReadItemFromSQLInput")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "ReadItemFromSQL")]
            HttpRequest req,
            [SqlInput("select [Id], [order], [title], [url], [completed] from dbo.ToDo where Id = @Id",
                commandType: System.Data.CommandType.Text,
                parameters: "@Id={Query.Id}",
                connectionStringSetting: "SqlConnectionString")]
            IEnumerable<ToDoItem> toDoItem)
        {
            return new OkObjectResult(toDoItem.FirstOrDefault());
        }
    }
}
