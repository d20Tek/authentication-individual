//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.UseCases.GetById;
using D20Tek.Authentication.Individual.UseCases.GetResetToken;
using D20Tek.Authentication.Individual.UseCases.Login;
using D20Tek.Authentication.Individual.UseCases.RefreshToken;

namespace D20Tek.Authentication.Individual.Api.UnitTests.UseCases;

[TestClass]
public class QueryTests
{
    private Guid _testUserId = Guid.NewGuid();

    [TestMethod]
    public void GetByIdQuery_Setters()
    {
        // arrange

        var query = new GetByIdQuery(new Guid());

        // act
        query = query with
        {
            UserId = _testUserId,
        };

        // assert
        query.Should().NotBeNull();
        query.UserId.Should().Be(_testUserId);
    }

    [TestMethod]
    public void GetResetTokenQuery_Setters()
    {
        // arrange
        var email = "tester@test.com";

        var query = new GetResetTokenQuery("");

        // act
        query = query with
        {
            Email = email,
        };

        // assert
        query.Should().NotBeNull();
        query.Email.Should().Be(email);
    }

    [TestMethod]
    public void LoginQuery_Setters()
    {
        // arrange
        var user = "TestUser";
        var password = "password-42";

        var query = new LoginQuery("", "");

        // act
        query = query with
        {
            UserName = user,
            Password = password
        };

        // assert
        query.Should().NotBeNull();
        query.UserName.Should().Be(user);
        query.Password.Should().Be(password);
    }

    [TestMethod]
    public void RefreshTokenQuery_Setters()
    {
        // arrange
        var query = new RefreshTokenQuery(new Guid());

        // act
        query = query with
        {
            UserId = _testUserId
        };

        // assert
        query.Should().NotBeNull();
        query.UserId.Should().Be(_testUserId);
    }
}
