//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Authentication.Individual.Api.UnitTests.Helpers;

[ExcludeFromCodeCoverage]
internal class MockTokenGenerator : IJwtTokenGenerator
{
    public TokenResponse GenerateAccessToken(UserAccount account, IEnumerable<string> userRoles)
    {
        throw new NotImplementedException();
    }

    public TokenResponse GenerateRefreshToken(UserAccount account)
    {
        throw new NotImplementedException();
    }

    public SigningCredentials GetSigningCredentials()
    {
        throw new NotImplementedException();
    }
}
