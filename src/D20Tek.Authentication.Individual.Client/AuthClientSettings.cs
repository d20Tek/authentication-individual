//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Authentication.Individual.Client;

internal class AuthClientSettings
{
    public string AppTitle { get; init; } = "Authentication";

    public string AppHomeUrl { get; init; } = "/";

    public string LogoutUrl { get; init; } = "/";
}
