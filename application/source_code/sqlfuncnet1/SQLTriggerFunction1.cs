using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;

namespace sqlfuncnet1
{
    public static class SQLTriggerFunction1
    {
        [Function("SQLTriggerFunction")]
        public static void Run(
        [SqlTrigger("[dbo].[ToDo]", connectionStringSetting: "SqlConnectionString")]
            IReadOnlyList<SqlChange<ToDoItem>> changes,
            FunctionContext context)
        {
            foreach (SqlChange<ToDoItem> change in changes)
            {
                var logger = context.GetLogger("ToDoTrigger");
                ToDoItem toDoItem = change.Item;
                logger.LogInformation($"Change operation: {change.Operation}");
                logger.LogInformation($"Id: {toDoItem.Id}, Title: {toDoItem.title}, Url: {toDoItem.url}, Completed: {toDoItem.completed}");
            }
        }
    }
}
