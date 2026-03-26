using Application.Features.Catalog.Categories.Dtos;
using Application.Features.Catalog.ItemMasters.Dtos;
using Application.Features.Catalog.ItemMasters.Queries.GetLowStockItems;
using Application.Features.Common.AuditTrail.Dtos;
using Application.Features.Common.DocumentAttachments.Dtos;
using Application.Features.Common.Notifications.Dtos;
using Application.Features.Dashboard.Dtos;
using Application.Features.Disposal.Dtos;
using Application.Features.PropertyManagement.Locations.Dtos;
using Application.Features.PropertyManagement.Properties.Dtos;
using Application.Features.PropertyManagement.PropertyCategories.Dtos;
using Application.Features.PropertyManagement.PropertyTypes.Dtos;
using Application.Features.PropertyManagement.SafetyBoxes.Dtos;
using Application.Features.Receiving.Inspections.Dtos;
using Application.Features.Receiving.ReceivingNotes.Dtos;
using Application.Features.Receiving.Suppliers.Dtos;
using Application.Features.Reports.DisposalReport.Dtos;
using Application.Features.Reports.InventoryValuationReport.Dtos;
using Application.Features.Reports.PropertyValuationReport.Dtos;
using Application.Features.Reports.RequisitionHistoryReport.Dtos;
using Application.Features.Reports.StockMovementReport.Dtos;
using Application.Features.Requisition.ServiceRequests.Dtos;
using Application.Features.Requisition.StoreIssueVouchers.Dtos;
using Application.Features.Storage.InventoryStock.Dtos;
using Application.Features.Storage.ShelfLocations.Dtos;
using Application.Features.Storage.StockLedger.Dtos;
using Application.Features.Storage.Warehouses.Dtos;
using Application.Features.TransferReturn.ReturnMaterialRequests.Dtos;
using Application.Features.TransferReturn.TransferRecords.Dtos;
using Application.Features.Users.Authentication.Dtos;
using Application.Features.Users.Employees.Dtos;
using Application.Features.Users.Roles.Dtos;
using Application.Features.Workflow.ApprovalStatuses.Dtos;
using Application.Features.Workflow.ApprovalWorkflows.Dtos;
using Application.Features.Workflow.Approvers.Dtos;
using AutoMapper;
using Domain.Catalog;
using Domain.Common;
using Domain.Disposal;
using Domain.PropertyManagement;
using Domain.Receiving;
using Domain.Requisition;
using Domain.Storage;
using Domain.TransferReturn;
using Domain.Users;
using Domain.Workflow;
using PAS.API.Models.DTOs;
using PAS.API.Models.Requests;
using PAS.API.Models.Responses;

