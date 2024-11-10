using Microsoft.Data.Sqlite;
using System.Configuration;
using Dapper;
using CodingTracker.Models;

namespace CodingTracker.Controllers;

internal class DatabaseController
{
    private static string LoadConnectionString(string id = "Default")
    {
        return ConfigurationManager.ConnectionStrings[id].ConnectionString;
    }
    private static SqliteConnection CreateConnection()
    {
        return new SqliteConnection(LoadConnectionString());
    }
    private static void InitDatabase()
    {
        using var connection = CreateConnection();

        connection.Open();

        var checkTableCommand = connection.CreateCommand();
        checkTableCommand.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='codingsession';";
        var tableName = checkTableCommand.ExecuteScalar() as string;

        if (string.IsNullOrEmpty(tableName))
        {
            var createQuery = @"
                CREATE TABLE IF NOT EXISTS 
                codingsession
                   (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    SessionDate TEXT,
                    StartTime TEXT,
                    EndTime TEXT,
                    Duration TEXT
                   );
                ";

            connection.Execute(createQuery);
            GenerateRandomData.GenerateData();

            connection.Close();
        }
    }

    public static void InsertRow(CodingSession codingSession)
    {
        InitDatabase();
        var insertQuery = @"
                           INSERT INTO codingsession
                            (SessionDate,StartTime,EndTime,Duration)
                            Values (@SessionDate,@StartTime,@EndTime,@Duration);
                           ";
        using (var connection = CreateConnection())
        {
            connection.Execute(insertQuery, codingSession);
        }
    }
    public static List<CodingSession> ReadAllRows()
    {
        InitDatabase();
        var readQuery = "SELECT * FROM codingsession";

        using (var connection = CreateConnection())
        {
            return connection.Query<CodingSession>(readQuery).ToList();
        }
    }
    public static List<CodingSession> FilteredRead(string filter = "")
    {
        InitDatabase();
        var filteredReadQuery = "SELECT * FROM codingsession " + filter;

        using (var connection = CreateConnection())
        {
            return connection.Query<CodingSession>(filteredReadQuery).ToList();
        }
    }
    public static void UpdateRow(int chosenRowId, CodingSession updatedCodingSession)
    {
        var updateQuery = @"
                           UPDATE codingsession
                            SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration
                            WHERE Id = @Id;
                           ";
        using (var connection = CreateConnection())
        {
            connection.Execute(updateQuery, updatedCodingSession);
        }
    }
    public static void DeleteRow(int chosenRowId)
    {
        var deleteQuery = "DELETE FROM codingsession where Id = @id";

        using (var connection = CreateConnection())
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", chosenRowId);
            connection.Execute(deleteQuery, parameters);
        }

    }
}

