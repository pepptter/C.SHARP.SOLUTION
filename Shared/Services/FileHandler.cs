using Newtonsoft.Json;
using Shared.Interfaces;
using Shared.Models;
using Shared.Models.Responses;
using System.Diagnostics;

namespace Shared.Services;

internal class FileHandler : IFileHandler
{
    /// <summary>
    /// Gets the whole list from given filepath
    /// </summary>
    /// <param name="filePath">Filepath with extension (eg. c:\filefolder\file.json)</param>
    /// <returns>The list if it exists</returns>
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
    /// <summary>
    /// Save content to the given filepath
    /// </summary>
    /// <param name="filePath">Filepath with extension (eg. c:\filefolder\file.json)</param>
    /// <param name="content">Content as a string</param>
    /// <returns>Returns true if successfully save, else writes debug message and returns false</returns>
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
    /// <summary>
    /// Save file after a person has been removed from the file
    /// </summary>
    /// <param name="filePath">Filepath with extension (eg. c:\filefolder\file.json)</param>
    /// <param name="persons">The name of the list</param>
    /// <returns>Returns true if successfully save, else writes debug message and returns false</returns>
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
