using System.Configuration;
using System.Globalization;
using Spectre.Console;

namespace CodingTracker.Controllers;

internal class Validation
{
    readonly static string? timeFormat = ConfigurationManager.AppSettings.Get("CorrectTimeFormat");
    internal static DateTime AskValidTimeInput(string messageToConsole)
    {
        string? readResult;
        bool validEntry = false;
        CultureInfo us = new CultureInfo("en-US");
        
        do
        {
            AnsiConsole.MarkupLine($"[yellow]{messageToConsole}in this format (h:mm (AM\\PM) e.g (6:16 AM)) :[/]");
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
    internal static string ConvertDateTimeToString(DateTime timeToConvert)
    {
        return timeToConvert.ToString(timeFormat);
    }
}

