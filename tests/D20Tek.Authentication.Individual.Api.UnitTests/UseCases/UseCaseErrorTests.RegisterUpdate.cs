//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Api.UnitTests.Assertions;
using D20Tek.Authentication.Individual.Api.UnitTests.Helpers;

using D20Tek.Authentication.Individual.UseCases.Register;
using D20Tek.Authentication.Individual.UseCases.UpdateAccount;
using D20Tek.Minimal.Result;
using Microsoft.Extensions.Logging;

namespace D20Tek.Authentication.Individual.Api.UnitTests.UseCases;

public partial class UseCaseErrorTests
{
    [TestMethod]
    public async Task Register_WithRepositoryFailure_ReturnsError()
    {
        // arrange
        var validator = new RegisterCommandValidator();
        var logger = new Mock<ILogger<RegisterCommandHandler>>().Object;

        var command = AccountCommandFactory.CreateRegisterCommand("NewTestUser");
        var handler = new RegisterCommandHandler(
            _mockEmptyRepository,
            _mockJwtGenerator,
            validator,
            logger);

        // act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // assert
        result.ShouldBeFailure(Error.Custom("Test.Error", "Test failure.", 2));
    }

    [TestMethod]
    public async Task RegisterWithRole_WithRepositoryFailure_ReturnsError()
    {
        // arrange
        var localRepo = new MockAccountRepository(allowCreate: true);
        var validator = new RegisterCommandValidator();
        var logger = new Mock<ILogger<RegisterCommandHandler>>().Object;

        var command = AccountCommandFactory.CreateRegisterCommand("NewTestUser");
        var handler = new RegisterCommandHandler(localRepo, _mockJwtGenerator, validator, logger);

        // act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // assert
        result.ShouldBeFailure(Errors.UserAccount.CannotAttachRole);
    }

    [TestMethod]
    public async Task Update_WithRepositoryFailure_ReturnsError()
    {
        // arrange
        var validator = new UpdateCommandValidator();
        var logger = new Mock<ILogger<UpdateCommandHandler>>().Object;

        var command = CreateUpdateCommand();
        var handler = new UpdateCommandHandler(_mockRepository, validator, logger);

        // act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // assert
        result.ShouldBeFailure(Error.Custom("Test.Error", "Update test failure.", 2));
    }

    [TestMethod]
    public async Task UpdateDuplicateUserName_WithRepositoryFailure_ReturnsError()
    {
        // arrange
        var localRepo = new MockAccountRepository(
            AccountCommandFactory.CreateAccount("TestUser"),
            getDuplicateUserName: true);
        var validator = new UpdateCommandValidator();
        var logger = new Mock<ILogger<UpdateCommandHandler>>().Object;

        var command = CreateUpdateCommand();
        var handler = new UpdateCommandHandler(localRepo, validator, logger);

        // act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // assert
        result.ShouldBeFailure(Errors.UserAccount.UserNameAlreadyInUse);
    }

    [TestMethod]
    public async Task UpdateDuplicateEmail_WithRepositoryFailure_ReturnsError()
    {
        // arrange
        var localRepo = new MockAccountRepository(
            AccountCommandFactory.CreateAccount("TestUser", email: "tester@test.com"),
            getDuplicateEmail: true);
        var validator = new UpdateCommandValidator();
        var logger = new Mock<ILogger<UpdateCommandHandler>>().Object;

        var command = CreateUpdateCommand();
        var handler = new UpdateCommandHandler(localRepo, validator, logger);

        // act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // assert
        result.ShouldBeFailure(Errors.UserAccount.EmailAlreadyInUse);
    }

    private UpdateCommand CreateUpdateCommand()
    {
        return new UpdateCommand(
            _testUserId,
            "NewTestUser",
            "foo",
            "bar",
            "foo@bar.com",
            null);
    }
}
