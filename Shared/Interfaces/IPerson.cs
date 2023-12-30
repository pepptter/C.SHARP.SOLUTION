namespace Shared.Interfaces;
/// <summary>
/// The interface for a person
/// ID is created when a person is created
/// FirstName, LastName, Email, PhoneNumber, Address is required
/// </summary>
public interface IPerson
{
    Guid Id { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    string Email { get; set; }
    string PhoneNumber { get; set; }
    string Address { get; set; }

}
