
namespace VendorGuard.Domain.Common;

public class BaseEntity
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTimeOffset UpdatedAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public bool Deleted { get; private set; }
    public void MarkDeleted() => Deleted = true;

    public void SetTimestamps(DateTimeOffset now)
    {
        if (CreatedAt == default) CreatedAt = now;
        UpdatedAt = now;
    }
}