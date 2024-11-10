using Spectre.Console;
using static CodingTracker.MenuEnums;
using static CodingTracker.Controllers.OperationController;
namespace CodingTracker;

internal class UserInterface
{
    internal static void ShowMainMenu()
    {
        bool exitApp = false;

        do
        {
            Console.Clear();

            var mainMenuUserChoice = AnsiConsole.Prompt(
                        new SelectionPrompt<MenuAction>()
                        .Title("[yellow]Choose an operation from the following list:[/]")
                        .AddChoices(Enum.GetValues<MenuAction>()));


            switch (mainMenuUserChoice)
            {
                case MenuAction.NewSession:
                    NewSession();
                    AnsiConsole.Markup("[green]Inserted Successfully!\n[white](Press Any Key To Continue)[/][/]");
                    Console.ReadKey();
                    break;
                case MenuAction.ViewAllSessions:
                    ViewAllSessions();
                    AnsiConsole.MarkupLine("\n\t\t\t\t\t\t[green]Press Any Key To Continue[/]");
                    Console.ReadKey();
                    break;
                case MenuAction.UpdateSession:
                    UpdateSession();
                    break;
                case MenuAction.DeleteSession:
                    DeleteSession();
                    AnsiConsole.Markup("[green]Deleted Successfully![white](Press Any Key To Continue)[/][/]");
                    Console.ReadKey();
                    break;
                case MenuAction.SetWeeklyGoal:
                    SetWeeklyGoal();
                    break;
                case MenuAction.ViewPeriodicReport:
                    FilteredView();
                    AnsiConsole.MarkupLine("\n\t\t\t\t\t\t[green]Press Any Key To Continue[/]");
                    Console.ReadKey();
                    break;
                case MenuAction.CloseApplication:
                    AnsiConsole.MarkupLine("[green]Thanks for using the application\nGoodBye![/]");
                    exitApp = true;
                    break;
            }
        }
        while (!exitApp);
    }
}

