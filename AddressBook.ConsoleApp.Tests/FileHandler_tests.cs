using Moq;
using Shared.Enums;
using Shared.Interfaces;
using Shared.Models;
using Shared.Models.Responses;
using Shared.Services;

namespace AddressBook.ConsoleApp.Tests;

public class FileHandler_tests
{
    [Fact]
    public void GetContentFromFile_FileExists_ReturnsFileContent()
    {
        // Arrange
        var testFilePath = @"c:\plugg\textfiles\Test.json";
        var fileContent = "Test file content";
        File.WriteAllText(testFilePath, fileContent);

        var mockFileHandler = new Mock<IFileHandler>();
        mockFileHandler.Setup(x => x.GetContentFromFile(testFilePath)).Returns(fileContent);

        // Act
        var result = mockFileHandler.Object.GetContentFromFile(testFilePath);

        // Assert
        Assert.Equal(fileContent, result);

        File.Delete(testFilePath);
    }
    [Fact]
    public void SaveContentToFile_SuccessfullySavesContentToFile_ReturnStatusMessageSuccessAndChecksIfCorrectContent()
    {
        // Arrange
        var filePath = @"c:\plugg\textfiles\Test.json";
        var fileHandler = new FileHandler();
        var testContent = "Test file content";

        // Act
        var result = fileHandler.SaveContentToFile(filePath, testContent);

        // Assert
        Assert.Equal(ServiceResultStatus.SUCCESS, result.Status);
        var fileContent = File.ReadAllText(filePath).Trim();
        Assert.Equal(testContent, fileContent);

        File.Delete(filePath);

    }
    [Fact]
    public void SaveToFileAfterRemovedPerson_SuccessfullySavesToFile_ReturnStatusMessageSuccess()
    {
        // Arrange
        var testFilePath = @"c:\plugg\textfiles\Test.json";
        var persons = new List<IPerson>
        { new Person
        { 
            Email = "sven@domain.com"
        } 
        };

        var mockFileHandler = new Mock<IFileHandler>();
        mockFileHandler.Setup(x => x.SaveToFileAfterRemovedPerson(It.IsAny<string>(), It.IsAny<List<IPerson>>())).Returns(new ServiceResult { Status = ServiceResultStatus.SUCCESS });

        // Act
        var result = mockFileHandler.Object.SaveToFileAfterRemovedPerson(testFilePath, persons);

        // Assert
        Assert.Equal(ServiceResultStatus.SUCCESS, result.Status);
        File.Delete(testFilePath);
    }
}
