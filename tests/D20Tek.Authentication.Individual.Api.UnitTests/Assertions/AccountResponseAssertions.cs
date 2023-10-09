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

    public static async Task ShouldBeEquivalentTo(
        this HttpResponseMessage httpResponse,
        UpdateAccountRequest request)
    {
        var account = await httpResponse.Content.ReadFromJsonAsync<AccountResponse>();

        account.Should().NotBeNull();
        account!.UserId.Should().NotBeEmpty();
        account.UserName.Should().Be(request.UserName);
        account.GivenName.Should().Be(request.GivenName);
        account.FamilyName.Should().Be(request.FamilyName);
        account.Email.Should().Be(request.Email);
        account.PhoneNumber.Should().Be(request.PhoneNumber);
    }

    public static async Task ShouldBeEquivalentTo(
        this HttpResponseMessage httpResponse,
        string expectedUserId)
    {
        var account = await httpResponse.Content.ReadFromJsonAsync<AccountResponse>();

        account.Should().NotBeNull();
        account!.UserId.Should().Be(expectedUserId);
        account.UserName.Should().BeNull();
        account.GivenName.Should().BeNull();
        account.FamilyName.Should().BeNull();
        account.Email.Should().BeNull();
        account.PhoneNumber.Should().BeNull();
    }
}
