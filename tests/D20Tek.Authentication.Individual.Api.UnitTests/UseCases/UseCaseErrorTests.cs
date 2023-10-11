//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Api.UnitTests.Assertions;
using D20Tek.Authentication.Individual.Api.UnitTests.Helpers;
using D20Tek.Authentication.Individual.UseCases.ChangePassword;
using D20Tek.Authentication.Individual.UseCases.ChangeRole;
using D20Tek.Authentication.Individual.UseCases.GetResetToken;
using D20Tek.Authentication.Individual.UseCases.RemoveAccount;
using D20Tek.Authentication.Individual.UseCases.ResetPassword;
using D20Tek.Minimal.Result;
using Microsoft.Extensions.Logging;

namespace D20Tek.Authentication.Individual.Api.UnitTests.UseCases;

[TestClass]
public partial class UseCaseErrorTests
{
    private readonly MockAccountRepository _mockEmptyRepository = new();
    private readonly MockAccountRepository _mockRepository = new(
        AccountCommandFactory.CreateAccount("TestUser"));
    private readonly MockTokenGenerator _mockJwtGenerator = new();
    private Guid _testUserId = Guid.NewGuid();

    [TestMethod]
    public async Task ChangePassword_WithRepositoryFailure_ReturnsError()
    {
        // arrange
        var validator = new ChangePasswordCommandValidator();
        var logger = new Mock<ILogger<ChangePasswordCommandHandler>>().Object;

        var command = new ChangePasswordCommand(_testUserId, "password-1", "password-2");
        var handler = new ChangePasswordCommandHandler(
            _mockRepository,
            _mockJwtGenerator,
            validator,
            logger);

        // act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // assert
        result.ShouldBeFailure(Errors.UserAccount.ChangePasswordForbidden);
    }

    [TestMethod]
    public async Task ChangeRole_WithRepositoryFailure_ReturnsError()
    {
        // arrange
        var validator = new ChangeRoleCommandValidator();
        var logger = new Mock<ILogger<ChangeRoleCommandHandler>>().Object;

        var command = new ChangeRoleCommand("TestUser", "admin");
        var handler = new ChangeRoleCommandHandler(
            _mockRepository,
            validator,
            logger);

        // act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // assert
        result.ShouldBeFailure(Errors.UserAccount.CannotAttachRole);
    }

    [TestMethod]
    public async Task ChangeRoleRemove_WithRepositoryFailure_ReturnsError()
    {
        // arrange
        var ourRepository = new MockAccountRepository(
                AccountCommandFactory.CreateAccount("TestUserWithRoles"),
                new List<string> { "user", "admin" });
        var validator = new ChangeRoleCommandValidator();
        var logger = new Mock<ILogger<ChangeRoleCommandHandler>>().Object;

        var command = new ChangeRoleCommand("TestUser", "admin");
        var handler = new ChangeRoleCommandHandler(
            ourRepository,
            validator,
            logger);

        // act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // assert
        result.ShouldBeFailure(Errors.UserAccount.CannotRemoveRoles);
    }

    [TestMethod]
    public async Task ChangeRole_WithGetByUserNameFailure_ReturnsError()
    {
        // arrange
        var validator = new ChangeRoleCommandValidator();
        var logger = new Mock<ILogger<ChangeRoleCommandHandler>>().Object;

        var command = new ChangeRoleCommand("TestUser", "admin");
        var handler = new ChangeRoleCommandHandler(
            _mockEmptyRepository,
            validator,
            logger);

        // act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // assert
        result.ShouldBeFailure(Errors.UserAccount.NotFound("TestUser"));
    }

    [TestMethod]
    public async Task GetResetToken_WithRepositoryFailure_ReturnsError()
    {
        // arrange
        var validator = new GetResetTokenQueryValidator();
        var logger = new Mock<ILogger<GetResetTokenQueryHandler>>().Object;

        var command = new GetResetTokenQuery("test2@test.com");
        var handler = new GetResetTokenQueryHandler(
            _mockRepository,
            validator,
            logger);

        // act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // assert
        result.ShouldBeFailure(Errors.UserAccount.CannotGenerateResetToken);
    }

    [TestMethod]
    public async Task Remove_WithRepositoryFailure_ReturnsError()
    {
        // arrange
        var logger = new Mock<ILogger<RemoveCommandHandler>>().Object;

        var command = new RemoveCommand(_testUserId);
        var handler = new RemoveCommandHandler(
            _mockRepository,
            logger);

        // act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // assert
        result.ShouldBeFailure(Error.Custom("Test.Error", "Remove test failure.", 2));
    }

    [TestMethod]
    public async Task ResetPassword_WithRepositoryFailure_ReturnsError()
    {
        // arrange
        var validator = new ResetPasswordCommandValidator();
        var logger = new Mock<ILogger<ResetPasswordCommandHandler>>().Object;

        var command = new ResetPasswordCommand("tester@test.com", "test-reset-token", "password-2");
        var handler = new ResetPasswordCommandHandler(
            _mockRepository,
            _mockJwtGenerator,
            validator,
            logger);

        // act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // assert
        result.ShouldBeFailure(Errors.UserAccount.ChangePasswordForbidden);
    }
}
