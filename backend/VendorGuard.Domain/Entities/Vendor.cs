using VendorGuard.Domain.Common;
using VendorGuard.Domain.Enums;

namespace VendorGuard.Domain.Entities;

public class Vendor:BaseEntity
{
   

    public string LegalName { get; private set; }
    public string? Lei { get; private set; }
    public string? CountryCode { get; private set; }
    public string? CompanyId { get; private set; }
    public string? Website { get; private set; }
    public string? ContactName { get; private set; }
    public string? ContactEmail { get; private set; }
    public bool IsIntraGroup { get; private set; }
    public VendorStatus Status { get; private set; }

    protected Vendor()
    {
        LegalName = null!;
        Lei = null;
        CountryCode = null;
        CompanyId = null;
        Website = null;
        ContactName = null;
        ContactEmail = null;
        IsIntraGroup = false;
        Status = VendorStatus.Onboarding;
    }
    
    public Vendor(string legalName, string? lei, string? countryCode, string? companyId, string? website, string? contactName, string? contactEmail, bool isIntraGroup)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(legalName);
        LegalName = legalName;
        Lei = lei;
        CountryCode = countryCode;
        CompanyId = companyId;
        Website = website;
        ContactName = contactName;
        ContactEmail = contactEmail;
        IsIntraGroup = isIntraGroup;
        Status = VendorStatus.Onboarding;
    }

    public void UpdateStatus(VendorStatus status)
    {
        Status = status;
    }

    public void UpdateVendor(string? legalName, string? lei, string? countryCode, string? companyId, string? website, string? contactName, string? contactEmail, bool isIntraGroup)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(legalName);
        LegalName = legalName;
        Lei = lei;
        CountryCode = countryCode;
        CompanyId = companyId;
        Website = website;
        ContactName = contactName;
        ContactEmail = contactEmail;
        IsIntraGroup = isIntraGroup;
        
    }
    

    
}