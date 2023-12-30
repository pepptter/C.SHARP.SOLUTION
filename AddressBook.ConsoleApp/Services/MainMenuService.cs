using Shared.Enums;
using Shared.Interfaces;
using Shared.Models;
using Shared.Services;

namespace AddressBook.ConsoleApp.Services;

/// <summary>
/// Service for managing the main menu and its options in the addressbook console application.
/// </summary>
internal class MainMenuService
{
    private static readonly IPersonService _personService = new PersonService(new FileHandler(), @"c:\plugg\textfiles\Addressbook.json");

    /// <summary>
    /// Shows the main menu options and asks the user to enter an option.
    /// </summary>
    public static void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("-------------------------------");
        Console.WriteLine("  WELCOME TO YOUR ADDRESS BOOK ");
        Console.WriteLine("-------------------------------");
        Console.WriteLine("- WHAT WOULD YOU LIKE TO DO?  -");
        Console.WriteLine("1. ADD A CONTACT");
        Console.WriteLine("2. REMOVE A CONTACT");
        Console.WriteLine("3. SHOW ALL CONTACTS");
        Console.WriteLine("4. SEARCH FOR A CONTACT VIA EMAIL");
        Console.WriteLine("5. EXIT THE PROGRAM");
        Console.Write("ENTER YOUR CHOICE (1-5): ");
        MainMenuChooser();
    }
    /// <summary>
    /// Takes the user-entered option and checks if it's valid. Enters the correct menu option or prompts the user to try again.
    /// </summary>
    public static void MainMenuChooser()
    {
        var parsed = int.TryParse(Console.ReadLine(), out int menuChoice);
        if (!parsed || menuChoice < 1 || menuChoice > 5)
        {
            Console.WriteLine("INVALID CHOICE, PLEASE ENTER A NUMBER FROM 1 TO 5");
            MainMenuChooser();
        }

        switch (menuChoice)
        {
            case 1:
                HandleAddPersonMenu();
                break;
            case 2:
                HandleRemovePersonMenu();
                break;
            case 3:
                ShowFullAddressBook(null!, null!);
                break;
            case 4:
                ShowContactByEmail();
                break;
            case 5:
                ShowExitConfirmationOption();
                break;
        }
    }

    /// <summary>
    /// The main menu's option to add a person to the list.
    /// Asks the user to enter the person's first name, last name, email, phone number, and address.
    /// </summary>
    private static void HandleAddPersonMenu()
    {
        IPerson person = new Person();

        RepeatsService.OptionTitle("ADD A CONTACT");
        Console.Write("ENTER FIRST NAME: ");
        person.FirstName = Console.ReadLine()!;
        Console.Write("ENTER LAST NAME: ");
        person.LastName = Console.ReadLine()!;
        Console.Write("ENTER EMAIL ADDRESS: ");
        person.Email = Console.ReadLine()!;
        Console.Write("ENTER PHONE NUMBER: ");
        person.PhoneNumber = Console.ReadLine()!;
        Console.Write("ENTER ADDRESS: ");
        person.Address = Console.ReadLine()!;

        var result = _personService.AddPersonToList(person);
        switch(result.Status)
        {
            case ServiceResultStatus.SUCCESS:
                Console.WriteLine("CONTACT SUCCESSFULLY ADDED");
                break;
            case ServiceResultStatus.ALREADY_EXISTS:
                Console.WriteLine("Duplicate email found: " + person.Email);
                break;
            case ServiceResultStatus.FAILED:
                Console.WriteLine("FAILED SEE ERROR MESSAGE: " + result.Result.ToString());
                break;

        }

        RepeatsService.TryAgain("ADD ANOTHER CONTACT", HandleAddPersonMenu);
    }

    /// <summary>
    /// The main menu's option to remove a person from the list.
    /// Asks the user if they know the email of the person they want to remove.
    /// If they do, they can enter the email and the person will be removed.
    /// If they dont, they can choose to show the full address book and find the email there.
    /// After finding out they can return to this menu to enter the email.
    /// </summary>
    private static void HandleRemovePersonMenu()
    {
        RepeatsService.OptionTitle("REMOVE A CONTACT");
        Console.Write("DO YOU KNOW THE EMAIL OF THE CONTACT YOU WANT TO REMOVE? (Y/N):  ");
        var option = Console.ReadLine() ?? "";
        if (option.Equals("Y", StringComparison.OrdinalIgnoreCase))
        {
            Console.Write("ENTER CONTACTS EMAIL ADDRESS: ");
            IServiceResult result = _personService.RemovePersonByEmail(Console.ReadLine()!);
            switch(result.Status)
            {
                case ServiceResultStatus.DELETED:
                    Console.WriteLine("CONTACT SUCCESSFULLY REMOVED");
                    RepeatsService.TryAgain("REMOVE ANOTHER CONTACT", HandleRemovePersonMenu);
                    break;
                case ServiceResultStatus.NOT_FOUND:
                    Console.WriteLine("EMAIL ADDRESS NOT FOUND");
                    RepeatsService.TryAgain("TRY AGAIN", HandleRemovePersonMenu);
                    break;
                case ServiceResultStatus.FAILED:
                    Console.WriteLine(result.Result);
                    RepeatsService.TryAgain("TRY AGAIN", HandleRemovePersonMenu);
                    break;
            }
        }
        else
        {
            Console.Clear();
            Console.Write("DO YOU WANT TO SHOW THE FULL ADDRESSBOOK TO FIND OUT THE EMAIL? (Y/N): ");
            string option2 = Console.ReadLine() ?? "";
            if (option2.Equals("Y", StringComparison.OrdinalIgnoreCase))
                ShowFullAddressBook("REMOVE A CONTACT PAGE", HandleRemovePersonMenu);            
            else
                ShowMainMenu();
        }
    }

    /// <summary>
    /// The main menu's option to show full information of a person by entering the person's email.
    /// Asks the user if they know the email of the person they want to show.
    /// If so, they can enter the email and the person's full information will be shown.
    /// If not, they can choose to show the full address book and find the email there.
    /// After finding out they can return to this menu to enter the email.
    /// </summary>
    private static void ShowContactByEmail()
    {
        RepeatsService.OptionTitle("SHOW A CONTACT BY EMAIL");
        Console.Write("DO YOU KNOW THE EMAIL OF THE CONTACT YOU WANT TO KNOW MORE ABOUT? (Y/N):  ");
        var option = Console.ReadLine() ?? "";
        if (option.Equals("Y", StringComparison.OrdinalIgnoreCase))
        {
            Console.Write("ENTER CONTACTS EMAIL ADDRESS: ");
            IServiceResult result = _personService.GetPersonByEmail(Console.ReadLine()!);

            if (result.Status != ServiceResultStatus.ALREADY_EXISTS && result.Result != null)
            {
                if(result.Result is IPerson person) { 
                Console.WriteLine($"FULL INFORMATION OF THE CONTACT WITH THE EMAIL ADDRESS: {person.Email}");
                Console.WriteLine($"NAME: {person.FirstName} {person.LastName}");
                Console.WriteLine($"EMAIL: {person.Email}");
                Console.WriteLine($"PHONENUMBER: {person.PhoneNumber}");
                Console.WriteLine($"ADDRESS: {person.Address}");
                Console.WriteLine();
                RepeatsService.TryAgain("SEARCH FOR ANOTHER CONTACT", ShowContactByEmail);
                }

            }
            else
            {
                Console.WriteLine("SOMETHING WENT WRONG OR EMAIL NOT FOUND, CHECK ERROR");
                RepeatsService.TryAgain("TRY AGAIN", ShowContactByEmail);
            }
        }
        else
        {
            Console.Write("DO YOU WANT TO SHOW THE FULL ADDRESSBOOK TO FIND OUT THE EMAIL? (Y/N): ");
            var option2 = Console.ReadLine() ?? "";
            if (option2.Equals("Y", StringComparison.OrdinalIgnoreCase))
                ShowFullAddressBook("SHOW PERSON BY EMAIL", ShowContactByEmail);
            else
                ShowMainMenu();
        }
    }
    /// <summary>
    /// The main menu's option to show the full address book.
    /// Allows the user to sort the list by first name, last name, or email.
    /// Also optionally allows the user to return to the previous menu if entered from another menu.
    /// </summary>
    private static void ShowFullAddressBook(string returnMethod, Action returnOption)
    {
        RepeatsService.OptionTitle("SHOW FULL ADDRESSBOOK");
        Console.WriteLine("HOW DO YOU WANT TO SORT THE LIST?");
        Console.WriteLine("1. BY FIRST NAME");
        Console.WriteLine("2. BY LAST NAME");
        Console.WriteLine("3. BY EMAIL");
        Console.Write("ENTER YOUR CHOICE (1-3): ");
        int menuInput;

        while (true)
        {
            var parsed = int.TryParse(Console.ReadLine(), out menuInput);
            if (parsed && menuInput >= 1 && menuInput <= 3)
            {
                break;
            }

            Console.WriteLine("INVALID CHOICE, PLEASE ENTER A NUMBER FROM 1 TO 3");
        }


        Console.Clear();
        RepeatsService.OptionTitle("SHOW FULL ADDRESSBOOK");

        var sortResult = _personService.SortPersonsByInput(menuInput);
        if (sortResult.Status != ServiceResultStatus.UPDATED)
        {
            Console.WriteLine("SORTING FAILED. CHECK ERROR.");
        }
        else
        {
            Console.WriteLine(sortResult.Result);
        }

        var persons = _personService.GetPersonsFromList();
        foreach (var person in persons)
        {
            Console.WriteLine($"Name: {person.FirstName} {person.LastName} <{person.Email}>");
        }
        Console.WriteLine();
        if (!string.IsNullOrEmpty(returnMethod))
        {
            Console.Write($"DO YOU WANT TO RETURN TO THE {returnMethod}? (Y/N): ");
            var option = Console.ReadLine() ?? "";
            if (option.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                returnOption();
            }
        }
        Console.WriteLine("PRESS ANY KEY TO RETURN TO THE MAIN MENU");
        Console.ReadKey();
        ShowMainMenu();
    }

    /// <summary>
    /// The main menu's exit option. Asks the user to confirm exiting the application.
    /// If yes, the application exits. If no, the main menu is shown again.
    /// </summary>
    private static void ShowExitConfirmationOption() 
    {
        Console.Clear();
        Console.WriteLine("ARE YOU SURE YOU WANT TO EXIT THE APPLICATION? (Y/N): ");
        var option = Console.ReadLine() ?? "";
        if (option.Equals("Y", StringComparison.OrdinalIgnoreCase))
        {
            Environment.Exit(0);
        }
        else
        {
            ShowMainMenu();
        }
    }
}