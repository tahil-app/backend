namespace Tahil.Domain.Repositories;

public interface IUserRepository : IRepository<User> 
{
    Task AddUserAsync(User newUser);
}