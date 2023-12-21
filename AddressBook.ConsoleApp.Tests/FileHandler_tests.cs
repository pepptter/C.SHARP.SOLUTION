using Shared.Enums;
using Shared.Interfaces;
using Shared.Models;
using Shared.Services;

namespace AddressBook.ConsoleApp.Tests;

public class FileHandler_tests
{
    [Fact]
    public void SaveContentToFile_SuccessfullySavesContentToFile_ReturnStatusMessageSuccess()
    {
        // Arrange
        var filePath = @"c:\plugg\textfiles\FileTest.json";
        var fileHandler = new FileHandler();
        var testContent = "text for testcontent";

        // Act
        var result = fileHandler.SaveContentToFile(filePath, testContent);

        // Assert
        Assert.Equal(ServiceResultStatus.SUCCESS, result.Status);
        var fileContent = File.ReadAllText(filePath).Trim();
        Assert.Equal(testContent, fileContent);
        File.Delete(filePath);

    }
}