namespace PAS.API.Mappings;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        // ============================================================================
        // Domain to Application DTOs (Already mapped in Application layer)
        // These are for API-specific mappings
        // ============================================================================

        // Dashboard Mappings
        CreateDashboardMappings();

        // Catalog Mappings
        CreateCatalogMappings();

        // Property Management Mappings
        CreatePropertyManagementMappings();

        // Storage Mappings
        CreateStorageMappings();

        // Receiving Mappings
        CreateReceivingMappings();

        // Requisition Mappings
        CreateRequisitionMappings();

        // Transfer & Return Mappings
        CreateTransferReturnMappings();

        // Disposal Mappings
        CreateDisposalMappings();

        // Workflow Mappings
        CreateWorkflowMappings();

        // User Management Mappings
        CreateUserMappings();

        // Common Mappings
        CreateCommonMappings();

        // Report Mappings
        CreateReportMappings();

        // Request/Response Mappings
        CreateRequestResponseMappings();
    }

    private void CreateDashboardMappings()
    {
        // Map from Application DTOs to API DTOs
        CreateMap<Application.Features.Dashboard.Dtos.DashboardDto, PAS.API.Models.DTOs.DashboardDto>()
            .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Requisitions, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Receiving, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Charts, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.RecentActivities, opt => opt.MapFrom(src => src.RecentActivities))
            .ForMember(dest => dest.Alerts, opt => opt.MapFrom(src => new AlertsDto
            {
                LowStockAlerts = src.LowStockAlerts,
                PendingTasks = src.PendingTasks
            }))
            .ForMember(dest => dest.QuickActions, opt => opt.Ignore());

        CreateMap<Application.Features.Dashboard.Dtos.AuditTrailDto, RecentActivityDto>()
            .ForMember(dest => dest.EntityTitle, opt => opt.Ignore())
            .ForMember(dest => dest.UserAvatar, opt => opt.Ignore())
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => $"{src.Action} {src.EntityName}"));

        CreateMap<Application.Features.Dashboard.Dtos.LowStockItemDto, LowStockAlertDto>()
            .ForMember(dest => dest.Severity, opt => opt.MapFrom(src =>
                src.CurrentStock == 0 ? "Critical" :
                src.CurrentStock <= src.MinStockLevel * 0.3 ? "Critical" :
                src.CurrentStock <= src.MinStockLevel * 0.6 ? "Warning" : "Info"))
            .ForMember(dest => dest.Deficit, opt => opt.MapFrom(src => src.MinStockLevel - src.CurrentStock))
            .ForMember(dest => dest.LastRestocked, opt => opt.Ignore())
            .ForMember(dest => dest.UnitOfMeasure, opt => opt.Ignore());

        CreateMap<Application.Features.Dashboard.Dtos.PendingTaskDto, PendingTaskDto>()
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => GetTaskIcon(src.TaskType)))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => GetTaskColor(src.Priority)));

        CreateMap<Application.Features.Dashboard.Dtos.PropertyByLocationDto, PropertyByLocationDto>();
        CreateMap<Application.Features.Dashboard.Dtos.PropertyByTypeDto, PropertyByTypeDto>();
        CreateMap<Application.Features.Dashboard.Dtos.PropertyByCategoryDto, PropertyByCategoryDto>();
        CreateMap<Application.Features.Dashboard.Dtos.PropertyAcquisitionDto, PropertyAcquisitionDto>();
        CreateMap<Application.Features.Dashboard.Dtos.TopStockItemDto, TopStockItemDto>();
    }

    private void CreateCatalogMappings()
    {
        // Category mappings
        CreateMap<CategoryDto, CategoryResponseDto>()
            .ForMember(dest => dest.HasChildren, opt => opt.MapFrom(src => src.SubCategoriesCount > 0))
            .ForMember(dest => dest.FullPath, opt => opt.Ignore());

        CreateMap<CategoryDetailDto, CategoryDetailResponseDto>();
        CreateMap<CategoryHierarchyDto, CategoryHierarchyResponseDto>();

        // ItemMaster mappings
        CreateMap<ItemMasterDto, ItemMasterResponseDto>();
        CreateMap<ItemMasterDetailDto, ItemMasterDetailResponseDto>();
        CreateMap<ItemMasterListDto, ItemMasterListResponseDto>();
        CreateMap<ItemStockLocationDto, ItemStockLocationResponseDto>();
        CreateMap<ItemMovementDto, ItemMovementResponseDto>();
        CreateMap<LowStockItemDto, LowStockItemResponseDto>();
    }

    private void CreatePropertyManagementMappings()
    {
        // Property mappings
        CreateMap<PropertyDto, PropertyResponseDto>()
            .ForMember(dest => dest.FormattedValue, opt => opt.MapFrom(src => src.TotalValue.ToString("C")))
            .ForMember(dest => dest.FormattedPrice, opt => opt.MapFrom(src => src.UnitPrice.ToString("C")))
            .ForMember(dest => dest.FormattedPurchaseDate, opt => opt.MapFrom(src => src.PurchaseDate.ToString("yyyy-MM-dd")));

        CreateMap<PropertyDetailDto, PropertyDetailResponseDto>();
        CreateMap<PropertyAttachmentDto, PropertyAttachmentResponseDto>();
        CreateMap<PropertyMovementDto, PropertyMovementResponseDto>();

        // PropertyType mappings
        CreateMap<PropertyTypeDto, PropertyTypeResponseDto>();
        CreateMap<PropertyTypeDetailDto, PropertyTypeDetailResponseDto>();

        // PropertyCategory mappings
        CreateMap<PropertyCategoryDto, PropertyCategoryResponseDto>();
        CreateMap<PropertyCategoryDetailDto, PropertyCategoryDetailResponseDto>();
        CreateMap<PropertyCategoryPropertyDto, PropertyCategoryPropertyResponseDto>();

        // Location mappings
        CreateMap<LocationDto, LocationResponseDto>();
        CreateMap<LocationDetailDto, LocationDetailResponseDto>();
        CreateMap<LocationSafetyBoxDto, LocationSafetyBoxResponseDto>();

        // SafetyBox mappings
        CreateMap<SafetyBoxDto, SafetyBoxResponseDto>();
        CreateMap<SafetyBoxDetailDto, SafetyBoxDetailResponseDto>();
        CreateMap<SafetyBoxShelfDto, SafetyBoxShelfResponseDto>();
        CreateMap<SafetyBoxPropertyDto, SafetyBoxPropertyResponseDto>();
    }

    private void CreateStorageMappings()
    {
        // Warehouse mappings
        CreateMap<WarehouseDto, WarehouseResponseDto>();
        CreateMap<WarehouseDetailDto, WarehouseDetailResponseDto>();
        CreateMap<WarehouseShelfLocationDto, WarehouseShelfLocationResponseDto>();

        // ShelfLocation mappings
        CreateMap<ShelfLocationDto, ShelfLocationResponseDto>()
            .ForMember(dest => dest.QRCodeUrl, opt => opt.Ignore());

        CreateMap<ShelfLocationDetailDto, ShelfLocationDetailResponseDto>();
        CreateMap<ShelfInventoryDto, ShelfInventoryResponseDto>();

        // InventoryStock mappings
        CreateMap<InventoryStockDto, InventoryStockResponseDto>();
        CreateMap<InventoryStockDetailDto, InventoryStockDetailResponseDto>();

        // StockLedger mappings
        CreateMap<StockLedgerDto, StockLedgerResponseDto>();
        CreateMap<StockMovementDto, StockMovementResponseDto>();
    }

    private void CreateReceivingMappings()
    {
        // Supplier mappings
        CreateMap<SupplierDto, SupplierResponseDto>();
        CreateMap<SupplierDetailDto, SupplierDetailResponseDto>();
        CreateMap<SupplierReceivingNoteDto, SupplierReceivingNoteResponseDto>();

        // ReceivingNote mappings
        CreateMap<ReceivingNoteDto, ReceivingNoteResponseDto>();
        CreateMap<ReceivingNoteDetailDto, ReceivingNoteDetailResponseDto>();
        CreateMap<ReceivingItemDto, ReceivingItemResponseDto>();
        CreateMap<InspectionInfoDto, InspectionInfoResponseDto>();

        // Inspection mappings
        CreateMap<InspectionDto, InspectionResponseDto>();
    }

    private void CreateRequisitionMappings()
    {
        // ServiceRequest mappings
        CreateMap<ServiceRequestDto, ServiceRequestResponseDto>()
            .ForMember(dest => dest.FormattedRequestDate, opt => opt.MapFrom(src => src.RequestDate.ToString("yyyy-MM-dd HH:mm")));

        CreateMap<ServiceRequestDetailDto, ServiceRequestDetailResponseDto>();
        CreateMap<ServiceRequestItemDto, ServiceRequestItemResponseDto>();
        CreateMap<CreateServiceRequestDto, CreateServiceRequestRequest>();

        // StoreIssueVoucher mappings
        CreateMap<StoreIssueVoucherDto, StoreIssueVoucherResponseDto>();
        CreateMap<StoreIssueVoucherDetailDto, StoreIssueVoucherDetailResponseDto>();
        CreateMap<SIVItemDto, SIVItemResponseDto>();
    }

    private void CreateTransferReturnMappings()
    {
        // TransferRecord mappings
        CreateMap<TransferRecordDto, TransferRecordResponseDto>();
        CreateMap<TransferRecordDetailDto, TransferRecordDetailResponseDto>();

        // ReturnMaterialRequest mappings
        CreateMap<ReturnMaterialRequestDto, ReturnMaterialRequestResponseDto>();
    }

    private void CreateDisposalMappings()
    {
        CreateMap<DisposalRecordDto, DisposalRecordResponseDto>();
        CreateMap<DisposalRecordDetailDto, DisposalRecordDetailResponseDto>();
    }

    private void CreateWorkflowMappings()
    {
        // ApprovalWorkflow mappings
        CreateMap<ApprovalWorkflowDto, ApprovalWorkflowResponseDto>();
        CreateMap<ApprovalWorkflowDetailDto, ApprovalWorkflowDetailResponseDto>();
        CreateMap<WorkflowApproverDto, WorkflowApproverResponseDto>();

        // Approver mappings
        CreateMap<ApproverDto, ApproverResponseDto>();

        // ApprovalStatus mappings
        CreateMap<ApprovalStatusDto, ApprovalStatusResponseDto>();
    }

    private void CreateUserMappings()
    {
        // Employee mappings
        CreateMap<EmployeeDto, EmployeeResponseDto>();
        CreateMap<EmployeeDetailDto, EmployeeDetailResponseDto>();
        CreateMap<UserAccountInfoDto, UserAccountInfoResponseDto>();

        // Role mappings
        CreateMap<RoleDto, RoleResponseDto>();

        // Authentication mappings
        CreateMap<UserInfoDto, UserInfoResponseDto>();
        CreateMap<AuthResultDto, AuthResultResponseDto>();
    }

    private void CreateCommonMappings()
    {
        // AuditTrail mappings
        CreateMap<AuditTrailDto, AuditTrailResponseDto>();
        CreateMap<AuditTrailListDto, AuditTrailListResponseDto>();

        // Notification mappings
        CreateMap<NotificationDto, NotificationResponseDto>();
        CreateMap<NotificationListDto, NotificationListResponseDto>();

        // DocumentAttachment mappings
        CreateMap<DocumentAttachmentDto, DocumentAttachmentResponseDto>();
    }

    private void CreateReportMappings()
    {
        // Property Valuation Report
        CreateMap<PropertyValuationReportDto, PropertyValuationReportResponseDto>();
        CreateMap<LocationValuationDto, LocationValuationResponseDto>();
        CreateMap<TypeValuationDto, TypeValuationResponseDto>();
        CreateMap<PropertySummaryDto, PropertySummaryResponseDto>();

        // Requisition History Report
        CreateMap<RequisitionHistoryReportDto, RequisitionHistoryReportResponseDto>();
        CreateMap<StatusSummaryDto, StatusSummaryResponseDto>();
        CreateMap<RequisitionSummaryDto, RequisitionSummaryResponseDto>();

        // Stock Movement Report
        CreateMap<StockMovementReportDto, StockMovementReportResponseDto>();
        CreateMap<MovementTypeSummaryDto, MovementTypeSummaryResponseDto>();
        CreateMap<StockMovementDetailDto, StockMovementDetailResponseDto>();

        // Disposal Report
        CreateMap<DisposalReportDto, DisposalReportResponseDto>();
        CreateMap<DisposalSummaryDto, DisposalSummaryResponseDto>();
        CreateMap<DisposalDetailDto, DisposalDetailResponseDto>();

        // Inventory Valuation Report
        CreateMap<InventoryValuationReportDto, InventoryValuationReportResponseDto>();
        CreateMap<InventoryValuationItemDto, InventoryValuationItemResponseDto>();
        CreateMap<InventoryValuationLocationDto, InventoryValuationLocationResponseDto>();
    }

    private void CreateRequestResponseMappings()
    {
        // Request to Command mappings
        CreateMap<CreatePropertyRequest, Application.Features.PropertyManagement.Properties.Commands.CreateProperty.CreatePropertyCommand>();
        CreateMap<UpdatePropertyRequest, Application.Features.PropertyManagement.Properties.Commands.UpdateProperty.UpdatePropertyCommand>();
        CreateMap<TransferPropertyRequest, Application.Features.PropertyManagement.Properties.Commands.TransferProperty.TransferPropertyCommand>();

        CreateMap<CreateCategoryRequest, Application.Features.Catalog.Categories.Commands.CreateCategory.CreateCategoryCommand>();
        CreateMap<UpdateCategoryRequest, Application.Features.Catalog.Categories.Commands.UpdateCategory.UpdateCategoryCommand>();

        CreateMap<CreateItemRequest, Application.Features.Catalog.ItemMasters.Commands.CreateItemMaster.CreateItemMasterCommand>();
        CreateMap<UpdateItemRequest, Application.Features.Catalog.ItemMasters.Commands.UpdateItemMaster.UpdateItemMasterCommand>();

        CreateMap<CreateLocationRequest, Application.Features.PropertyManagement.Locations.Commands.CreateLocation.CreateLocationCommand>();
        CreateMap<UpdateLocationRequest, Application.Features.PropertyManagement.Locations.Commands.UpdateLocation.UpdateLocationCommand>();

        CreateMap<CreateSafetyBoxRequest, Application.Features.PropertyManagement.SafetyBoxes.Commands.CreateSafetyBox.CreateSafetyBoxCommand>();
        CreateMap<UpdateSafetyBoxRequest, Application.Features.PropertyManagement.SafetyBoxes.Commands.UpdateSafetyBox.UpdateSafetyBoxCommand>();

        CreateMap<CreatePropertyTypeRequest, Application.Features.PropertyManagement.PropertyTypes.Commands.CreatePropertyType.CreatePropertyTypeCommand>();
        CreateMap<UpdatePropertyTypeRequest, Application.Features.PropertyManagement.PropertyTypes.Commands.UpdatePropertyType.UpdatePropertyTypeCommand>();

        CreateMap<CreatePropertyCategoryRequest, Application.Features.PropertyManagement.PropertyCategories.Commands.CreatePropertyCategory.CreatePropertyCategoryCommand>();
        CreateMap<UpdatePropertyCategoryRequest, Application.Features.PropertyManagement.PropertyCategories.Commands.UpdatePropertyCategory.UpdatePropertyCategoryCommand>();

        CreateMap<CreateWarehouseRequest, Application.Features.Storage.Warehouses.Commands.CreateWarehouse.CreateWarehouseCommand>();
        CreateMap<UpdateWarehouseRequest, Application.Features.Storage.Warehouses.Commands.UpdateWarehouse.UpdateWarehouseCommand>();

        CreateMap<CreateShelfLocationRequest, Application.Features.Storage.ShelfLocations.Commands.CreateShelfLocation.CreateShelfLocationCommand>();
        CreateMap<UpdateShelfLocationRequest, Application.Features.Storage.ShelfLocations.Commands.UpdateShelfLocation.UpdateShelfLocationCommand>();

        CreateMap<CreateSupplierRequest, Application.Features.Receiving.Suppliers.Commands.CreateSupplier.CreateSupplierCommand>();
        CreateMap<UpdateSupplierRequest, Application.Features.Receiving.Suppliers.Commands.UpdateSupplier.UpdateSupplierCommand>();

        CreateMap<CreateReceivingNoteRequest, Application.Features.Receiving.ReceivingNotes.Commands.CreateReceivingNote.CreateReceivingNoteCommand>();
        CreateMap<CreateInspectionRequest, Application.Features.Receiving.Inspections.Commands.CreateInspection.CreateInspectionCommand>();

        CreateMap<CreateServiceRequestRequest, Application.Features.Requisition.ServiceRequests.Commands.CreateServiceRequest.CreateServiceRequestCommand>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<CreateStoreIssueVoucherRequest, Application.Features.Requisition.StoreIssueVouchers.Commands.CreateStoreIssueVoucher.CreateStoreIssueVoucherCommand>();

        CreateMap<CreateTransferRecordRequest, Application.Features.TransferReturn.TransferRecords.Commands.CreateTransferRecord.CreateTransferRecordCommand>();
        CreateMap<CreateReturnRequestRequest, Application.Features.TransferReturn.ReturnMaterialRequests.Commands.CreateReturnRequest.CreateReturnRequestCommand>();

        CreateMap<CreateDisposalRecordRequest, Application.Features.Disposal.Commands.CreateDisposalRecord.CreateDisposalRecordCommand>();

        CreateMap<CreateWorkflowRequest, Application.Features.Workflow.ApprovalWorkflows.Commands.CreateWorkflow.CreateWorkflowCommand>();
        CreateMap<UpdateWorkflowRequest, Application.Features.Workflow.ApprovalWorkflows.Commands.UpdateWorkflow.UpdateWorkflowCommand>();
        CreateMap<AssignApproverRequest, Application.Features.Workflow.Approvers.Commands.AssignApprover.AssignApproverCommand>();

        CreateMap<CreateEmployeeRequest, Application.Features.Users.Employees.Commands.CreateEmployee.CreateEmployeeCommand>();
        CreateMap<UpdateEmployeeRequest, Application.Features.Users.Employees.Commands.UpdateEmployee.UpdateEmployeeCommand>();
        CreateMap<CreateUserRequest, Application.Features.Users.Authentication.Commands.RegisterUser.RegisterUserCommand>();
    }

    #region Helper Methods

    private string GetTaskIcon(string taskType)
    {
        return taskType.ToLower() switch
        {
            "requisition approval" => "bi-clipboard-check",
            "inspection" => "bi-eye",
            "transfer" => "bi-arrow-left-right",
            "disposal" => "bi-trash",
            "return" => "bi-arrow-return-left",
            _ => "bi-bell"
        };
    }

    private string GetTaskColor(string priority)
    {
        return priority.ToLower() switch
        {
            "high" => "danger",
            "medium" => "warning",
            "low" => "info",
            _ => "secondary"
        };
    }

    #endregion
}

