using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SyncPendingModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DisposalRecords_UserLogins_DisposedBy",
                table: "DisposalRecords");

            migrationBuilder.DropIndex(
                name: "IX_StoreIssueVouchers_SRId",
                table: "StoreIssueVouchers");

            migrationBuilder.RenameColumn(
                name: "DisposedBy",
                table: "DisposalRecords",
                newName: "DisposedById");

            migrationBuilder.RenameIndex(
                name: "IX_DisposalRecords_DisposedBy",
                table: "DisposalRecords",
                newName: "IX_DisposalRecords_DisposedById");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Warehouses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "UserLogins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedById",
                table: "TransferRecords",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "TransferRecords",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BatchNumber",
                table: "TransferRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "TransferRecords",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FromShelfId",
                table: "TransferRecords",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InitiatedById",
                table: "TransferRecords",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "TransferRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "TransferRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "TransferRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "TransferRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ToShelfId",
                table: "TransferRecords",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransferNumber",
                table: "TransferRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipientName",
                table: "StoreIssueVouchers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "StoreIssueVouchers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "StoreIssueVouchers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BatchNumber",
                table: "StockLedgers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InventoryStockId",
                table: "StockLedgers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReceivingNoteId",
                table: "StockLedgers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ItemMasterId",
                table: "SR_Details",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BinType",
                table: "ShelfLocations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "ShelfLocations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ShelfLocations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Zone",
                table: "ShelfLocations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedById",
                table: "ReturnMaterialRequestNotes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RequestedById",
                table: "ReturnMaterialRequestNotes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReturnNumber",
                table: "ReturnMaterialRequestNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReturnType",
                table: "ReturnMaterialRequestNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SourceLocationId",
                table: "ReturnMaterialRequestNotes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SourceShelfId",
                table: "ReturnMaterialRequestNotes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ReturnMaterialRequestNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "ReturnMaterialRequestNotes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNoteNumber",
                table: "ReceivingNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverName",
                table: "ReceivingNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InvoiceDate",
                table: "ReceivingNotes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                table: "ReceivingNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PONumber",
                table: "ReceivingNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "ReceivingNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleNumber",
                table: "ReceivingNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PropertyCategoryId",
                table: "Properties",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SafetyBoxShelfId",
                table: "Properties",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "ItemMasters",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "BatchNumber",
                table: "InventoryStocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "InventoryStocks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ItemMasterId",
                table: "InventoryStocks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShelfLocationId",
                table: "InventoryStocks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PropertyId",
                table: "DocumentAttachments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReceivingNoteId",
                table: "DocumentAttachments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedById",
                table: "DisposalRecords",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "DisposalRecords",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisposalMethod",
                table: "DisposalRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EstimatedValue",
                table: "DisposalRecords",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "DisposalRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReceivingNoteItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceivingNoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingNoteItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceivingNoteItems_ItemMasters_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ItemMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceivingNoteItems_ReceivingNotes_ReceivingNoteId",
                        column: x => x.ReceivingNoteId,
                        principalTable: "ReceivingNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferRecords_ApprovedById",
                table: "TransferRecords",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_TransferRecords_FromShelfId",
                table: "TransferRecords",
                column: "FromShelfId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferRecords_InitiatedById",
                table: "TransferRecords",
                column: "InitiatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TransferRecords_ToShelfId",
                table: "TransferRecords",
                column: "ToShelfId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreIssueVouchers_SRId",
                table: "StoreIssueVouchers",
                column: "SRId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockLedgers_InventoryStockId",
                table: "StockLedgers",
                column: "InventoryStockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockLedgers_ReceivingNoteId",
                table: "StockLedgers",
                column: "ReceivingNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_SR_Details_ItemMasterId",
                table: "SR_Details",
                column: "ItemMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnMaterialRequestNotes_ApprovedById",
                table: "ReturnMaterialRequestNotes",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnMaterialRequestNotes_RequestedById",
                table: "ReturnMaterialRequestNotes",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnMaterialRequestNotes_SourceLocationId",
                table: "ReturnMaterialRequestNotes",
                column: "SourceLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnMaterialRequestNotes_SourceShelfId",
                table: "ReturnMaterialRequestNotes",
                column: "SourceShelfId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnMaterialRequestNotes_SupplierId",
                table: "ReturnMaterialRequestNotes",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyCategoryId",
                table: "Properties",
                column: "PropertyCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_SafetyBoxShelfId",
                table: "Properties",
                column: "SafetyBoxShelfId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryStocks_ItemMasterId",
                table: "InventoryStocks",
                column: "ItemMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryStocks_ShelfLocationId",
                table: "InventoryStocks",
                column: "ShelfLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentAttachments_PropertyId",
                table: "DocumentAttachments",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentAttachments_ReceivingNoteId",
                table: "DocumentAttachments",
                column: "ReceivingNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_DisposalRecords_ApprovedById",
                table: "DisposalRecords",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingNoteItems_ItemId",
                table: "ReceivingNoteItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingNoteItems_ReceivingNoteId_ItemId",
                table: "ReceivingNoteItems",
                columns: new[] { "ReceivingNoteId", "ItemId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DisposalRecords_UserLogins_ApprovedById",
                table: "DisposalRecords",
                column: "ApprovedById",
                principalTable: "UserLogins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DisposalRecords_UserLogins_DisposedById",
                table: "DisposalRecords",
                column: "DisposedById",
                principalTable: "UserLogins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentAttachments_Properties_PropertyId",
                table: "DocumentAttachments",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentAttachments_ReceivingNotes_ReceivingNoteId",
                table: "DocumentAttachments",
                column: "ReceivingNoteId",
                principalTable: "ReceivingNotes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryStocks_ItemMasters_ItemMasterId",
                table: "InventoryStocks",
                column: "ItemMasterId",
                principalTable: "ItemMasters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryStocks_ShelfLocations_ShelfLocationId",
                table: "InventoryStocks",
                column: "ShelfLocationId",
                principalTable: "ShelfLocations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertyCategories_PropertyCategoryId",
                table: "Properties",
                column: "PropertyCategoryId",
                principalTable: "PropertyCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_SafetyBoxShelves_SafetyBoxShelfId",
                table: "Properties",
                column: "SafetyBoxShelfId",
                principalTable: "SafetyBoxShelves",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnMaterialRequestNotes_PropertyLocations_SourceLocationId",
                table: "ReturnMaterialRequestNotes",
                column: "SourceLocationId",
                principalTable: "PropertyLocations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnMaterialRequestNotes_ShelfLocations_SourceShelfId",
                table: "ReturnMaterialRequestNotes",
                column: "SourceShelfId",
                principalTable: "ShelfLocations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnMaterialRequestNotes_Suppliers_SupplierId",
                table: "ReturnMaterialRequestNotes",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnMaterialRequestNotes_UserLogins_ApprovedById",
                table: "ReturnMaterialRequestNotes",
                column: "ApprovedById",
                principalTable: "UserLogins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnMaterialRequestNotes_UserLogins_RequestedById",
                table: "ReturnMaterialRequestNotes",
                column: "RequestedById",
                principalTable: "UserLogins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SR_Details_ItemMasters_ItemMasterId",
                table: "SR_Details",
                column: "ItemMasterId",
                principalTable: "ItemMasters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockLedgers_InventoryStocks_InventoryStockId",
                table: "StockLedgers",
                column: "InventoryStockId",
                principalTable: "InventoryStocks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockLedgers_ReceivingNotes_ReceivingNoteId",
                table: "StockLedgers",
                column: "ReceivingNoteId",
                principalTable: "ReceivingNotes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransferRecords_ShelfLocations_FromShelfId",
                table: "TransferRecords",
                column: "FromShelfId",
                principalTable: "ShelfLocations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransferRecords_ShelfLocations_ToShelfId",
                table: "TransferRecords",
                column: "ToShelfId",
                principalTable: "ShelfLocations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransferRecords_UserLogins_ApprovedById",
                table: "TransferRecords",
                column: "ApprovedById",
                principalTable: "UserLogins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransferRecords_UserLogins_InitiatedById",
                table: "TransferRecords",
                column: "InitiatedById",
                principalTable: "UserLogins",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DisposalRecords_UserLogins_ApprovedById",
                table: "DisposalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_DisposalRecords_UserLogins_DisposedById",
                table: "DisposalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentAttachments_Properties_PropertyId",
                table: "DocumentAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentAttachments_ReceivingNotes_ReceivingNoteId",
                table: "DocumentAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryStocks_ItemMasters_ItemMasterId",
                table: "InventoryStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryStocks_ShelfLocations_ShelfLocationId",
                table: "InventoryStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyCategories_PropertyCategoryId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_SafetyBoxShelves_SafetyBoxShelfId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnMaterialRequestNotes_PropertyLocations_SourceLocationId",
                table: "ReturnMaterialRequestNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnMaterialRequestNotes_ShelfLocations_SourceShelfId",
                table: "ReturnMaterialRequestNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnMaterialRequestNotes_Suppliers_SupplierId",
                table: "ReturnMaterialRequestNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnMaterialRequestNotes_UserLogins_ApprovedById",
                table: "ReturnMaterialRequestNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnMaterialRequestNotes_UserLogins_RequestedById",
                table: "ReturnMaterialRequestNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_SR_Details_ItemMasters_ItemMasterId",
                table: "SR_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_StockLedgers_InventoryStocks_InventoryStockId",
                table: "StockLedgers");

            migrationBuilder.DropForeignKey(
                name: "FK_StockLedgers_ReceivingNotes_ReceivingNoteId",
                table: "StockLedgers");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferRecords_ShelfLocations_FromShelfId",
                table: "TransferRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferRecords_ShelfLocations_ToShelfId",
                table: "TransferRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferRecords_UserLogins_ApprovedById",
                table: "TransferRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferRecords_UserLogins_InitiatedById",
                table: "TransferRecords");

            migrationBuilder.DropTable(
                name: "ReceivingNoteItems");

            migrationBuilder.DropIndex(
                name: "IX_TransferRecords_ApprovedById",
                table: "TransferRecords");

            migrationBuilder.DropIndex(
                name: "IX_TransferRecords_FromShelfId",
                table: "TransferRecords");

            migrationBuilder.DropIndex(
                name: "IX_TransferRecords_InitiatedById",
                table: "TransferRecords");

            migrationBuilder.DropIndex(
                name: "IX_TransferRecords_ToShelfId",
                table: "TransferRecords");

            migrationBuilder.DropIndex(
                name: "IX_StoreIssueVouchers_SRId",
                table: "StoreIssueVouchers");

            migrationBuilder.DropIndex(
                name: "IX_StockLedgers_InventoryStockId",
                table: "StockLedgers");

            migrationBuilder.DropIndex(
                name: "IX_StockLedgers_ReceivingNoteId",
                table: "StockLedgers");

            migrationBuilder.DropIndex(
                name: "IX_SR_Details_ItemMasterId",
                table: "SR_Details");

            migrationBuilder.DropIndex(
                name: "IX_ReturnMaterialRequestNotes_ApprovedById",
                table: "ReturnMaterialRequestNotes");

            migrationBuilder.DropIndex(
                name: "IX_ReturnMaterialRequestNotes_RequestedById",
                table: "ReturnMaterialRequestNotes");

            migrationBuilder.DropIndex(
                name: "IX_ReturnMaterialRequestNotes_SourceLocationId",
                table: "ReturnMaterialRequestNotes");

            migrationBuilder.DropIndex(
                name: "IX_ReturnMaterialRequestNotes_SourceShelfId",
                table: "ReturnMaterialRequestNotes");

            migrationBuilder.DropIndex(
                name: "IX_ReturnMaterialRequestNotes_SupplierId",
                table: "ReturnMaterialRequestNotes");

            migrationBuilder.DropIndex(
                name: "IX_Properties_PropertyCategoryId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_SafetyBoxShelfId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_InventoryStocks_ItemMasterId",
                table: "InventoryStocks");

            migrationBuilder.DropIndex(
                name: "IX_InventoryStocks_ShelfLocationId",
                table: "InventoryStocks");

            migrationBuilder.DropIndex(
                name: "IX_DocumentAttachments_PropertyId",
                table: "DocumentAttachments");

            migrationBuilder.DropIndex(
                name: "IX_DocumentAttachments_ReceivingNoteId",
                table: "DocumentAttachments");

            migrationBuilder.DropIndex(
                name: "IX_DisposalRecords_ApprovedById",
                table: "DisposalRecords");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "UserLogins");

            migrationBuilder.DropColumn(
                name: "ApprovedById",
                table: "TransferRecords");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "TransferRecords");

            migrationBuilder.DropColumn(
                name: "BatchNumber",
                table: "TransferRecords");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "TransferRecords");

            migrationBuilder.DropColumn(
                name: "FromShelfId",
                table: "TransferRecords");

            migrationBuilder.DropColumn(
                name: "InitiatedById",
                table: "TransferRecords");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "TransferRecords");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "TransferRecords");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "TransferRecords");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TransferRecords");

            migrationBuilder.DropColumn(
                name: "ToShelfId",
                table: "TransferRecords");

            migrationBuilder.DropColumn(
                name: "TransferNumber",
                table: "TransferRecords");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "RecipientName",
                table: "StoreIssueVouchers");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "StoreIssueVouchers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StoreIssueVouchers");

            migrationBuilder.DropColumn(
                name: "BatchNumber",
                table: "StockLedgers");

            migrationBuilder.DropColumn(
                name: "InventoryStockId",
                table: "StockLedgers");

            migrationBuilder.DropColumn(
                name: "ReceivingNoteId",
                table: "StockLedgers");

            migrationBuilder.DropColumn(
                name: "ItemMasterId",
                table: "SR_Details");

            migrationBuilder.DropColumn(
                name: "BinType",
                table: "ShelfLocations");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "ShelfLocations");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ShelfLocations");

            migrationBuilder.DropColumn(
                name: "Zone",
                table: "ShelfLocations");

            migrationBuilder.DropColumn(
                name: "ApprovedById",
                table: "ReturnMaterialRequestNotes");

            migrationBuilder.DropColumn(
                name: "RequestedById",
                table: "ReturnMaterialRequestNotes");

            migrationBuilder.DropColumn(
                name: "ReturnNumber",
                table: "ReturnMaterialRequestNotes");

            migrationBuilder.DropColumn(
                name: "ReturnType",
                table: "ReturnMaterialRequestNotes");

            migrationBuilder.DropColumn(
                name: "SourceLocationId",
                table: "ReturnMaterialRequestNotes");

            migrationBuilder.DropColumn(
                name: "SourceShelfId",
                table: "ReturnMaterialRequestNotes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ReturnMaterialRequestNotes");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "ReturnMaterialRequestNotes");

            migrationBuilder.DropColumn(
                name: "DeliveryNoteNumber",
                table: "ReceivingNotes");

            migrationBuilder.DropColumn(
                name: "DriverName",
                table: "ReceivingNotes");

            migrationBuilder.DropColumn(
                name: "InvoiceDate",
                table: "ReceivingNotes");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "ReceivingNotes");

            migrationBuilder.DropColumn(
                name: "PONumber",
                table: "ReceivingNotes");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "ReceivingNotes");

            migrationBuilder.DropColumn(
                name: "VehicleNumber",
                table: "ReceivingNotes");

            migrationBuilder.DropColumn(
                name: "PropertyCategoryId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "SafetyBoxShelfId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "ItemMasters");

            migrationBuilder.DropColumn(
                name: "BatchNumber",
                table: "InventoryStocks");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "InventoryStocks");

            migrationBuilder.DropColumn(
                name: "ItemMasterId",
                table: "InventoryStocks");

            migrationBuilder.DropColumn(
                name: "ShelfLocationId",
                table: "InventoryStocks");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "DocumentAttachments");

            migrationBuilder.DropColumn(
                name: "ReceivingNoteId",
                table: "DocumentAttachments");

            migrationBuilder.DropColumn(
                name: "ApprovedById",
                table: "DisposalRecords");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "DisposalRecords");

            migrationBuilder.DropColumn(
                name: "DisposalMethod",
                table: "DisposalRecords");

            migrationBuilder.DropColumn(
                name: "EstimatedValue",
                table: "DisposalRecords");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "DisposalRecords");

            migrationBuilder.RenameColumn(
                name: "DisposedById",
                table: "DisposalRecords",
                newName: "DisposedBy");

            migrationBuilder.RenameIndex(
                name: "IX_DisposalRecords_DisposedById",
                table: "DisposalRecords",
                newName: "IX_DisposalRecords_DisposedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StoreIssueVouchers_SRId",
                table: "StoreIssueVouchers",
                column: "SRId");

            migrationBuilder.AddForeignKey(
                name: "FK_DisposalRecords_UserLogins_DisposedBy",
                table: "DisposalRecords",
                column: "DisposedBy",
                principalTable: "UserLogins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
