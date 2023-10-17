//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Authentication.Individual;

public class AuthApiSettings
{
    public bool EnableOpenApi { get; init; } = true;

    public string AuthDbConnectionName { get; init; } = "DefaultConnection";
}
