using System.Configuration;
using System.Globalization;
using Spectre.Console;

namespace CodingTracker.Controllers;

internal class Validation
{
    readonly static string? timeFormat = ConfigurationManager.AppSettings.Get("CorrectTimeFormat");
    readonly static string? dateFormat = ConfigurationManager.AppSettings.Get("CorrectDateFormat");
    internal static DateTime AskValidTimeInput(string messageToConsole)
    {
        string? readResult;
        bool validEntry = false;
        CultureInfo us = new("en-US");
        
        do
        {
            AnsiConsole.MarkupLine($"[yellow]{messageToConsole}in this format (h:mm (AM\\PM) e.g. (6:16 AM)) :[/]");
            readResult = Console.ReadLine();
            if (readResult != null)
            {
                if (DateTime.TryParseExact(readResult, timeFormat, us, DateTimeStyles.None, out DateTime cleanTime))
                    return cleanTime;
                else
                {
                    AnsiConsole.MarkupLine($"[red]Error: Invalid Input -Please enter the time in the correct format![/]");
                    continue;
                }
            }

        }
        while (!validEntry);
        return DateTime.Today;
    }
    internal static DateTime AskValidDateInput(string messageToConsole)
    {
        string? readResult;
        bool validEntry = false;
        CultureInfo us = new("en-US");
        
        do
        {
            AnsiConsole.MarkupLine($"[yellow]{messageToConsole}in this format (yyyy-MM-dd e.g. (2024-11-14)) :[/]");
            readResult = Console.ReadLine();
            if (readResult != null)
            {
                if (DateTime.TryParseExact(readResult, dateFormat, us, DateTimeStyles.None, out DateTime cleanDate))
                    return cleanDate;
                else
                {
                    AnsiConsole.MarkupLine($"[red]Error: Invalid Input -Please enter the date in the correct format![/]");
                    continue;
                }
            }

        }
        while (!validEntry);
        return DateTime.Today;
    }
    internal static string ConvertDateToString(DateTime dateToConvert)
    {
        return dateToConvert.ToString(dateFormat);
    }
    internal static string ConvertTimeToString(DateTime timeToConvert)
    {
        return timeToConvert.ToString(timeFormat);
    }
}

