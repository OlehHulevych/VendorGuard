using VendorGuard.Application.Common.Interfaces;

namespace VendorGuard.Infrastructure;

public sealed class SystemClock:IClocker
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}