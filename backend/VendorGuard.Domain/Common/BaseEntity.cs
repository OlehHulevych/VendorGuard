using System.ComponentModel.DataAnnotations;

namespace VendorGuard.Domain.Common;

public class BaseEntity
{
    [Key] public Guid Id { get; } = Guid.NewGuid();
    public DateTimeOffset UpdatedAt { get; } = new DateTimeOffset();
    public DateTimeOffset CreatedAt { get; } = new DateTimeOffset();
    public Boolean Deleted { get; } = false;
}