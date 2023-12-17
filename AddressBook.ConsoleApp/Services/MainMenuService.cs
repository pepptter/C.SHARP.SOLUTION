using Shared.Interfaces;
using Shared.Models;
using Shared.Services;

namespace AddressBook.ConsoleApp.Services;

internal class MainMenuService
{
    private static readonly IPersonService _personService = new PersonService();

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
            default:
                Console.WriteLine("INVALID CHOICE, PLEASE ENTER A NUMBER FROM 1 TO 5");
                MainMenuChooser();
                break;
        }
    }
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

        Console.Write("WOULD YOU LIKE TO ENTER ANOTHER CONTACT? Y/N: ");

        string option = Console.ReadLine()!.ToUpper();
        if (option == "Y")
        {
            AddPersonMenu();
        }
        else
        {
            ShowMainMenu();
        }
    }
    public static void RemovePersonMenu() 
    {
    
    }
    public static void ShowContactByEmail()
    {

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
    }
}