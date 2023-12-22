using Moq;
using Shared.Enums;
using Shared.Interfaces;
using Shared.Models;
using Shared.Models.Responses;
using Shared.Services;

namespace AddressBook.ConsoleApp.Tests;

public class PersonService_Tests
{
    [Fact]
    public void AddPersonToList_SuccessfullyAddsPerson_ReturnsSuccessStatus()
    {
        // Arrange
        var testFilePath = @"c:\plugg\textfiles\Test.json";
        var mockFileHandler = new Mock<IFileHandler>();
        mockFileHandler.Setup(x => x.SaveContentToFile(It.IsAny<string>(), It.IsAny<string>())).Returns(new ServiceResult { Status = ServiceResultStatus.SUCCESS });
        var personService = new PersonService(mockFileHandler.Object, testFilePath);

        var person = new Person
        {
            FirstName = "Karl",
            LastName = "Karlsson",
            Email = "karl@domain.com",
            PhoneNumber = "123126789",
            Address = "Lillgatan 2"
        };

        // Act
        var result = personService.AddPersonToList(person);

        // Assert
        Assert.Equal(ServiceResultStatus.SUCCESS, result.Status);
        mockFileHandler.Verify(x => x.SaveContentToFile(testFilePath, It.IsAny<string>()), Times.Once);
    }
    [Fact]
    public void AddPersonToList_DuplicateEmail_ReturnsAlreadyExistsStatus()
    {
        // Arrange
        var testFilePath = @"c:\plugg\textfiles\Test.json";
        var mockFileHandler = new Mock<IFileHandler>();
        mockFileHandler.Setup(x => x.SaveContentToFile(It.IsAny<string>(), It.IsAny<string>())).Returns(new ServiceResult { Status = ServiceResultStatus.SUCCESS });
        var personService = new PersonService(mockFileHandler.Object, testFilePath);

        var person1 = new Person
        {
            FirstName = "Sven",
            LastName = "Svensson",
            Email = "sven@domain.com",
            PhoneNumber = "0123456789",
            Address = "Storgatan 2"
        };
        var person2 = new Person
        {
            FirstName = "Sven-Erik",
            LastName = "Svensson",
            Email = "sven@domain.com",
            PhoneNumber = "0223456789",
            Address = "Storgatan 42"
        };
        personService.AddPersonToList(person1);
        // Act
        var result = personService.AddPersonToList(person2);

        // Assert
        Assert.Equal(ServiceResultStatus.ALREADY_EXISTS, result.Status);

    }
    [Fact]
    public void GetPersonByEmail_PersonExists_ReturnsPersonWithSuccessStatus()
    {
        // Arrange
        var testFilePath = @"c:\plugg\textfiles\Test.json";
        var mockFileHandler = new Mock<IFileHandler>();
        mockFileHandler.Setup(x => x.SaveContentToFile(It.IsAny<string>(), It.IsAny<string>())).Returns(new ServiceResult { Status = ServiceResultStatus.SUCCESS });
        var personService = new PersonService(mockFileHandler.Object, testFilePath);

        var person = new Person
        {
            Id = Guid.NewGuid(),
            FirstName = "Sven",
            LastName = "Svensson",
            Email = "sven@domain.com",
            PhoneNumber = "0123456789",
            Address = "Storgatan 2"
        };
        personService.AddPersonToList(person);

        // Act
        var result = personService.GetPersonByEmail("sven@domain.com");

        // Assert
        Assert.Equal(ServiceResultStatus.SUCCESS, result.Status);
        Assert.IsType<Person>(result.Result);

    }
    [Fact]
    public void RemovePersonByEmail_PersonExists_ReturnsDeletedStatus()
    {
        // Arrange
        var testFilePath = @"c:\plugg\textfiles\Test.json";
        var mockFileHandler = new Mock<IFileHandler>();
        mockFileHandler.Setup(x => x.SaveContentToFile(It.IsAny<string>(), It.IsAny<string>())).Returns(new ServiceResult { Status = ServiceResultStatus.SUCCESS });
        var personService = new PersonService(mockFileHandler.Object, testFilePath);
        var person = new Person
        {
            Id = System.Guid.NewGuid(),
            FirstName = "Sven",
            LastName = "Svensson",
            Email = "sven@domain.com",
            PhoneNumber = "0123456789",
            Address = "Storgatan 2"
        };
        personService.AddPersonToList(person);

        // Act
        var result = personService.RemovePersonByEmail("sven@domain.com");

        // Assert
        Assert.Equal(ServiceResultStatus.DELETED, result.Status);
        Assert.Null(personService.GetPersonByEmail("sven@domain.com").Result);
    }
    [Fact]
    public void RemovePersonByEmail_PersonNotFound_ReturnsNotFoundStatus()
    {
        // Arrange
        var testFilePath = @"c:\plugg\textfiles\Test.json";
        var mockFileHandler = new Mock<IFileHandler>();
        mockFileHandler.Setup(x => x.SaveContentToFile(It.IsAny<string>(), It.IsAny<string>())).Returns(new ServiceResult { Status = ServiceResultStatus.SUCCESS });
        var personService = new PersonService(mockFileHandler.Object, testFilePath);

        // Act
        var result = personService.RemovePersonByEmail("testmail@domain.com");

        // Assert
        Assert.Equal(ServiceResultStatus.NOT_FOUND, result.Status);

    }
    [Fact]
    public void GetPersonByEmail_PersonNotFound_ReturnsNotFoundStatus()
    {
        // Arrange
        var testFilePath = @"c:\plugg\textfiles\Test.json";
        var mockFileHandler = new Mock<IFileHandler>();
        mockFileHandler.Setup(x => x.SaveContentToFile(It.IsAny<string>(), It.IsAny<string>())).Returns(new ServiceResult { Status = ServiceResultStatus.SUCCESS });
        var personService = new PersonService(mockFileHandler.Object, testFilePath);

        // Act
        var result = personService.GetPersonByEmail("testmail@domain.com");

        // Assert
        Assert.Equal(ServiceResultStatus.NOT_FOUND, result.Status);
        Assert.Null(result.Result);
    }

}
