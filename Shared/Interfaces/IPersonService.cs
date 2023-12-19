using Shared.Models.Responses;

namespace Shared.Interfaces;

public interface IPersonService
{

    /// <summary>
    /// Add a person to the addressbooks list
    /// </summary>
    /// <param name="person">a person of type IPerson</param>
    /// <returns>Returns true if successful, false if it fails or email already exists</returns>
    IServiceResult AddPersonToList(IPerson person);

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
    IServiceResult GetPersonByEmail(string email);

    /// <summary>
    /// Enter a persons email to remove that person from the list
    /// </summary>
    /// <param name="email">The email of the person you want to remove</param>
    /// <returns>Returns true if successfully removed, false if something went wrong or email didn't exist</returns>
    IServiceResult RemovePersonByEmail(string email);

   
}
