//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.UseCases.Register;
using System.Net.Http.Json;

namespace D20Tek.Authentication.Individual.Api.UnitTests.Assertions;

internal static class AccountResponseAssertions
{
    public static async Task ShouldBeEquivalentTo(
        this HttpResponseMessage httpResponse,
        RegisterCommand command)
    {
        var account = await httpResponse.Content.ReadFromJsonAsync<AccountResponse>();

        account.Should().NotBeNull();
        account!.UserId.Should().NotBeEmpty();
        account.UserName.Should().Be(command.UserName);
        account.GivenName.Should().Be(command.GivenName);
        account.FamilyName.Should().Be(command.FamilyName);
        account.Email.Should().Be(command.Email);
        account.PhoneNumber.Should().Be(command.PhoneNumber);
    }
}
