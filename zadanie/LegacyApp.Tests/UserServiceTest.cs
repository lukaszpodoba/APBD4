using System;
using JetBrains.Annotations;
using LegacyApp;
using Xunit;

namespace LegacyApp.Tests;

[TestSubject(typeof(UserService))]
public class UserServiceTest
{
    [Fact]
    public void AddUser_Should_Return_False_When_FirstName_Is_Missing()
    {
        //Arragnge
        var userService = new UserService();
        //Act
        var addResult = userService.AddUser("", "Doe", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 1);
        //Assert
        Assert.False(addResult);
    }
    
    [Fact]
    public void AddUser_Should_Return_False_When_Surname_Is_Missing()
    {
        //Arragnge
        var userService = new UserService();
        //Act
        var addResult = userService.AddUser("John", "", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 1);
        //Assert
        Assert.False(addResult);
    }

    [Fact]
    public void AddUser_Should_Return_False_When_Email_Is_Missing_AtSign_Or_Dot()
    {
        //Arragnge
        var userService = new UserService();
        //Act
        var addResult = userService.AddUser("John", "Doe", "johndoegmailcom", DateTime.Parse("1982-03-21"), 1);
        //Assert
        Assert.False(addResult);
    }
    
    [Fact]
    public void AddUser_Should_Return_False_When_Client_Is_Not_VeryImportantClient()
    {
        //Arragnge
        var client = new Client();
        //Act
        client.Type = "NormalClient";
        var result = UserService.IsVeryImportantClient(client);
        //Assert
        Assert.False(result);
    }
    
    [Fact]
    public void AddUser_Should_Return_False_When_Client_Is_Not_ImportantClient()
    {
        //Arragnge
        var client = new Client();
        //Act
        client.Type = "NormalClient";
        var result = UserService.IsImportantClient(client);
        //Assert
        Assert.False(result);
    }
    
    [Fact]
    public void SettingCreditLimit_WhenClientIsVeryImportant_ShouldSetUserCreditLimitToFalse()
    {
        // Arrange
        var client = new Client();
        var user = UserService.CreatingNewUser("John", "Doe", "john.doe@gmail.com", DateTime.Parse("1982-03-21"), client);
        // Act
        client.Type = "VeryImportantClient";
        UserService.SettingCreditLimit(client, user);
        // Assert
        Assert.False(user.HasCreditLimit);
    }
    
    [Fact]
    public void SettingCreditLimit_WhenClientIsImportant_DoubleTheUserCreditLimitValue()
    {
        // Arrange
        var client = new Client();
        var user = UserService.CreatingNewUser("John", "Doe", "john.doe@gmail.com", DateTime.Parse("1982-03-21"), client);
        UserService.SettingCreditLimitOfNormalClient(user);
        var limitBefore = user.CreditLimit;
        // Act
        client.Type = "ImportantClient";
        UserService.SettingCreditLimit(client, user);
        // Assert
        Assert.Equal(limitBefore * 2, user.CreditLimit);
    }
    
    [Fact]
    public void SettingCreditLimit_WhenClientIsNormal_ShouldSetUserCreditLimitToTrue()
    {
        // Arrange
        var client = new Client();
        var user = UserService.CreatingNewUser("John", "Doe", "john.doe@gmail.com", DateTime.Parse("1982-03-21"), client);
        // Act
        client.Type = "NormalClient";
        UserService.SettingCreditLimit(client, user);
        // Assert
        Assert.True(user.HasCreditLimit);
    }

    [Fact]
    public void AddUser_Should_Return_False_When_UserHasNotSufficientLimit()
    {
        //Arrange
        var userService = new UserService();
        //Act
        var addResult = userService.AddUser("John", "Kowalski", "kowalski@wp.pl", DateTime.Parse("1982-03-21"), 1);
        //Assert 
        Assert.False(addResult);
    }

    [Fact]
    public void AddUser_Should_Return_False_When_UserHasLessThan21Years()
    {
        //Arrange
        var userService = new UserService();
        //Act
        var addResult = userService.AddUser("John", "Kowalski", "kowalski@wp.pl", DateTime.Parse("2023-12-21"), 1);
        //Assert
        Assert.False(addResult);
    }
    
    [Fact]
    public void AddUser_Should_Return_True_When_Everything_Is_Achieved()
    {
        //Arrange
        var userService = new UserService();
        //Act
        var addResult = userService.AddUser("John", "Doe", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 1);
        //Assert
        Assert.True(addResult);
    }
    
    
}