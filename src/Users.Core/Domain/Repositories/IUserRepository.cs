namespace Users.Core.Domain.Repositories;

using System.Collections.Generic;
using System.Threading.Tasks;

using Users.Core.Domain.Models;

internal interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync();
}
