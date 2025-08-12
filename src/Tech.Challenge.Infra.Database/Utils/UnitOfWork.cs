using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Infra.Database.Contexts;

namespace Tech.Challenge.Infra.Database.Utils;

public class UnitOfWork(DataContext dbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
