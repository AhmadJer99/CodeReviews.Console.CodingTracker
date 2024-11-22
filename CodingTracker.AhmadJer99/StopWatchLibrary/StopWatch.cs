using System.Diagnostics;

namespace StopWatchLibrary;

public class StopWatch
{
    public static DateTime ClockStartTime { get; set; }
    public static DateTime ClockEndTime { get; set; }

    static bool isTimerRuning = default;
    private static readonly Stopwatch watch = new();
    public static void StartClock()
    {
        ShowMenu();
        CancellationTokenSource cancellationToken = new CancellationTokenSource();
        var task = new Task(() => ShowTheWatch(cancellationToken));
        task.Start();
        while (!task.IsCompleted)
        {
            var keyInput = Console.ReadKey(true);
            if (!Console.KeyAvailable)
            {
                if (keyInput.Key == ConsoleKey.Spacebar && !isTimerRuning)
                {
                    ClockStartTime = DateTime.Now;
                    watch.Start();
                    Console.ForegroundColor = ConsoleColor.Green;
                    isTimerRuning = !isTimerRuning;
                }
                else if (keyInput.Key == ConsoleKey.Escape)
                {
                    cancellationToken.Cancel();
                    ClockEndTime = DateTime.Now;
                    ShowMenu();
                    Task.Delay(1000).Wait();
                    break;
                }
                Task.Delay(35).Wait();
            }
        }
    }
    private static void ShowMenu()
    {
        Console.Clear();
        Console.WriteLine("[SPACE] to start the timer");
        Console.WriteLine("[ESC] to stop the timer and save the session");
    }

    static void ShowTheWatch(CancellationTokenSource _cancellationToken)
    {

        int minuteInOneHour = 60;
        int secondInOneMinute = 60;
        int milisecondInOneSecond = 1000;

        Task.Delay(1).Wait();
        while (!_cancellationToken.IsCancellationRequested)
        {
            if (isTimerRuning)
            {
                if (watch.ElapsedMilliseconds != 0)
                {
                    var minute = (watch.ElapsedMilliseconds / (secondInOneMinute * milisecondInOneSecond)) % minuteInOneHour;
                    var sec = (watch.ElapsedMilliseconds / milisecondInOneSecond) % secondInOneMinute;
                    var miliSec = watch.ElapsedMilliseconds % milisecondInOneSecond;
                    Console.Write("\r{0,2:0#}:{1,2:0#}:{2,-100:0##}", minute, sec, miliSec);
                }
            }
            Task.Delay(1).Wait();
        }

    }
}

