namespace Application;

public interface ICurrentUserService
{
    Guid? UserGuid { get; }
    bool HasPermission(string permission);
}

public interface IFileStorageService
{
    Task<string> SaveFileAsync(byte[] fileBytes, string fileName, string folder, CancellationToken cancellationToken = default);
    Task<byte[]> GetFileAsync(string filePath, CancellationToken cancellationToken = default);
    Task<string> GetFileUrlAsync(string filePath, CancellationToken cancellationToken = default);
    Task DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);
}

public interface IApplicationDbContext
{
    DbSet<Category> Categories { get; }
    DbSet<ItemMaster> ItemMasters { get; }

    DbSet<Property> Properties { get; }
    DbSet<PropertyType> PropertyTypes { get; }
    DbSet<PropertyCategory> PropertyCategories { get; }
    DbSet<PropertyLocation> PropertyLocations { get; }
    DbSet<SafetyBox> SafetyBoxes { get; }
    DbSet<SafetyBoxShelf> SafetyBoxShelves { get; }

    DbSet<ApprovalStatus> ApprovalStatuses { get; }
    DbSet<ApprovalWorkflow> ApprovalWorkflows { get; }
    DbSet<Approver> Approvers { get; }

    DbSet<DocumentAttachment> DocumentAttachments { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<AuditTrail> AuditTrails { get; }

    DbSet<ServiceRequest> ServiceRequests { get; }
    DbSet<SR_Detail> SR_Details { get; }
    DbSet<StoreIssueVoucher> StoreIssueVouchers { get; }

    DbSet<ReceivingNote> ReceivingNotes { get; }
    DbSet<InspectionLog> InspectionLogs { get; }
    DbSet<Supplier> Suppliers { get; }

    DbSet<Warehouse> Warehouses { get; }
    DbSet<ShelfLocation> ShelfLocations { get; }
    DbSet<InventoryStock> InventoryStocks { get; }
    DbSet<StockLedger> StockLedgers { get; }

    DbSet<TransferRecord> TransferRecords { get; }
    DbSet<UserLogin> UserLogins { get; }
    DbSet<Employee> Employees { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
