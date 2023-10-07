//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.IdentityModel.Tokens;

namespace D20Tek.Authentication.Individual.Abstractions;

internal interface IJwtTokenGenerator
{
    public SigningCredentials GetSigningCredentials();

    public TokenResponse GenerateAccessToken(UserAccount account, IEnumerable<string> userRoles);

    public TokenResponse GenerateRefreshToken(UserAccount account);
}
