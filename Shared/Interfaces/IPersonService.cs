using Shared.Models.Responses;

namespace Shared.Interfaces;

/// <summary>
/// Services for managing operations related to persons, such as adding, retrieving, and removing.
/// </summary>
public interface IPersonService
{

    /// <summary>
    /// Adds a person to the list and saves the updated list to a file.
    /// </summary>
    /// <param name="person">The person to be added.</param>
    /// <returns>A service result indicating success or failure.</returns>
    IServiceResult AddPersonToList(IPerson person);

    /// <summary>
    /// Retrieves the list of persons from the file.
    /// </summary>
    /// <returns>The list of persons.</returns>
    IEnumerable<IPerson> GetPersonsFromList();

    /// <summary>
    /// Retrieves a person by email address.
    /// </summary>
    /// <param name="email">The email address of the person to retrieve.</param>
    /// <returns>A service result containing the retrieved person or a status indicating failure.</returns>
    IServiceResult GetPersonByEmail(string email);

    /// <summary>
    /// Removes a person by email address and saves the updated list to the file.
    /// </summary>
    /// <param name="email">The email address of the person to remove.</param>
    /// <returns>A service result indicating success or failure.</returns>
    IServiceResult RemovePersonByEmail(string email);

   
}