#region Response DTOs

public class CategoryResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }
    public int SubCategoriesCount { get; set; }
    public int ItemsCount { get; set; }
    public bool HasChildren { get; set; }
    public string? FullPath { get; set; }
}

public class CategoryDetailResponseDto : CategoryResponseDto
{
    public List<CategoryResponseDto> SubCategories { get; set; } = new();
    public List<CategoryItemResponseDto> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CategoryItemResponseDto
{
    public Guid Id { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string UnitOfMeasure { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
}

public class CategoryHierarchyResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<CategoryHierarchyResponseDto> Children { get; set; } = new();
}

public class ItemMasterResponseDto
{
    public Guid Id { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string UnitOfMeasure { get; set; } = string.Empty;
    public bool RequiresInspection { get; set; }
    public int MinStockLevel { get; set; }
    public int TotalStock { get; set; }
    public int AvailableStock { get; set; }
    public bool IsLowStock => TotalStock <= MinStockLevel;
}

public class ItemMasterDetailResponseDto : ItemMasterResponseDto
{
    public List<ItemStockLocationResponseDto> StockLocations { get; set; } = new();
    public List<ItemMovementResponseDto> RecentMovements { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class ItemMasterListResponseDto
{
    public Guid Id { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string UnitOfMeasure { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
    public int ReservedStock { get; set; }
    public int AvailableStock { get; set; }
    public bool IsLowStock { get; set; }
}

public class ItemStockLocationResponseDto
{
    public Guid ShelfId { get; set; }
    public string ShelfLocation { get; set; } = string.Empty;
    public string WarehouseName { get; set; } = string.Empty;
    public int CurrentQuantity { get; set; }
    public int ReservedQuantity { get; set; }
    public int AvailableQuantity { get; set; }
}

public class ItemMovementResponseDto
{
    public DateTime Date { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public int QuantityChange { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string ShelfLocation { get; set; } = string.Empty;
}

public class LowStockItemResponseDto
{
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
    public int MinStockLevel { get; set; }
    public int Deficit { get; set; }
    public List<string> Locations { get; set; } = new();
}

public class PropertyResponseDto
{
    public Guid Id { get; set; }
    public string TagNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public Guid PropertyTypeId { get; set; }
    public string PropertyTypeName { get; set; } = string.Empty;
    public Guid? PropertyCategoryId { get; set; }
    public string PropertyCategoryName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public decimal TotalValue { get; set; }
    public int Quantity { get; set; }
    public DateTime PurchaseDate { get; set; }
    public Guid LocationId { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public Guid? SafetyBoxId { get; set; }
    public string? SafetyBoxNumber { get; set; }
    public int? ShelfNumber { get; set; }
    public string FormattedValue { get; set; } = string.Empty;
    public string FormattedPrice { get; set; } = string.Empty;
    public string FormattedPurchaseDate { get; set; } = string.Empty;
}

public class PropertyDetailResponseDto : PropertyResponseDto
{
    public List<PropertyAttachmentResponseDto> Attachments { get; set; } = new();
    public List<PropertyMovementResponseDto> MovementHistory { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

public class PropertyAttachmentResponseDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public DateTime UploadedAt { get; set; }
    public string DownloadUrl { get; set; } = string.Empty;
}

public class PropertyMovementResponseDto
{
    public DateTime Date { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string FromLocation { get; set; } = string.Empty;
    public string ToLocation { get; set; } = string.Empty;
    public string PerformedBy { get; set; } = string.Empty;
    public string Remarks { get; set; } = string.Empty;
}

public class PropertyTypeResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PropertiesCount { get; set; }
}

public class PropertyTypeDetailResponseDto : PropertyTypeResponseDto
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class PropertyCategoryResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PropertiesCount { get; set; }
}

public class PropertyCategoryDetailResponseDto : PropertyCategoryResponseDto
{
    public List<PropertyCategoryPropertyResponseDto> Properties { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class PropertyCategoryPropertyResponseDto
{
    public Guid Id { get; set; }
    public string TagNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string LocationName { get; set; } = string.Empty;
}

public class LocationResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LocationType { get; set; } = string.Empty;
    public int PropertiesCount { get; set; }
    public int SafetyBoxesCount { get; set; }
}

public class LocationDetailResponseDto : LocationResponseDto
{
    public List<LocationSafetyBoxResponseDto> SafetyBoxes { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class LocationSafetyBoxResponseDto
{
    public Guid Id { get; set; }
    public string BoxNumber { get; set; } = string.Empty;
    public int TotalShelves { get; set; }
    public int PropertiesCount { get; set; }
}

public class SafetyBoxResponseDto
{
    public Guid Id { get; set; }
    public string BoxNumber { get; set; } = string.Empty;
    public int TotalShelves { get; set; }
    public Guid LocationId { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public int OccupiedShelves { get; set; }
    public int PropertiesCount { get; set; }
}

public class SafetyBoxDetailResponseDto : SafetyBoxResponseDto
{
    public List<SafetyBoxShelfResponseDto> Shelves { get; set; } = new();
    public List<SafetyBoxPropertyResponseDto> Properties { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class SafetyBoxShelfResponseDto
{
    public Guid Id { get; set; }
    public int ShelfNumber { get; set; }
    public int PropertiesCount { get; set; }
}

public class SafetyBoxPropertyResponseDto
{
    public Guid Id { get; set; }
    public string TagNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int ShelfNumber { get; set; }
}

public class WarehouseResponseDto
{
    public Guid Id { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
    public string LocationCode { get; set; } = string.Empty;
    public int ShelfLocationsCount { get; set; }
}

public class WarehouseDetailResponseDto : WarehouseResponseDto
{
    public List<WarehouseShelfLocationResponseDto> ShelfLocations { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class WarehouseShelfLocationResponseDto
{
    public Guid Id { get; set; }
    public string Aisle { get; set; } = string.Empty;
    public string Rack { get; set; } = string.Empty;
    public string ShelfNumber { get; set; } = string.Empty;
    public string FullAddress { get; set; } = string.Empty;
    public int CurrentStockCount { get; set; }
}

public class ShelfLocationResponseDto
{
    public Guid Id { get; set; }
    public Guid WarehouseId { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
    public string Aisle { get; set; } = string.Empty;
    public string Rack { get; set; } = string.Empty;
    public string ShelfNumber { get; set; } = string.Empty;
    public string QRCodeValue { get; set; } = string.Empty;
    public string QRCodeUrl { get; set; } = string.Empty;
    public string FullAddress { get; set; } = string.Empty;
    public int CurrentStockCount { get; set; }
    public int ItemCount { get; set; }
}

public class ShelfLocationDetailResponseDto : ShelfLocationResponseDto
{
    public List<ShelfInventoryResponseDto> Inventory { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class ShelfInventoryResponseDto
{
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int CurrentQuantity { get; set; }
    public int ReservedQuantity { get; set; }
    public int AvailableQuantity { get; set; }
}

public class InventoryStockResponseDto
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public Guid ShelfId { get; set; }
    public string ShelfLocation { get; set; } = string.Empty;
    public int CurrentQuantity { get; set; }
    public int ReservedQuantity { get; set; }
    public int AvailableQuantity { get; set; }
}

public class InventoryStockDetailResponseDto : InventoryStockResponseDto
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<StockMovementResponseDto> MovementHistory { get; set; } = new();
}

public class StockLedgerResponseDto
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public Guid ShelfId { get; set; }
    public string ShelfLocation { get; set; } = string.Empty;
    public int QuantityChange { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public Guid ReferenceId { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class StockMovementResponseDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public int QuantityChange { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string ShelfLocation { get; set; } = string.Empty;
    public string PerformedBy { get; set; } = string.Empty;
}

public class SupplierResponseDto
{
    public Guid Id { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string TinNumber { get; set; } = string.Empty;
    public int ReceivingNotesCount { get; set; }
}

public class SupplierDetailResponseDto : SupplierResponseDto
{
    public List<SupplierReceivingNoteResponseDto> ReceivingNotes { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class SupplierReceivingNoteResponseDto
{
    public Guid Id { get; set; }
    public string GRNNumber { get; set; } = string.Empty;
    public DateTime ReceivedDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ReceivedBy { get; set; } = string.Empty;
}

public class ReceivingNoteResponseDto
{
    public Guid Id { get; set; }
    public string GRNNumber { get; set; } = string.Empty;
    public Guid SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public DateTime ReceivedDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public Guid ReceivedById { get; set; }
    public string ReceivedByName { get; set; } = string.Empty;
    public bool HasInspection { get; set; }
}

public class ReceivingNoteDetailResponseDto : ReceivingNoteResponseDto
{
    public InspectionInfoResponseDto? Inspection { get; set; }
    public List<ReceivingItemResponseDto> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class ReceivingItemResponseDto
{
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
}

public class InspectionInfoResponseDto
{
    public Guid Id { get; set; }
    public Guid InspectorId { get; set; }
    public string InspectorName { get; set; } = string.Empty;
    public bool IsPassed { get; set; }
    public string DeviationNotes { get; set; } = string.Empty;
    public DateTime InspectionDate { get; set; }
}

public class InspectionResponseDto
{
    public Guid Id { get; set; }
    public Guid ReceivingNoteId { get; set; }
    public string GRNNumber { get; set; } = string.Empty;
    public Guid InspectorId { get; set; }
    public string InspectorName { get; set; } = string.Empty;
    public bool IsPassed { get; set; }
    public string DeviationNotes { get; set; } = string.Empty;
    public DateTime InspectionDate { get; set; }
}

public class ServiceRequestResponseDto
{
    public Guid Id { get; set; }
    public string SRNumber { get; set; } = string.Empty;
    public Guid RequesterId { get; set; }
    public string RequesterName { get; set; } = string.Empty;
    public Guid? ApprovedById { get; set; }
    public string? ApprovedByName { get; set; }
    public DateTime RequestDate { get; set; }
    public string FormattedRequestDate { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int TotalItems { get; set; }
    public int TotalQuantity { get; set; }
    public int IssuedQuantity { get; set; }
}

public class ServiceRequestDetailResponseDto : ServiceRequestResponseDto
{
    public List<ServiceRequestItemResponseDto> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class ServiceRequestItemResponseDto
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string UnitOfMeasure { get; set; } = string.Empty;
    public int RequestedQty { get; set; }
    public int IssuedQty { get; set; }
    public int PendingQty => RequestedQty - IssuedQty;
    public Guid? ShelfId { get; set; }
    public string? ShelfLocation { get; set; }
}

public class StoreIssueVoucherResponseDto
{
    public Guid Id { get; set; }
    public string SIVNumber { get; set; } = string.Empty;
    public Guid SRId { get; set; }
    public string SRNumber { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public Guid IssuedById { get; set; }
    public string IssuedByName { get; set; } = string.Empty;
    public string RecipientSignature { get; set; } = string.Empty;
    public int TotalItems { get; set; }
    public int TotalQuantity { get; set; }
}

public class StoreIssueVoucherDetailResponseDto : StoreIssueVoucherResponseDto
{
    public List<SIVItemResponseDto> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class SIVItemResponseDto
{
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int RequestedQty { get; set; }
    public int IssuedQty { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
    public string? ShelfLocation { get; set; }
}

public class TransferRecordResponseDto
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public Guid FromLocationId { get; set; }
    public string FromLocationName { get; set; } = string.Empty;
    public Guid ToLocationId { get; set; }
    public string ToLocationName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime TransferDate { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class TransferRecordDetailResponseDto : TransferRecordResponseDto
{
    public string? Remarks { get; set; }
    public Guid? ApprovedById { get; set; }
    public string? ApprovedByName { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class ReturnMaterialRequestResponseDto
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public Guid? ApprovedById { get; set; }
    public string? ApprovedByName { get; set; }
    public DateTime? ApprovedDate { get; set; }
}

public class DisposalRecordResponseDto
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime DisposalDate { get; set; }
    public Guid DisposedBy { get; set; }
    public string DisposedByName { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal TotalValue { get; set; }
}

public class DisposalRecordDetailResponseDto : DisposalRecordResponseDto
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? ApprovedById { get; set; }
    public string? ApprovedByName { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovalRemarks { get; set; }
}

public class ApprovalWorkflowResponseDto
{
    public Guid Id { get; set; }
    public string WorkflowName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ApproversCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class ApprovalWorkflowDetailResponseDto : ApprovalWorkflowResponseDto
{
    public List<WorkflowApproverResponseDto> Approvers { get; set; } = new();
}

public class WorkflowApproverResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int ApprovalLevel { get; set; }
    public DateTime AssignedAt { get; set; }
}

public class ApproverResponseDto
{
    public Guid Id { get; set; }
    public Guid WorkflowId { get; set; }
    public string WorkflowName { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int ApprovalLevel { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ApprovalStatusResponseDto
{
    public Guid Id { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class EmployeeResponseDto
{
    public Guid Id { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public bool HasUserAccount { get; set; }
}

public class EmployeeDetailResponseDto : EmployeeResponseDto
{
    public UserAccountInfoResponseDto? UserAccount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class UserAccountInfoResponseDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class RoleResponseDto
{
    public Guid Id { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int UsersCount { get; set; }
}

public class UserInfoResponseDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string[] Roles { get; set; } = Array.Empty<string>();
    public string[] Permissions { get; set; } = Array.Empty<string>();
}

public class AuthResultResponseDto
{
    public bool Succeeded { get; set; }
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserInfoResponseDto User { get; set; } = new();
    public string[] Errors { get; set; } = Array.Empty<string>();
}

public class AuditTrailResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public DateTime ActionDate { get; set; }
    public string Details { get; set; } = string.Empty;
}

public class AuditTrailListResponseDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public DateTime ActionDate { get; set; }
}

public class NotificationResponseDto
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime SentDate { get; set; }
    public string TimeAgo { get; set; } = string.Empty;
}

public class NotificationListResponseDto
{
    public List<NotificationResponseDto> Notifications { get; set; } = new();
    public int TotalCount { get; set; }
    public int UnreadCount { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}

public class DocumentAttachmentResponseDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public Guid RelatedEntityId { get; set; }
    public string RelatedEntityName { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    public string UploadedBy { get; set; } = string.Empty;
    public string DownloadUrl { get; set; } = string.Empty;
}

public class PropertyValuationReportResponseDto
{
    public DateTime GeneratedAt { get; set; }
    public int TotalProperties { get; set; }
    public decimal TotalValue { get; set; }
    public decimal AverageValue { get; set; }
    public List<LocationValuationResponseDto> ByLocation { get; set; } = new();
    public List<TypeValuationResponseDto> ByType { get; set; } = new();
    public List<PropertySummaryResponseDto> RecentAcquisitions { get; set; } = new();
}

public class LocationValuationResponseDto
{
    public string LocationName { get; set; } = string.Empty;
    public int PropertyCount { get; set; }
    public decimal TotalValue { get; set; }
}

public class TypeValuationResponseDto
{
    public string TypeName { get; set; } = string.Empty;
    public int PropertyCount { get; set; }
    public decimal TotalValue { get; set; }
}

public class PropertySummaryResponseDto
{
    public string TagNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalValue { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

public class RequisitionHistoryReportResponseDto
{
    public DateTime GeneratedAt { get; set; }
    public int TotalRequisitions { get; set; }
    public List<StatusSummaryResponseDto> ByStatus { get; set; } = new();
    public List<RequisitionSummaryResponseDto> Requisitions { get; set; } = new();
}

public class StatusSummaryResponseDto
{
    public string Status { get; set; } = string.Empty;
    public int Count { get; set; }
    public int TotalItems { get; set; }
}

public class RequisitionSummaryResponseDto
{
    public Guid Id { get; set; }
    public string SRNumber { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public string Requester { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int ItemCount { get; set; }
    public int TotalQuantity { get; set; }
    public int IssuedQuantity { get; set; }
    public string? SIVNumber { get; set; }
    public DateTime? IssueDate { get; set; }
}

public class StockMovementReportResponseDto
{
    public DateTime GeneratedAt { get; set; }
    public int TotalMovements { get; set; }
    public List<MovementTypeSummaryResponseDto> ByType { get; set; } = new();
    public List<StockMovementDetailResponseDto> Movements { get; set; } = new();
}

public class MovementTypeSummaryResponseDto
{
    public string TransactionType { get; set; } = string.Empty;
    public int Count { get; set; }
    public int TotalQuantity { get; set; }
}

public class StockMovementDetailResponseDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string TransactionType { get; set; } = string.Empty;
    public int QuantityChange { get; set; }
    public string Warehouse { get; set; } = string.Empty;
    public string ShelfLocation { get; set; } = string.Empty;
    public Guid ReferenceId { get; set; }
}

public class DisposalReportResponseDto
{
    public DateTime GeneratedAt { get; set; }
    public int TotalDisposals { get; set; }
    public decimal TotalValue { get; set; }
    public List<DisposalSummaryResponseDto> ByReason { get; set; } = new();
    public List<DisposalDetailResponseDto> Disposals { get; set; } = new();
}

public class DisposalSummaryResponseDto
{
    public string Reason { get; set; } = string.Empty;
    public int Count { get; set; }
    public int TotalQuantity { get; set; }
    public decimal TotalValue { get; set; }
}

public class DisposalDetailResponseDto
{
    public Guid Id { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalValue { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime DisposalDate { get; set; }
    public string DisposedBy { get; set; } = string.Empty;
}

public class InventoryValuationReportResponseDto
{
    public DateTime GeneratedAt { get; set; }
    public int TotalItems { get; set; }
    public int TotalQuantity { get; set; }
    public decimal TotalValue { get; set; }
    public List<InventoryValuationItemResponseDto> Items { get; set; } = new();
    public List<InventoryValuationLocationResponseDto> ByLocation { get; set; } = new();
}

public class InventoryValuationItemResponseDto
{
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalValue { get; set; }
    public List<string> Locations { get; set; } = new();
}

public class InventoryValuationLocationResponseDto
{
    public string LocationName { get; set; } = string.Empty;
    public int ItemCount { get; set; }
    public int Quantity { get; set; }
    public decimal TotalValue { get; set; }
}

#endregion

#region Request DTOs

public class CreateCategoryRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? ParentCategoryId { get; set; }
}

public class UpdateCategoryRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? ParentCategoryId { get; set; }
}

public class CreateItemRequest
{
    public string SKU { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
    public bool RequiresInspection { get; set; }
    public int MinStockLevel { get; set; }
}

public class UpdateItemRequest
{
    public string SKU { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
    public bool RequiresInspection { get; set; }
    public int MinStockLevel { get; set; }
}

public class CreatePropertyRequest
{
    public string TagNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public Guid PropertyTypeId { get; set; }
    public Guid? PropertyCategoryId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public DateTime PurchaseDate { get; set; }
    public Guid LocationId { get; set; }
    public Guid? SafetyBoxId { get; set; }
    public int? ShelfNumber { get; set; }
}

public class UpdatePropertyRequest
{
    public string TagNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public Guid PropertyTypeId { get; set; }
    public Guid? PropertyCategoryId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public DateTime PurchaseDate { get; set; }
    public Guid LocationId { get; set; }
    public Guid? SafetyBoxId { get; set; }
    public int? ShelfNumber { get; set; }
}

public class TransferPropertyRequest
{
    public Guid NewLocationId { get; set; }
    public Guid? NewSafetyBoxId { get; set; }
    public int? NewShelfNumber { get; set; }
    public string Remarks { get; set; } = string.Empty;
}

public class CreateLocationRequest
{
    public string Name { get; set; } = string.Empty;
    public string LocationType { get; set; } = string.Empty;
}

public class UpdateLocationRequest
{
    public string Name { get; set; } = string.Empty;
    public string LocationType { get; set; } = string.Empty;
}

public class CreateSafetyBoxRequest
{
    public string BoxNumber { get; set; } = string.Empty;
    public int TotalShelves { get; set; }
    public Guid LocationId { get; set; }
}

public class UpdateSafetyBoxRequest
{
    public string BoxNumber { get; set; } = string.Empty;
    public int TotalShelves { get; set; }
    public Guid LocationId { get; set; }
}

public class CreatePropertyTypeRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class UpdatePropertyTypeRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class CreatePropertyCategoryRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class UpdatePropertyCategoryRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class CreateWarehouseRequest
{
    public string WarehouseName { get; set; } = string.Empty;
    public string LocationCode { get; set; } = string.Empty;
}

public class UpdateWarehouseRequest
{
    public string WarehouseName { get; set; } = string.Empty;
    public string LocationCode { get; set; } = string.Empty;
}

public class CreateShelfLocationRequest
{
    public Guid WarehouseId { get; set; }
    public string Aisle { get; set; } = string.Empty;
    public string Rack { get; set; } = string.Empty;
    public string ShelfNumber { get; set; } = string.Empty;
}

public class UpdateShelfLocationRequest
{
    public Guid WarehouseId { get; set; }
    public string Aisle { get; set; } = string.Empty;
    public string Rack { get; set; } = string.Empty;
    public string ShelfNumber { get; set; } = string.Empty;
}

public class CreateSupplierRequest
{
    public string SupplierName { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string TinNumber { get; set; } = string.Empty;
}

public class UpdateSupplierRequest
{
    public string SupplierName { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string TinNumber { get; set; } = string.Empty;
}

public class CreateReceivingNoteRequest
{
    public string GRNNumber { get; set; } = string.Empty;
    public Guid SupplierId { get; set; }
    public List<ReceivingNoteItemRequest> Items { get; set; } = new();
}

public class ReceivingNoteItemRequest
{
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
}

public class CreateInspectionRequest
{
    public Guid ReceivingNoteId { get; set; }
    public bool IsPassed { get; set; }
    public string DeviationNotes { get; set; } = string.Empty;
}

public class CreateServiceRequestRequest
{
    public List<ServiceRequestItemRequest> Items { get; set; } = new();
}

public class ServiceRequestItemRequest
{
    public Guid ItemId { get; set; }
    public int RequestedQty { get; set; }
    public Guid? ShelfId { get; set; }
}

public class CreateStoreIssueVoucherRequest
{
    public Guid SRId { get; set; }
    public string RecipientSignature { get; set; } = string.Empty;
    public List<IssueItemRequest> Items { get; set; } = new();
}

public class IssueItemRequest
{
    public Guid SRDetailId { get; set; }
    public int IssuedQty { get; set; }
    public Guid? ShelfId { get; set; }
}

public class CreateTransferRecordRequest
{
    public Guid ItemId { get; set; }
    public Guid FromLocationId { get; set; }
    public Guid ToLocationId { get; set; }
    public int Quantity { get; set; }
    public string Remarks { get; set; } = string.Empty;
}

public class CreateReturnRequestRequest
{
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class CreateDisposalRecordRequest
{
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class CreateWorkflowRequest
{
    public string WorkflowName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class UpdateWorkflowRequest
{
    public string WorkflowName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class AssignApproverRequest
{
    public Guid UserId { get; set; }
    public int ApprovalLevel { get; set; }
}

public class CreateEmployeeRequest
{
    public string EmployeeCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
}

public class UpdateEmployeeRequest
{
    public string FullName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
}

public class CreateUserRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Guid EmployeeId { get; set; }
    public Guid RoleId { get; set; }
    public bool IsActive { get; set; } = true;
}

#endregion