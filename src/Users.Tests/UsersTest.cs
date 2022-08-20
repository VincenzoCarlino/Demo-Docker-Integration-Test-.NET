namespace Users.Tests;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

using System.Threading.Tasks;

using Users.Core.Domain.Repositories;
using Users.Core.Domain.Models;
using System;

public class UsersTest
{
    /**
     * Goal of this test is to check the behaviour when we try to get a user that doesn't exist
    */
    [Test, Order(1)]
    public async Task UserNotFoundAsync()
    {
        using (var scope = SetUp.GetServiceScopeFactory().CreateScope())
        {
            var userId = System.Guid.Empty;
            var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var getUserResult = await repository.GetByIdAsync(userId).ConfigureAwait(false);

            getUserResult.Match(
                _ =>
                {
                    Assert.Fail("We shouldn't get user");
                },
                entityNotFoundError =>
                {
                    Assert.That(entityNotFoundError.EntityName, Is.EqualTo("user"));
                    Assert.That(entityNotFoundError.PropertyValue, Is.EqualTo(userId));
                    Assert.That(entityNotFoundError.PropertyName, Is.EqualTo("id"));
                }
            );
        }
    }

    /**
     * Goal of this test is to try the insert of an user
    */
    [Test, Order(2)]
    public async Task InsertAndRetrieveUserAsync()
    {
        using (var scope = SetUp.GetServiceScopeFactory().CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var user = await repository
                .InsertAsync(
                    new(
                        Guid.NewGuid(),
                        "Vincenzo",
                        "Carlino",
                        "vincenzocarlino147@gmail.com"
                    )
                )
                .ConfigureAwait(false);

            var getUserResult = await repository.GetByIdAsync(user.Id).ConfigureAwait(false);

            getUserResult.Match(
                retrievedUser =>
                {
                    Assert.That(user.Id, Is.EqualTo(retrievedUser.Id));
                    Assert.That(user.FirstName, Is.EqualTo(retrievedUser.FirstName));
                    Assert.That(user.LastName, Is.EqualTo(retrievedUser.LastName));
                    Assert.That(user.Email, Is.EqualTo(retrievedUser.Email));
                },
                _ =>
                {
                    Assert.Fail("The new user is not been inserted");
                }
            );
        }
    }
}