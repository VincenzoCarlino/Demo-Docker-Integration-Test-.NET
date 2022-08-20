namespace Users.Core.Infrastracture.Persistence.Repositories;

using LanguageExt;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

using Users.Core.Domain.DTO.Errors;
using Users.Core.Domain.Models;
using Users.Core.Domain.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Either<EntityNotFoundError, User>> GetByIdAsync(Guid id)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == id)
            .ConfigureAwait(false);

        if (user is null)
        {
            return EntityNotFoundError.UserNotFoundById(id);
        }

        return user;
    }

    public async Task<User> InsertAsync(User user)
    {
        await _dbContext.AddAsync(user).ConfigureAwait(false);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        return user;
    }
}
