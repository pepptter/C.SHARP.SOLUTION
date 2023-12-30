using Shared.Models.Responses;

namespace Shared.Interfaces;

/// <summary>
/// Services for handling file operations, such as reading, updating and saving content.
/// </summary>
public interface IFileHandler
{

    /// <summary>
    /// Gets the entire content from the specified file path.
    /// </summary>
    /// <param name="filePath">The file path with extension (e.g., c:\filefolder\file.json).</param>
    /// <returns>The content as a string if the file exists; otherwise, returns null.</returns>
    string GetContentFromFile(string filePath);

    /// <summary>
    /// Saves content to the specified file path.
    /// </summary>
    /// <param name="filePath">The file path with extension (e.g., c:\filefolder\file.json).</param>
    /// <param name="content">The content to be saved as a string.</param>
    /// <returns>A service result indicating success or failure.</returns>
    IServiceResult SaveContentToFile(string filePath, string content);

    /// <summary>
    /// Saves the file after changes have been made to the list of persons.
    /// </summary>
    /// <param name="filePath">The file path with extension (e.g., c:\filefolder\file.json).</param>
    /// <param name="persons">The list of persons to be turned into a json-object and saved.</param>
    /// <returns>A service result indicating updated or failure.</returns>
    IServiceResult SaveToFileAfterChanges(string filePath, List<IPerson> persons);

}
