using Shared.Interfaces;
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
                return File.ReadAllText(filePath);
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
    /// <returns>Returns true if successfully saved, else false</returns>
    public bool SaveContentToFile(string filePath, string content)
    {
        try
        {
            using var sw = new StreamWriter(filePath);
            sw.WriteLine(content);
            return true;
        }
        catch (Exception ex) { Debug.WriteLine("FileHandler - SaveContentToFile" + ex.Message); }
        return false;
    }
}
