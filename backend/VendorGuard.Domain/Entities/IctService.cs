using VendorGuard.Domain.Common;
using VendorGuard.Domain.Enums;

namespace VendorGuard.Domain.Entities;

public class IctService:BaseEntity
{

    public Guid VendorId { get; private set; }
    public Vendor Vendor { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public List<String> DataLocations { get; private set; } 
    public bool ProcessesPersonalData { get; private set; }
    public Substitutability Substitutability { get; private set; }

    protected IctService()
    {
        Vendor = null!;
        Name = null!;
        Description = null!;
        DataLocations = new List<string>();
        ProcessesPersonalData = false;
        Substitutability = Substitutability.Easy;
    }
    
    public IctService(Guid vendorId, string name, string description, List<string> dataLocations, bool processesPersonalData, Substitutability substitutability)
    {
        VendorId = vendorId;
        Vendor = null!;
        Name = name;
        Description = description;
        DataLocations = dataLocations;
        ProcessesPersonalData = processesPersonalData;
        Substitutability = substitutability;
    }
    
    
}