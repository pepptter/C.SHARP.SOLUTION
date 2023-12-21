
namespace AddressBook.ConsoleApp.Services;

/// <summary>
/// Service for handling repetitive tasks and displaying option titles in the address book console application.
/// </summary>
public class RepeatsService
{
    /// <summary>
    /// Prompts the user if they want to try a specific action again. Returns to the specified method if chosen else it returns to the main menu.
    /// </summary>
    /// <param name="tryWhat">The action or operation to try again.</param>
    /// <param name="methodAgain">The method to execute again if the user chooses to try again.</param>
    public static void TryAgain(string tryWhat, Action methodAgain)
    {
        Console.Write($"WOULD YOU LIKE TO {tryWhat}? (Y/N): ");
        var option = Console.ReadLine() ?? "";
        if (option.Equals("Y", StringComparison.OrdinalIgnoreCase))
        {
            methodAgain();
        }
        else
        {
            MainMenuService.ShowMainMenu();
        }
    }
    /// <summary>
    /// Displays a title for a specific option in the console.
    /// </summary>
    /// <param name="option">The option for which to display the title.</param>
    public static void OptionTitle(string option)
    {
        Console.Clear();
        Console.WriteLine($"###### {option} ######");
        Console.WriteLine();
    }
}
