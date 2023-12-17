namespace Shared.Interfaces;

public interface IPersonService
{

    /// <summary>
    /// Add a person to the addressbooks list
    /// </summary>
    /// <param name="person">a person of type IPerson</param>
    /// <returns>Returns true if successful or false if it fails or email already exists</returns>
    bool AddPersonToList(IPerson person);

    /// <summary>
    /// Gets all persons from the list
    /// </summary>
    /// <returns>Returns the list or</returns>

    IEnumerable<IPerson> GetPersonsFromList();

    /// <summary>
    /// Enter a persons email to get that persons full information
    /// </summary>
    /// <param name="email">The email of the person in the list that you want information about/param>
    /// <returns>Returns the persons full information, or an error if the email doesn't exist</returns>
    IPerson GetPersonByEmail(string email);
}
