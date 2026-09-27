namespace VendorGuard.Application.Common.Interfaces;

public interface IClocker
{
    DateTimeOffset UtcNow { get; }
}