using Tech.Challenge.Domain.Interfaces;

namespace Tech.Challenge.Unit;

internal class UnitOfWorkMock : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
