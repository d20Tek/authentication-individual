//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Authentication.Individual.Abstractions;
using D20Tek.Authentication.Individual.Infrastructure;
using D20Tek.Authentication.Individual.UseCases;
using D20Tek.Authentication.Individual.UseCases.ChangeRole;
using D20Tek.Authentication.Individual.UseCases.Login;
using D20Tek.Authentication.Individual.UseCases.Register;
using D20Tek.Minimal.Result;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Authentication.Individual.Api.UnitTests.Helpers;

internal class AuthenticationWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace the real database context with an in-memory database
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<UserAccountDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<UserAccountDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });
        });
    }

    public async Task<AuthenticationResult> RegisterTestUser(RegisterCommand register)
    {
        // Create an instance of your DbContext
        using var scope = Services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IUserAccountRepository>();

        Result<AuthenticationResult> result;
        if (await repository.GetByUserNameAsync(register.UserName) == null)
        {
            var handler = scope.ServiceProvider.GetRequiredService<IRegisterCommandHandler>();

            // Add test data to the DbContext
            result = await handler.HandleAsync(register, CancellationToken.None);
            EnsureSuccess(result);
        }
        else
        {
            var handler = scope.ServiceProvider.GetRequiredService<ILoginQueryHandler>();

            result = await handler.HandleAsync(
                new LoginQuery(register.UserName, register.Password),
                CancellationToken.None);
            EnsureSuccess(result);
        }
        
        return result.Value;
    }

    public HttpClient CreateAuthenticatedClient(string token)
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        return client;
    }

    public IJwtTokenGenerator GetJwtTokenGenerator() =>
        Services.GetRequiredService<IJwtTokenGenerator>();

    public IChangeRoleCommandHandler GetChangeRoleCommandHandler()
    {
        var scope = Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IChangeRoleCommandHandler>();
    }

    [ExcludeFromCodeCoverage]
    internal void EnsureSuccess(Result<AuthenticationResult> result)
    {
        if (result.IsFailure)
        {
            throw new InvalidOperationException();
        }
    }
}
