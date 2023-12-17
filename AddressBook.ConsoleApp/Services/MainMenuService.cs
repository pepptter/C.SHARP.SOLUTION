using Shared.Interfaces;
using Shared.Models;
using Shared.Services;

namespace AddressBook.ConsoleApp.Services;

internal class MainMenuService
{
    private static readonly IPersonService _personService = new PersonService();

    //shows the main menu options and asks the user to enter an option
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
    //takes the user entered option and checks if it's valid, if so it enters the correct menu option
    //else the option is not valid, it asks the user to try again
    public static void MainMenuChooser()
    {
        var parsed = int.TryParse(Console.ReadLine(), out int menuChoice);
        if (!parsed)
        {
            Console.WriteLine("INVALID CHOICE, PLEASE ENTER A NUMBER FROM 1 TO 5");
            MainMenuChooser();
        }

        switch (menuChoice)
        {
            case 1:
                AddPersonMenu();
                break;
            case 2:
                RemovePersonMenu();
                break;
            case 3:
                ShowFullAddressBook();
                break;
            case 4:
                ShowContactByEmail();
                break;
            case 5:
                Environment.Exit(0);
                break;
        }
    }

    //The main menus option to add a person to the list
    //asks the user for inputs and adds the person to the list through the AddPersonToList method
    //after the person has been added it asks the user if it wants to add another one through the TryAgain method in TryAgainPrompt.cs
    public static void AddPersonMenu()
    {
        IPerson person = new Person();

        Console.Clear();
        Console.WriteLine($"{"",-5} ADD A CONTACT");
        Console.WriteLine("--------------------------------");
        Console.Write("ENTER FIRST NAME: ");
        person.FirstName = Console.ReadLine()!;
        Console.Write("ENTER LAST NAME: ");
        person.LastName = Console.ReadLine()!;
        Console.WriteLine("ENTER EMAIL ADDRESS: ");
        person.Email = Console.ReadLine()!;
        Console.WriteLine("ENTER PHONE NUMBER: ");
        person.PhoneNumber = Console.ReadLine()!;
        Console.WriteLine("ENTER ADDRESS: ");
        person.Address = Console.ReadLine()!;

        _personService.AddPersonToList(person);

        TryAgainPrompt.TryAgain("ADD ANOTHER PERSON", AddPersonMenu);
    }
    //The main menus option to remove a person from the list
    //asks the user if it knows the persons email, if the user answers yes it asks for the email and removes the person if the email exists
    //if the email does not exists the person is told so and gets asked if it wants to try again.
    //if the user answers that it doesn't know the persons email, it is asked if it wants to see the full addressbook to find out the email of the person
    //after the person has been removed it asks the user if it wants to remove another one through the TryAgain method in TryAgainPrompt.cs
    public static void RemovePersonMenu()
    {
        Console.Clear();
        Console.WriteLine("DO YOU KNOW THE EMAIL OF THE PERSON YOU WANT TO REMOVE? Y/N");
        string option = Console.ReadLine()!.ToUpper();
        if (option == "Y")
        {
            Console.Write("ENTER PERSONS EMAIL ADDRESS: ");
            bool result = _personService.RemovePersonByEmail(Console.ReadLine()!);
            if (result == true)
            {
                Console.WriteLine("PERSON SUCCESSFULLY REMOVED");
                TryAgainPrompt.TryAgain("REMOVE ANOTHER PERSON", AddPersonMenu);
            }
            else
            {
                Console.WriteLine("SOMETHING WENT WRONG OR EMAIL NOT FOUND, CHECK ERROR");
                TryAgainPrompt.TryAgain("TRY AGAIN", AddPersonMenu);
            }
        }
        else
        {
            Console.Clear();
            Console.WriteLine("DO YOU WANT TO SHOW THE FULL ADDRESSBOOK TO FIND OUT THE EMAIL? Y/N: ");
            string option2 = Console.ReadLine()!.ToUpper();
            if (option2 == "Y")
                ShowFullAddressBook();            
            else
                ShowMainMenu();
        }
    }
    //The main menus option to show full information of a person from entering the persons email
    //asks the user for inputs and adds the person to the list through the AddPersonToList method
    //after the person has been added it asks the user if it wants to add another one through the TryAgain method in TryAgainPrompt.cs
    public static void ShowContactByEmail()
    {
        Console.WriteLine("DO YOU KNOW THE EMAIL OF THE PERSON YOU WANT TO KNOW MORE ABOUT? Y/N");
        string option = Console.ReadLine()!.ToUpper();
        if (option == "Y")
        {
            Console.Write("ENTER PERSONS EMAIL ADDRESS: ");
            var searchEmail = Console.ReadLine();
            var fullPerson = _personService.GetPersonByEmail(searchEmail!);
            if (fullPerson != null)
            {
                Console.WriteLine($"FULL INFORMATION OF THE PERSON WITH THE EMAIL ADDRESS: {fullPerson.Email}");
                Console.WriteLine($"NAME: {fullPerson.FirstName} {fullPerson.LastName}");
                Console.WriteLine($"PHONENUMBER: {fullPerson.PhoneNumber}");
                Console.WriteLine($"ADDRESS: {fullPerson.Address}");
                Console.WriteLine();
                TryAgainPrompt.TryAgain("SEARCH FOR ANOTHER PERSON", ShowContactByEmail);

            }
            else
            {
                Console.WriteLine("SOMETHING WENT WRONG OR EMAIL NOT FOUND, CHECK ERROR");
                TryAgainPrompt.TryAgain("TRY AGAIN", ShowContactByEmail);
            }
        }
        else
        {
            Console.WriteLine("DO YOU WANT TO SHOW THE FULL ADDRESSBOOK TO FIND OUT THE EMAIL? Y/N: ");
            string option2 = Console.ReadLine()!.ToUpper();
            if (option2 == "Y")
                ShowFullAddressBook();
            else
                ShowMainMenu();
        }
    }
    public static void ShowFullAddressBook()
    {
        Console.Clear();
        var persons = _personService.GetPersonsFromList();
        foreach (var person in persons)
        {
            Console.WriteLine($"Name: {person.FirstName} {person.LastName}");
            Console.WriteLine($"Email: {person.Email}");
            Console.WriteLine($"Phone: {person.PhoneNumber}");
            Console.WriteLine($"Address: {person.Address}");
            Console.WriteLine();

        }
        Console.WriteLine("Press any key to return to the main menu: ");
        Console.ReadKey();
        ShowMainMenu();
    }
}