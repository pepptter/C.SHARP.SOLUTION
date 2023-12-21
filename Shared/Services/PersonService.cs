using Newtonsoft.Json;
using Shared.Enums;
using Shared.Interfaces;
using Shared.Models.Responses;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Shared.Services;

public class PersonService : IPersonService
{
    private readonly IFileHandler _fileHandler = new FileHandler();
    private List<IPerson> _persons = [];
    private readonly string _filePath;
    /// <summary>
    /// Initializes a new instance of the <see cref="PersonService"/> class.
    /// Loads the list persons from a file if the file exists.
    /// </summary>
    public PersonService(string filePath)
    {
        _filePath = filePath;
        LoadPersonsFromFileIfExists(_filePath);
    }



    public IServiceResult AddPersonToList(IPerson person)
    {
        IServiceResult response = new ServiceResult();
        try
        {
            if (!_persons.Any(x => string.Equals(x.Email, person.Email, StringComparison.OrdinalIgnoreCase)))
            {
                _persons.Add(person);
                string json = JsonConvert.SerializeObject(_persons, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All});
                var result = _fileHandler.SaveContentToFile(_filePath, json);
                response.Status = ServiceResultStatus.SUCCESS;                
            }
            else 
            {
                response.Status = ServiceResultStatus.ALREADY_EXISTS;
            }
        }
        catch (Exception ex) 
        { 
            Debug.WriteLine("PersonService - AddPersonToList" + ex.Message);
            response.Status = ServiceResultStatus.FAILED;
            response.Result = ex.Message;
        }
        return response;
    }


    public IServiceResult GetPersonByEmail(string email)
    {
        try
        {
            GetPersonsFromList();
            var person = _persons.FirstOrDefault(x => x.Email == email);
            if (person != null)
            {
                var result = new ServiceResult
                {
                    Status = ServiceResultStatus.SUCCESS,
                    Result = person
                };

                return result;
            }
            else
            {
                return new ServiceResult { Status = ServiceResultStatus.NOT_FOUND };
            }
        }
        catch (Exception ex) 
        { 
            Debug.WriteLine("PersonService - GetPersonByEmail" + ex.Message); 
            return new ServiceResult { Status = ServiceResultStatus.FAILED, Result = ex.Message };
        }
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

    public IServiceResult RemovePersonByEmail(string email)
    {
        IServiceResult response = new ServiceResult();
        try
        {
            GetPersonsFromList();
            var person = _persons.FirstOrDefault(x => x.Email == email);
            if (person != null)
            {
                _persons.Remove(person);
                _fileHandler.SaveToFileAfterRemovedPerson(_filePath, _persons);
                response.Status = Enums.ServiceResultStatus.DELETED;
            }
            else
            {
                response.Status = Enums.ServiceResultStatus.NOT_FOUND;
                response.Result = "Person not found";
            }
        }
        catch (Exception ex)
        { 
            Debug.WriteLine("PersonService - RemovePersonByEmail" + ex.Message);
            response.Status = Enums.ServiceResultStatus.FAILED;
            response.Result = ex.Message;
        }

        return response;
    }

    private void LoadPersonsFromFileIfExists(string filePath)
    {
        var content = _fileHandler.GetContentFromFile(filePath);
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
