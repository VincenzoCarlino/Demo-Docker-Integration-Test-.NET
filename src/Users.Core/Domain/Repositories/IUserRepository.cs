namespace Users.Core.Domain.Repositories;

using LanguageExt;

using System;
using System.Threading.Tasks;

using Users.Core.Domain.DTO.Errors;
using Users.Core.Domain.Models;

public interface IUserRepository
{
    Task<Either<EntityNotFoundError, User>> GetByIdAsync(Guid id);

    Task<User> InsertAsync(User user);
}
