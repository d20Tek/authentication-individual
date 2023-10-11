//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace D20Tek.Authentication.Individual.Api.UnitTests.Infrastructure;

[TestClass]
public class OperationManagerTests
{
    [TestMethod]
    public async Task OperationAsync_WithSuccess_DoesNotLog()
    {
        // arrange
        var logger = new Mock<ILogger>();
        var opMgr = new OperationManager(logger.Object, "TestClass");
        var innerCheck = "not called";

        // act
        var result = await opMgr.OperationAsync<string>(
            () =>
            {
                innerCheck = "called";
                return Task.FromResult(innerCheck);
            },
            "TestMethod");

        // assert
        result.Should().NotBeNull();
        result.Should().Be(innerCheck);
        VerifyLogCalled(logger, Times.Never());
    }

    [TestMethod]
    public async Task OperationAsync_WithException_LogsError()
    {
        // arrange
        var logger = new Mock<ILogger>();
        var opMgr = new OperationManager(logger.Object, "TestClass");

        // act
        var result = await opMgr.OperationAsync<string>(
            () =>
            {
                throw new InvalidOperationException();
            },
            "TestMethod");

        // assert
        result.Should().BeNull();

        logger.Verify(o => o.Log<It.IsAnyType>(
            LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception?>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [TestMethod]
    public async Task OperationAsync_WithIdentityResultSuccess_DoesNotLog()
    {
        // arrange
        var logger = new Mock<ILogger>();
        var opMgr = new OperationManager(logger.Object, "TestClass");
        var innerCheck = "not called";

        // act
        var result = await opMgr.OperationAsync(
            () =>
            {
                innerCheck = "called";
                return Task.FromResult(IdentityResult.Success);
            },
            "TestMethod");

        // assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        innerCheck.Should().Be("called");
        VerifyLogCalled(logger, Times.Never());
    }

    [TestMethod]
    public async Task OperationAsync_WithIdentityResultException_LogsError()
    {
        // arrange
        var logger = new Mock<ILogger>();
        var opMgr = new OperationManager(logger.Object, "TestClass");

        // act
        var result = await opMgr.OperationAsync(
            () =>
            {
                throw new InvalidOperationException();
            },
            "TestMethod");

        // assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
        VerifyLogCalled(logger);
    }

    [TestMethod]
    public async Task OperationAsync_WithIdentityResultFailure_LogsError()
    {
        // arrange
        var logger = new Mock<ILogger>();
        var opMgr = new OperationManager(logger.Object, "TestClass");

        // act
        var result = await opMgr.OperationAsync(
            () =>
            {
                return Task.FromResult(
                    IdentityResult.Failed(
                        new IdentityError
                        {
                            Code = "Test.Error",
                            Description = "test"
                        }));
            },
            "TestMethod");

        // assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeFalse();
        VerifyLogCalled(logger);
    }

    private static void VerifyLogCalled(Mock<ILogger> logger, Times? times = null)
    {
        logger.Verify(o => o.Log<It.IsAnyType>(
            LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception?>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times ??= Times.Once());
    }
}
