//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Authentication.Individual.Api.UnitTests;

[TestClass]
public class DependencyInjectionTests
{
    [TestMethod]
    [ExcludeFromCodeCoverage]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddDatabaseServices_WithMissingConfig_ThrowsException()
    {
        // arrange
        var services = new ServiceCollection();
        var config = new ConfigurationManager();

        // act
        services.AddIndividualAuthentication(config);
    }
}
