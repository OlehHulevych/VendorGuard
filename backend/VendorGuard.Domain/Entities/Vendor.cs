using VendorGuard.Domain.Common;

namespace VendorGuard.Domain.Entities;

public class Vendor:BaseEntity
{
    public string LegalName { get; }
    public string? Lei { get; }
    public string? CountryCode { get; }
    public string? CompanydId { get; }
    public string? Website { get; }
    public string? ContactName { get; }
    public string? ContactEmail { get; }
    public bool IsIntraGroup { get; }
    
    
}