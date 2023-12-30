using Newtonsoft.Json;
using Shared.Enums;
using Shared.Interfaces;
using Shared.Models.Responses;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Shared.Services;

public class PersonService : IPersonService
{
    private readonly IFileHandler _fileHandler;
    private List<IPerson> _persons = [];
    private readonly string _filePath;

    /// <summary>
    /// Initializes a new instance of the PersonService class.
    /// Loads the list of persons from a file if the file exists.
    /// </summary>
    /// <param name="fileHandler">The file handler service.</param>
    /// <param name="filePath">The file path to the savefile.</param>
    public PersonService(IFileHandler fileHandler, string filePath)
    {
        _fileHandler = fileHandler ?? throw new ArgumentNullException(nameof(fileHandler));
        _filePath = filePath;

        LoadPersonsFromFileIfExists();
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
                _fileHandler.SaveToFileAfterChanges(_filePath, _persons);
                response.Status = ServiceResultStatus.DELETED;
            }
            else
            {
                response.Status = ServiceResultStatus.NOT_FOUND;
                response.Result = "PERSON NOT FOUND";
            }
        }
        catch (Exception ex)
        { 
            Debug.WriteLine("PersonService - RemovePersonByEmail" + ex.Message);
            response.Status = ServiceResultStatus.FAILED;
            response.Result = ex.Message;
        }

        return response;
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
            _persons = [];
        }
    }
    public IServiceResult SortPersonsByInput(int input)
    {
        IServiceResult response = new ServiceResult();
        try
        {
            switch (input)
            {
                case 1:
                    _persons = _persons.OrderBy(x => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.FirstName.ToLower())).ToList();
                    response.Status = ServiceResultStatus.UPDATED;
                    response.Result = "LIST CURRENTLY SORTED BY FIRSTNAME";
                    break;
                case 2:
                    _persons = _persons.OrderBy(x => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.LastName.ToLower())).ToList();
                    response.Status = ServiceResultStatus.UPDATED;
                    response.Result = "LIST CURRENTLY SORTED BY LASTNAME";
                    break;
                case 3:
                    _persons = _persons.OrderBy(x => x.Email.ToLower()).ToList();
                    response.Status = ServiceResultStatus.UPDATED;
                    response.Result = "LIST CURRENTLY SORTED BY EMAIL";
                    break;
                default:
                    response.Status = ServiceResultStatus.FAILED;
                    response.Result = "INVALID INPUT";
                    break;
            }
            _fileHandler.SaveToFileAfterChanges(_filePath, _persons);

        }
        catch (Exception ex)
        {
            Debug.WriteLine("PersonService - SortPersonsByInput" + ex.Message);
            response.Status = ServiceResultStatus.FAILED;
            response.Result = ex.Message;
        }
        return response;
    }
}
