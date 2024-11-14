using CodingTracker.Models;
using ConsoleTableExt;
using Spectre.Console;
using StopWatchLibrary;
using System.Configuration;
using System.Globalization;

namespace CodingTracker.Controllers;

internal class OperationController
{
    private static int WeeklyGoal { get; set; } = 26;
    private static readonly string? dateFormat = ConfigurationManager.AppSettings.Get("CorrectDateFormat");
    readonly static string? timeFormat = ConfigurationManager.AppSettings.Get("CorrectTimeFormat");

    private enum ReportFilter
    {
        PastWeek,
        PastMonth,
        PastYear
    }
    private enum SortOrder
    {
        Ascending,
        Descending
    }

    private enum RowUpdateOptions
    {
        StartTime,
        EndTime,
        Done
    }

    private enum StartTimeOptions
    {
        StopWatch,
        SpecificTime
    }

    private static CodingSession CodingSessionsToSelectableItems(List<CodingSession> codingSessions)
    {
        var chosenCodingSession = AnsiConsole.Prompt(
        new SelectionPrompt<CodingSession>()
       .Title("[blue]Session's Date\t\t|Start Time\t|\tEnd Time\t|\tDuration (Hrs:mins)[/]")
       .UseConverter(codingSession => $"Session's Date: {codingSession.SessionDate} | Start Time: {codingSession.StartTime} - End Time: {codingSession.EndTime} | Total Duration {codingSession.Duration}")
       .AddChoices(codingSessions));

        return chosenCodingSession;
    }

    private static void ShowReportStats(List<CodingSession> codingSessions, ReportFilter reportPeriod)
    {
        var reportType = reportPeriod switch
        {
            ReportFilter.PastWeek => "Last Week",
            ReportFilter.PastMonth => "Last Month",
            ReportFilter.PastYear => "Last Year",
            _ => throw new NotImplementedException(),
        };

        var count = 0;
        int minsCoded = 0;
        int hoursCoded = 0;

        foreach (var codingSession in codingSessions)
        {
            count++;
            minsCoded += DateTime.Parse(codingSession.Duration).Minute;
            if (minsCoded > 59)
            {
                minsCoded = 0;
                hoursCoded++;
            }
            hoursCoded += DateTime.Parse(codingSession.Duration).Hour;
        }

        var avgHoursPerPeriod = (float)hoursCoded / count;
        var avgMinsPerPeriod = (float)minsCoded / count;
        AnsiConsole.MarkupLine("You had a total of [green]{0,2:0#}:{1,2:0#}(Hours:Minutes)[/] " + reportType, hoursCoded, minsCoded);
        AnsiConsole.MarkupLine("You had a daily average of [green]{0,2:0#}:{1,2:0#}(Hours:Minutes)[/] " + reportType, avgHoursPerPeriod, avgMinsPerPeriod);
        if (reportPeriod == ReportFilter.PastWeek)
        {
            string weeklyGoalMessage = $"Weekly Goal: [lightgoldenrod2_1]{WeeklyGoal} Hours[/] =>";
            weeklyGoalMessage += hoursCoded >= WeeklyGoal ? "[green] Goal Achieved[/]" : $"[red] You're off by {WeeklyGoal - hoursCoded} Hours[/]";
            AnsiConsole.MarkupLine(weeklyGoalMessage);
        }


    }

    internal static void SetWeeklyGoal()
    {
        WeeklyGoal = AnsiConsole.Ask<int>("[yellow]Set Your Coding Hours Weekly goal: [/]\n");
        AnsiConsole.Markup($"[green]A weekly goal of [lightgoldenrod2_1]{WeeklyGoal} hours[/] has been set successfully![/]\n");
        AnsiConsole.Markup($"[green]You would need to code [lightgoldenrod2_1]{((float)WeeklyGoal / 7):0.0} hours[/] a day to achieve this goal![/]\n");

        AnsiConsole.Markup($"[white](Press Any Key To Continue)[/]");
        Console.ReadKey();
    }
    internal static void NewSession()
    {
        string startTime = "";
        string endTime = "";
        string sessionDate = "";

        var userTimeEntryChoice = AnsiConsole.Prompt(
            new SelectionPrompt<StartTimeOptions>()
            .Title("[yellow]Do you want to start the session manually by entering start and end date or you can use a stopwatch[/]")
            .AddChoices(Enum.GetValues<StartTimeOptions>()));

        switch (userTimeEntryChoice)
        {
            case StartTimeOptions.StopWatch:
                Console.WriteLine("Unsupported Yet");
                StopWatch.StartClock();

                startTime = Validation.ConvertTimeToString(StopWatch.ClockStartTime);
                endTime = Validation.ConvertTimeToString(StopWatch.ClockEndTime);
                break;

            case StartTimeOptions.SpecificTime:
                var start = Validation.AskValidTimeInput("Enter start time ");
                var end = Validation.AskValidTimeInput("Enter end time ");
                do
                {
                    if (end < start)
                    {
                        AnsiConsole.MarkupLine($"[red]Error: Invalid Input -End time cant be before starting time!!![/]");
                        end = Validation.AskValidTimeInput("Enter end time ");
                    }
                }
                while (end < start);

                var date = Validation.AskValidDateInput("Enter session's date ");

                sessionDate = Validation.ConvertDateToString(date);
                startTime = Validation.ConvertTimeToString(start);
                endTime = Validation.ConvertTimeToString(end);
                break;
        }

        var newCodingSession = new CodingSession
        {
            SessionDate = sessionDate,
            StartTime = startTime,
            EndTime = endTime
        };

        DatabaseController.InsertRow(newCodingSession);

    }

