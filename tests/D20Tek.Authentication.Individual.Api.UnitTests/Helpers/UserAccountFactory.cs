//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.UseCases.Register;

namespace D20Tek.Authentication.Individual.Api.UnitTests.Helpers;

internal static class AccountCommandFactory
{
    public static RegisterCommand CreateRegisterCommand(
        string userName,
        string? givenName = "Tester",
        string? familyName = "McTest",
        string? email = "tester@test.com",
        string? password = "Password123!",
        string? phoneNumber = null)
    {
        return new RegisterCommand(
            userName,
            givenName!,
            familyName!,
            email!,
            password!,
            phoneNumber);
    }

    public static UserAccount CreateAccount(
        string userName,
        string? givenName = "Tester",
        string? familyName = "McTest",
        string? email = "tester@test.com",
        string? password = "Password123!",
        string? phoneNumber = null)
    {
        return new UserAccount
        {
            UserName = userName,
            GivenName = givenName!,
            FamilyName = familyName!,
            Email = email!,
            PasswordHash = password!,
            PhoneNumber = phoneNumber
        };
    }
}
