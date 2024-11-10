using System.Configuration;
using CodingTracker.Models;
namespace CodingTracker.Controllers;

internal static class GenerateRandomData
{
    public static int RandomRowsNumber { get; set; }
    private readonly static string? dateFormat = ConfigurationManager.AppSettings.Get("CorrectDateFormat");
    private readonly static string? timeFormat = ConfigurationManager.AppSettings.Get("CorrectTimeFormat");
    private readonly static Random gen = new();

    private readonly static string randomRecordQuery =
            @"
                INSERT INTO codingsession 
                (SessionDate,StartTime,EndTime,Duration) VALUES (@SessionDate,@StartTime,@EndTime,@Duration);
                ";

    private static string RandomSessionDay()
    {
        // Defining a range to  for random date to be from a specific start date untill today.
        DateTime sessionDay = new DateTime(2024, 01, 01);
        int range = (DateTime.Today - sessionDay).Days;

        return sessionDay.AddDays(gen.Next(range)).ToString(dateFormat);
    }
    private static DateTime RandomTime()
    {
        TimeSpan start = TimeSpan.FromHours(6);
        TimeSpan end = TimeSpan.FromHours(14);

        int maxMinutes = (int)((end - start).TotalMinutes);
        int minutes = gen.Next(maxMinutes);

        TimeSpan time = start.Add(TimeSpan.FromMinutes(minutes)); // 24 hr format
        DateTime timeOfDay = DateTime.Today.Add(time); // converts to 12hr AM/PM format due to database specifications

        return timeOfDay;
    }

    public static void GenerateData(int randomRows = 100)
    {
        RandomRowsNumber = randomRows;
        for (int i = 0; i < RandomRowsNumber; i++)
        {
            string randomDay = RandomSessionDay();


            DateTime randomStartTime = RandomTime();
            DateTime randomEndTime = RandomTime();
            while (randomEndTime < randomStartTime | randomEndTime == randomStartTime)
            {
                randomStartTime = RandomTime();
                randomEndTime = RandomTime();
            }

            var randomCodingSession = new CodingSession
            {
                SessionDate = RandomSessionDay(),
                StartTime = randomStartTime.ToString(timeFormat),
                EndTime = randomEndTime.ToString(timeFormat),
            };

            DatabaseController databaseController = new DatabaseController();
            DatabaseController.InsertRow(randomCodingSession);
        }
    }
}

