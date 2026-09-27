using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using VendorGuard.Application.Common.Interfaces;
using VendorGuard.Domain.Common;

namespace VendorGuard.Infrastructure.Presistance;

public sealed class AuditableEntitySaveChangesInterceptor:SaveChangesInterceptor
{
    private readonly IClocker _clocker;

    public AuditableEntitySaveChangesInterceptor(IClocker clocker) => _clocker = clocker;

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateTimeStamps(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateTimeStamps(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);  
    }

    private void UpdateTimeStamps(DbContext? context)
    {
        if (context is null) return;
        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>() )
        {
            if (entry.State is EntityState.Added or EntityState.Modified)
            {
                entry.Entity.SetTimestamps(_clocker.UtcNow);
            }
        }
    }
}