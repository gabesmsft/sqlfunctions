using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Sql;

namespace sqlfuncnet1
{
    public static class SQLTriggerFunction1
    {
        [FunctionName("SQLTriggerFunction")]
        public static void Run(
    [SqlTrigger("[dbo].[ToDo]", ConnectionStringSetting = "SqlConnectionString")]
            IReadOnlyList<SqlChange<ToDoItem>> changes,
    ILogger logger)
        {
            foreach (SqlChange<ToDoItem> change in changes)
            {
                ToDoItem toDoItem = change.Item;
                logger.LogInformation($"Change operation: {change.Operation}");
                logger.LogInformation($"Id: {toDoItem.Id}, Title: {toDoItem.title}, Url: {toDoItem.url}, Completed: {toDoItem.completed}");
            }
        }
    }
}
