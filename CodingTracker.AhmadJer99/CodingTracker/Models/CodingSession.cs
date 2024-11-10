using System.Configuration;
using System.Globalization;

namespace CodingTracker.Models;

public class CodingSession
{


    public int Id { get; set; }
    public string SessionDate { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string Duration
    {
        get
        {
            var correctTimeFormat = ConfigurationManager.AppSettings.Get("CorrectTimeFormat");
            var currentCulture = ConfigurationManager.AppSettings.Get("CurrentSupportedCutlure");

            var currCult = new CultureInfo(currentCulture);

            DateTime startTime = DateTime.ParseExact(StartTime, correctTimeFormat, currCult);
            DateTime endTime = DateTime.ParseExact(EndTime, correctTimeFormat, currCult);

            TimeSpan timeSpan = endTime - startTime;

            return timeSpan.ToString(@"hh\:mm");
        }
    }
}

