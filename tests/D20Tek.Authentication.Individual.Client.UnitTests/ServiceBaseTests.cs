//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using Res = D20Tek.Minimal.Result;
using System.Net.Http.Json;
using D20Tek.Minimal.Result.Client;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace D20Tek.Authentication.Individual.Client.UnitTests;

[TestClass]
public class ServiceBaseTests
{
    [TestMethod]
    public async Task InvokeServiceOperation_WithException_ReturnsErrorResult()
    {
        // arrange
        var service = new FakeService();

        // act
        var result = await service.DoServiceOperation();

        // assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
    }

    [TestMethod]
    public async Task InvokeOperation_WithException_ReturnsErrorResult()
    {
        // arrange
        var service = new FakeService();

        // act
        var result = await service.DoOperation();

        // assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
    }
}