    internal static void ViewAllSessions()
    {

        var codingSessions = DatabaseController.ReadAllRows(); // parse the rows into a list of coding sessions , each element is  a row in the table.

        var columnNames = new List<string>() { "Id", "Session's Date", "Start Time", "End Time", "Duration(Hrs:Mins)" };
        TableVisualisationEngine.ViewAsTable(codingSessions, TableAligntment.Center, columnNames);
        // Pass the list of sessions into a table view engine , and show the results in a user friendly table.
    }
    internal static void FilteredView()
    {
        string filter = "";

        var filterType = AnsiConsole.Prompt(
            new SelectionPrompt<ReportFilter>()
            .Title("[yellow]Choose a filter to apply[/]")
            .AddChoices(Enum.GetValues<ReportFilter>()));

        var orderType = AnsiConsole.Prompt(
            new SelectionPrompt<SortOrder>()
            .Title("[yellow]Choose the sort to apply on duration[/]")
            .AddChoices(Enum.GetValues<SortOrder>()));

        string dateTimeToday = DateTime.Now.ToString(dateFormat);
        switch (filterType)
        {
            case ReportFilter.PastWeek:
                string pastWeek = (DateTime.Now.AddDays(-7)).ToString(dateFormat);

                filter += $"WHERE SessionDate BETWEEN  '{pastWeek}'  AND '{dateTimeToday}' ";
                break;
            case ReportFilter.PastMonth:
                string pastMonth = (DateTime.Now.AddDays(-30)).ToString(dateFormat);

                filter += $"WHERE SessionDate BETWEEN  '{pastMonth}'  AND '{dateTimeToday}' ";
                break;
            case ReportFilter.PastYear:
                string pastYear = (DateTime.Now.AddDays(-365)).ToString(dateFormat);

                filter += $"WHERE SessionDate BETWEEN  '{pastYear}'  AND '{dateTimeToday}' ";
                break;
        }
        if (orderType == SortOrder.Ascending)
            filter += "ORDER BY Duration ASC;";
        else
            filter += "ORDER BY Duration DESC;";

        var codingSessions = DatabaseController.FilteredRead(filter); // parse the rows into a list of coding sessions , each element is  a row in the table.

        var columnNames = new List<string>() { "Id", "Session's Date", "Start Time", "End Time", "Duration(Hrs:Mins)" };
        TableVisualisationEngine.ViewAsTable(codingSessions, TableAligntment.Center, columnNames);

        ShowReportStats(codingSessions, filterType);
    }

    internal static void UpdateSession()
    {
        CultureInfo us = new CultureInfo("en-US");
        bool doneUpdating = false;
        // list all sessions by a select query annd make them a selectable list to be able to let the user update the session he chooses
        do
        {
            var codingSessions = DatabaseController.ReadAllRows();
            var chosenCodingSession = CodingSessionsToSelectableItems(codingSessions);

            var updateProperty = AnsiConsole.Prompt(
                new SelectionPrompt<RowUpdateOptions>()
                .Title("[yellow]Choose what property you want to edit (or select done if you're finished updating): [/]")
                .AddChoices(Enum.GetValues<RowUpdateOptions>()));

            int id = chosenCodingSession.Id;

            switch (updateProperty)
            {
                case RowUpdateOptions.StartTime:

                    var newStart = Validation.AskValidTimeInput("Enter new start time ");
                    var currentEndTime = DateTime.ParseExact(chosenCodingSession.EndTime, timeFormat, us);
                    if (newStart > currentEndTime)
                    {
                        AnsiConsole.MarkupLine($"[red]Update Failed: Invalid Input -New start time cant be after current end time!!![/]\n(Press any key to confirm this error and continue)");
                        Console.ReadKey();
                        break;
                    }

                    var startTime = Validation.ConvertTimeToString(newStart);
                    var updatedStartTimeSession = new CodingSession
                    {
                        Id = id,
                        StartTime = startTime,
                        EndTime = chosenCodingSession.EndTime,
                    };
                    DatabaseController.UpdateRow(id, updatedStartTimeSession);

                    AnsiConsole.Markup("[green]Session Updated Successfully!\n[white](Press Any Key To Continue)[/][/]");
                    Console.ReadKey();
                    break;
                case RowUpdateOptions.EndTime:
                    var newEnd = Validation.AskValidTimeInput("Enter new end time ");
                    var currentStartTime = DateTime.ParseExact(chosenCodingSession.StartTime, timeFormat, us);
                    if (newEnd < currentStartTime)
                    {
                        AnsiConsole.MarkupLine($"[red]Update Failed: Invalid Input -New End time cant be before current Start time!!![/]\n(Press any key to confirm this error and continue)");
                        Console.ReadKey();
                        break;
                    }

                    var endTime = Validation.ConvertTimeToString(newEnd);
                    var updatedEndTimeSession = new CodingSession
                    {
                        Id = id,
                        StartTime = chosenCodingSession.StartTime,
                        EndTime = endTime,
                    };
                    DatabaseController.UpdateRow(id, updatedEndTimeSession);

                    AnsiConsole.Markup("[green]Session Updated Successfully!\n[white](Press Any Key To Continue)[/][/]");
                    Console.ReadKey();
                    break;
                case RowUpdateOptions.Done:
                    doneUpdating = true;
                    break;
            }
        }
        while (!doneUpdating);
    }
    internal static void DeleteSession()
    {
        var codingSessions = DatabaseController.ReadAllRows();
        var chosenCodingSession = CodingSessionsToSelectableItems(codingSessions);
        // list all sessions by a select query annd make them a selectable list to be able to delete a session
        int chosenRowId = chosenCodingSession.Id;
        DatabaseController.DeleteRow(chosenRowId);
    }

}

