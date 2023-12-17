
namespace AddressBook.ConsoleApp.Services;

public class TryAgainPrompt
{
    public static void TryAgain(string tryWhat, Action methodAgain)
    {
        Console.Write($"WOULD YOU LIKE TO {tryWhat}? Y/N: ");
        string option = Console.ReadLine()!.ToUpper();
        if (option == "Y")
        {
            methodAgain();
        }
        else
        {
            MainMenuService.ShowMainMenu();
        }
    }
}
