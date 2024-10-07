
# SQL bindings for Azure Functions

[![Deploy To Azure](https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/1-CONTRIBUTION-GUIDE/images/deploytoazure.svg?sanitize=true)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fgabesmsft%2Fsqlfunctions%2Fmaster%2Fdeploy%2Fazuredeploy.json)

This sample Azure Resource Manager template deploys an Azure SQL database and Azure Function App. The Azure Function App contains code that will trigger when an item is added to the SQL table, and also contains input and output bindings to read from and write to the database table.
A storage account for the Function App, an Azure SQL server for the database, and an Application Insights instance for Function App logging will also be deployed. This demo is provided as-is and is not intended for real-life production scenarios.

The Function App contains the following Functions:
- **SQLTriggerFunction**: A SQL trigger Function that will execute when an item is added to database table. This Function does not do anything besides log information about the item that was added to the SQL table. The purpose of this Function is merely to demonstrate SQL triggering.
- **HttpTriggerWriteToSQLOutput**: An HTTP Trigger Function with a route of /api/WriteToSQL that will write an item to the SQL table. The purpose of this Function is to provide a convenient way to add an item to the SQL table and trigger SQLTriggerFunction, and to demonstrate SQL outbound bindings.
- **HttpTriggerReadAllFromSQLInput**: An HTTP Trigger Function with a route of /api/ReadAllFromSQL that will print all items from the SQL table. The purpose of this Function is to provide a quick way to verify that HttpTriggerWriteToSQLOutput wrote to the SQL table, and to demonstrate SQL input bindings.
- **HttpTriggerReadItemFromSQLInput**: An HTTP Trigger Function with a route of /api/ReadItemFromSQL that will print a specified item from the SQL table. The purpose of this Function is to demonstrate customization of the SQL query that is passed into the input binding, and to provide a quick way to verify that HttpTriggerWriteToSQLOutput wrote to the SQL table, and to demonstrate SQL input bindings.
- **HttpTriggerDeleteAllFromSQLInput**: An HTTP Trigger Function with a route of /api/DeleteAllFromSQL that will delete all items from the SQL table. The purpose of this Function is to provide a convenient way to empty the table after you test the other Functions. This approach is not recommended in a real-world scenario.

## Post-deployment preparation

1. Add your IP address to the SQL Server firewall.
2. In the fakedb1 database, run the following set of commands to add the ToDo table and enable change tracking so that triggering is possible.

```
CREATE TABLE dbo.ToDo (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY,
    [order] INT NULL,
    [title] NVARCHAR(200) NOT NULL,
    [url] NVARCHAR(200) NOT NULL,
    [completed] BIT NOT NULL
);

ALTER DATABASE [fakedb1]
SET CHANGE_TRACKING = ON
(CHANGE_RETENTION = 2 DAYS, AUTO_CLEANUP = ON);

ALTER TABLE [dbo].[ToDo]
ENABLE CHANGE_TRACKING;

```

3. In the fakedb1 database, add the following SPROC, which will delete all data from the table whenever executed.
```
-- Not a real-world best-practice

CREATE PROCEDURE [dbo].[DeleteToDo]
AS
    BEGIN
        DELETE FROM dbo.ToDo
    END
GO
```

## Demo

1. Add an item to the table by running the following Function in your browser (replace <YourFunctionAppName> with your Function App name):

```
https://<YourFunctionAppName>.azurewebsites.net/api/WriteToSQL?title=mytitle1
```

Adding the record should trigger SQLTriggerFunction, which we will verify in the logs later.

2. Run the following Function in your browser (replace <YourFunctionAppName> with your Function App name):

```
https://<YourFunctionAppName>.azurewebsites.net/api/ReadAllFromSQL
```

This should return a json response containing information about the added record(s), which will verify that the record(s) was/were added to the SQL table.

3. Run steps 1 a few more times, using different values for the title query parameter (e.g. ?title=mytitle2, etc.). Then run step 2 to verify that the records were added to the SQL table.

4. In Application Insights, verify that the SQLTriggerFunction Function executed. It might take a few minutes for the log to appear. You can use the following query to check for Functions activity.
```
traces
| where message startswith "Execut" or message contains "SQL trigger listener" or message contains "queried from database" or  severityLevel >= 3
| extend InvocationId = customDimensions.InvocationId
| project timestamp, message, severityLevel, operation_Name, InvocationId, customDimensions, sdkVersion
| order by timestamp asc
```

- The operation_Name column contains the Function name. 
- For each time the Function was triggered, there should be one Executing and one Executed log with the same InvocationId. E.g.:
```
Executing 'SQLTriggerFunction' (Reason='New change detected on table '[dbo].[ToDo]' at 2023-02-23T23:19:01.7984777Z.', Id=f01bc3e5-1ee4-4120-a9a7-df9242203716)

Executed 'SQLTriggerFunction' (Succeeded, Id=f01bc3e5-1ee4-4120-a9a7-df9242203716, Duration=1ms)
```


5. Optional: note down the ids of one of the records that were added, and then run the following Function to check the record with the specified id in the SQL table:
https://<YourFunctionAppName>.azurewebsites.net/api/WriteToSQL?id=<replace with record id>

6. Optional: to remove all records from the table and restart with an empty table, run the folowing Function:

```
https://<YourFunctionAppName>.azurewebsites.net/api/DeleteAllFromSQL
```