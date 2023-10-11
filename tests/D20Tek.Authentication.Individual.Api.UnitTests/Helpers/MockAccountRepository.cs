//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Authentication.Individual.Api.UnitTests.Helpers;

[ExcludeFromCodeCoverage]
internal class MockAccountRepository : IUserAccountRepository
{
    private readonly UserAccount? _defaultTestAccount;
    private readonly List<string> _defaultRoles;

    public MockAccountRepository(UserAccount? defaultTestAccount = null, List<string>? roles = null)
    {
        _defaultTestAccount = defaultTestAccount;
        _defaultRoles = roles ?? new List<string>();
    }

    public Task<bool> AttachUserRoleAsync(UserAccount userAccount, string userRole)
    {
        return Task.FromResult(false);
    }

    public Task<IdentityResult> ChangePasswordAsync(
        UserAccount userAccount,
        string currentPassword,
        string newPassword)
    {
        return Task.FromResult(IdentityResult.Failed());
    }

    public Task<bool> CheckPasswordAsync(UserAccount userAccountstring, string password)
    {
        return Task.FromResult(true);
    }

    public Task<IdentityResult> CreateAsync(UserAccount userAccount, string password)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> DeleteAsync(UserAccount userAccount)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GeneratePasswordResetTokenAsync(UserAccount userAccount)
    {
        throw new NotImplementedException();
    }

    public Task<UserAccount?> GetByEmailAsync(string email)
    {
        return Task.FromResult(_defaultTestAccount);
    }

    public Task<UserAccount?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_defaultTestAccount);
    }

    public Task<UserAccount?> GetByUserNameAsync(string userName)
    {
        return Task.FromResult(_defaultTestAccount);
    }

    public Task<IEnumerable<string>> GetUserRolesAsync(UserAccount userAccount)
    {
        return Task.FromResult<IEnumerable<string>>(_defaultRoles);
    }

    public Task<bool> RemoveUserRolesAsync(
        UserAccount userAccount,
        IEnumerable<string> userRoles)
    {
        return Task.FromResult(false);
    }

    public Task<IdentityResult> ResetPasswordAsync(
        UserAccount userAccount,
        string resetToken,
        string newPassword)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> UpdateAsync(UserAccount userAccount)
    {
        throw new NotImplementedException();
    }
}
