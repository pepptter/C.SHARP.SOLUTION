using Newtonsoft.Json;
using Shared.Interfaces;
using Shared.Models;
using Shared.Models.Responses;
using System.Diagnostics;

namespace Shared.Services;


internal class FileHandler : IFileHandler
{

    public string GetContentFromFile(string filePath)
    {
        try
        {
            if(File.Exists(filePath))
            {
                using (var sr = new StreamReader(filePath)) 
                {
                    return sr.ReadToEnd();
                }                
            }
        }
        catch (Exception ex) { Debug.WriteLine("FileHandler - GetContentFromFile" + ex.Message); }
        return null!;
    }

    public IServiceResult SaveContentToFile(string filePath, string content)
    {
        IServiceResult response = new ServiceResult();

        try
        {
            using var sw = new StreamWriter(filePath);
            sw.WriteLine(content);
            response.Status = Enums.ServiceResultStatus.SUCCESS;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("FileHandler - SaveContentToFile" + ex.Message);
            response.Status = Enums.ServiceResultStatus.FAILED;
        }
        return response;
    }

    public IServiceResult SaveToFileAfterRemovedPerson(string filePath, List<IPerson> persons)
    {
        IServiceResult response = new ServiceResult();
        try
        {
            string json = JsonConvert.SerializeObject(persons, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            using var sw = new StreamWriter(filePath);
            sw.WriteLine(json);
            response.Status = Enums.ServiceResultStatus.SUCCESS;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("FileHandler - SaveToFileAfterRemovedPerson Exception: " + ex.Message);
            response.Status = Enums.ServiceResultStatus.FAILED;
        }
        return response;
    }
}
