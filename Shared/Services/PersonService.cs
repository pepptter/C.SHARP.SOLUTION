using Newtonsoft.Json;
using Shared.Interfaces;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Shared.Services;

public class PersonService : IPersonService
{
    private readonly IFileHandler _fileHandler = new FileHandler();
    private List<IPerson> _persons = [];
    public PersonService()
    {
        LoadPersonsFromFileIfExists();
    }
    private readonly string _filePath = @"c:\plugg\textfiles\Addressbook.json";



    public bool AddPersonToList(IPerson person)
    {
        try
        {
            if (!_persons.Any(x => string.Equals(x.Email, person.Email, StringComparison.OrdinalIgnoreCase)))
            {
                _persons.Add(person);
                string json = JsonConvert.SerializeObject(_persons, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All});
                var result = _fileHandler.SaveContentToFile(_filePath, json);
                return result;
            }
            else
            {
               Console.WriteLine("Duplicate email found: " + person.Email);
                return false;
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
                _fileHandler.SaveToFileAfterRemovedPerson(_filePath, _persons);
            }
            return true;

        }
        catch (Exception ex) { Debug.WriteLine("PersonService - RemovePersonByEmail" + ex.Message); }
        return false;
    }

    private void LoadPersonsFromFileIfExists()
    {
        var content = _fileHandler.GetContentFromFile(_filePath);
        if (!string.IsNullOrEmpty(content))
        {
            _persons = JsonConvert.DeserializeObject<List<IPerson>>(content, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })!;
        }
        else
        {
            _persons = new List<IPerson>();
        }
    }
}
