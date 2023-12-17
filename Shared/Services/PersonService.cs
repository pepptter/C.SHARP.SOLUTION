using Newtonsoft.Json;
using Shared.Interfaces;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Shared.Services;

public class PersonService : IPersonService
{
    private readonly IFileHandler _fileHandler = new FileHandler();
    private List<IPerson> _persons = [];
    private readonly string _filePath = @"c:\plugg\textfiles\AddressBook.json";

    public bool AddPersonToList(IPerson person)
    {
        try
        { 
            if(!_persons.Any(x => x.Email == person.Email)) 
            {
                _persons.Add(person);
                string json = JsonConvert.SerializeObject(_persons, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All});
                var result = _fileHandler.SaveContentToFile(_filePath, json);
                return result;
            }
        }
        catch (Exception ex) { Debug.WriteLine("PersonService - AddPersonToList" + ex.Message); }
        return false;
    }


    public IPerson GetPersonByEmail(string email)
    {
        try
        {
            GetPersonsFromList();
            var person = _persons.FirstOrDefault(x => x.Email == email);
            return person ??= null!;
        }
        catch (Exception ex) { Debug.WriteLine("PersonService - GetPersonByEmail" + ex.Message); }
        return null!;
    }

    public IEnumerable<IPerson> GetPersonsFromList()
    {
        try
        {
            var content = _fileHandler.GetContentFromFile(_filePath);
            if (!string.IsNullOrEmpty(content))
            {
                _persons = JsonConvert.DeserializeObject<List<IPerson>>(content, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })!;
                return _persons;
            }
        }
        catch (Exception ex) { Debug.WriteLine("PersonService - GetPersonsFromList" + ex.Message); }
        return null!;
    }

    public bool RemovePersonByEmail(string email)
    {
        try
        {
            GetPersonsFromList();
            var person = _persons.FirstOrDefault(x => x.Email == email);
            if (person != null)
            {
                _persons.Remove(person);
            }
            return true;

        }
        catch (Exception ex) { Debug.WriteLine("PersonService - RemovePersonByEmail" + ex.Message); }
        return false;


    }
}
