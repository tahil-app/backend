namespace Tahil.Domain.Repositories;

public interface IUserRepository : IRepository<User> 
{
    Task<Result<bool>> AddUserAsync(User user, Guid tenantId);
    Task<Result<bool>> DeleteUserAsync(int id, Guid tenantId);
}