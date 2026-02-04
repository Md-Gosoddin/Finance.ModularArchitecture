namespace Evently.Module.User.Application.Repositry;
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}
