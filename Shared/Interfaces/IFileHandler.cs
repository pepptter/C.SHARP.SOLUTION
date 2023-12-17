namespace Shared.Interfaces;

internal interface IFileHandler
{

    /// <summary>
    /// Save content to the given filepath
    /// </summary>
    /// <param name="filePath">Filepath with extension (eg. c:\filefolder\file.json)</param>
    /// <param name="content">Content as a string</param>
    /// <returns>Returns true if successfully saved, else false</returns>
    bool SaveContentToFile(string filePath, string content);

    
    /// <summary>
    /// Gets content as string from given filepath
    /// </summary>
    /// <param name="filePath">Filepath with extension (eg. c:\filefolder\file.json)</param>
    /// <returns>Returns file content as string if file exists, else returns null</returns>
    string GetContentFromFile(string filePath);
}
