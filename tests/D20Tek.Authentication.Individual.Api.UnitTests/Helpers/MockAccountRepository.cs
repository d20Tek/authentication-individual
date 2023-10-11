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
    private readonly bool _allowCreate;
    private readonly bool _getDuplicateUserName;
    private readonly bool _getDuplicateEmail;

    public MockAccountRepository(
        UserAccount? defaultTestAccount = null,
        List<string>? roles = null,
        bool allowCreate = false,
        bool getDuplicateUserName = false,
        bool getDuplicateEmail = false)
    {
        _defaultTestAccount = defaultTestAccount;
        _defaultRoles = roles ?? new List<string>();
        _allowCreate = allowCreate;
        _getDuplicateUserName = getDuplicateUserName;
        _getDuplicateEmail = getDuplicateEmail;
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
        if (_allowCreate)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        return Task.FromResult(IdentityResult.Failed(new IdentityError
        {
            Code = "Test.Error",
            Description = "Test failure."
        }));
    }

    public Task<IdentityResult> DeleteAsync(UserAccount userAccount)
    {
        return Task.FromResult(IdentityResult.Failed(new IdentityError
        {
            Code = "Test.Error",
            Description = "Remove test failure."
        }));
    }

    public Task<string?> GeneratePasswordResetTokenAsync(UserAccount userAccount)
    {
        return Task.FromResult<string?>(null);
    }

    public Task<UserAccount?> GetByEmailAsync(string email)
    {
        if (_getDuplicateEmail)
        {
            return Task.FromResult<UserAccount?>(
                AccountCommandFactory.CreateAccount("TestUser", email: "tester@test.com"));
        }

        return Task.FromResult(_defaultTestAccount);
    }

    public Task<UserAccount?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_defaultTestAccount);
    }

    public Task<UserAccount?> GetByUserNameAsync(string userName)
    {
        if (_getDuplicateUserName)
        {
            return Task.FromResult<UserAccount?>(
                AccountCommandFactory.CreateAccount("TestUser"));
        }
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
        return Task.FromResult(IdentityResult.Failed());
    }

    public Task<IdentityResult> UpdateAsync(UserAccount userAccount)
    {
        return Task.FromResult(IdentityResult.Failed(new IdentityError
        {
            Code = "Test.Error",
            Description = "Update test failure."
        }));
    }
}
