namespace Application.Common.Security
{
    public static class Permissions
    {
        public static class Dashboard { public const string View = "Reports.Dashboard.View"; }

        public static class Categories
        {
            public const string View = "Catalog.Categories.View";
            public const string Create = "Catalog.Categories.Create";
            public const string Edit = "Catalog.Categories.Edit";
            public const string Delete = "Catalog.Categories.Delete";
        }

        public static class Items
        {
            public const string View = "Catalog.Items.View";
            public const string Create = "Catalog.Items.Create";
            public const string Edit = "Catalog.Items.Edit";
            public const string Delete = "Catalog.Items.Delete";
            public const string ViewStock = "Catalog.Items.ManageStockLevels";
        }

        public static class Documents
        {
            public const string View = "Document.View";
            public const string Upload = "Document.Upload";
            public const string Download = "Document.Download";
            public const string Delete = "Document.Delete";
        }

        public static class AuditTrail
        {
            public const string View = "Audit.View";
        }

        public static class Notifications
        {
            public const string View = "Notification.View";
            public const string Create = "Notification.Send";
            public const string MarkAsRead = "Notification.View";
        }

        public static class PropertyTypes
        {
            public const string View = "Property.Types.View";
            public const string Create = "Property.Types.Manage";
            public const string Edit = "Property.Types.Manage";
            public const string Delete = "Property.Types.Manage";
        }

        public static class Properties
        {
            public const string View = "Property.Properties.View";
            public const string ViewDetails = "Property.Properties.View";
            public const string Create = "Property.Properties.Create";
            public const string Edit = "Property.Properties.Edit";
            public const string Delete = "Property.Properties.Delete";
            public const string Transfer = "Property.Properties.Transfer";
        }

        public static class PropertyCategories
        {
            public const string View = "Property.Categories.View";
            public const string Create = "Property.Categories.Create";
            public const string Edit = "Property.Categories.Edit";
            public const string Delete = "Property.Categories.Delete";
        }

        public static class Locations
        {
            public const string View = "Property.Locations.View";
            public const string Create = "Property.Locations.Manage";
            public const string Edit = "Property.Locations.Manage";
            public const string Delete = "Property.Locations.Manage";
        }

        public static class SafetyBoxes
        {
            public const string View = "Property.SafetyBoxes.View";
            public const string Create = "Property.SafetyBoxes.Manage";
            public const string Edit = "Property.SafetyBoxes.Manage";
            public const string Delete = "Property.SafetyBoxes.Manage";
        }

        public static class Requisitions
        {
            public const string View = "Requisition.View";
            public const string Create = "Requisition.Create";
            public const string Edit = "Requisition.Edit";
            public const string Delete = "Requisition.Delete";
            public const string Approve = "Requisition.Approve";
            public const string Issue = "Requisition.Issue";
        }

        public static class Receiving
        {
            public const string View = "Receiving.View";
            public const string Create = "Receiving.Create";
            public const string Edit = "Receiving.Edit";
            public const string Delete = "Receiving.Delete";
            public const string Inspect = "Receiving.Inspection.Perform";
            public const string Process = "Receiving.Process";
        }

        public static class Suppliers
        {
            public const string View = "Receiving.Suppliers.View";
            public const string Create = "Receiving.Suppliers.Create";
            public const string Edit = "Receiving.Suppliers.Edit";
            public const string Delete = "Receiving.Suppliers.Delete";
        }

        public static class TransferReturn
        {
            public const string View = "Transfer.View";
            public const string Create = "Transfer.Create";
            public const string Approve = "Transfer.Approve";
        }

        public static class Disposal
        {
            public const string View = "Disposal.View";
            public const string Create = "Disposal.Create";
            public const string Approve = "Disposal.Approve";
            public const string Process = "Disposal.Process";
        }

        public static class Users
        {
            public const string View = "Users.View";
            public const string Create = "Users.Create";
            public const string Edit = "Users.Edit";
            public const string Delete = "Users.Delete";
        }

        public static class Roles
        {
            public const string View = "Users.Roles.View";
        }

        public static class Employees
        {
            public const string View = "Users.Employees.View";
            public const string Create = "Users.Employees.Manage";
            public const string Edit = "Users.Employees.Manage";
            public const string Delete = "Users.Employees.Manage";
        }

        public static class Warehouses
        {
            public const string View = "Storage.Warehouses.View";
            public const string Create = "Storage.Warehouses.Create";
            public const string Edit = "Storage.Warehouses.Edit";
            public const string Delete = "Storage.Warehouses.Delete";
        }

        public static class ShelfLocations
        {
            public const string View = "Storage.Shelves.View";
            public const string Create = "Storage.Shelves.Create";
            public const string Edit = "Storage.Shelves.Edit";
            public const string Delete = "Storage.Shelves.Delete";
        }

        public static class Inventory
        {
            public const string View = "Storage.Inventory.View";
            public const string Adjust = "Storage.Inventory.Adjust";
            public const string Reserve = "Storage.Inventory.Transfer";
            public const string Release = "Storage.Inventory.Transfer";
        }

        public static class StockLedger
        {
            public const string View = "Storage.Inventory.ViewLedger";
        }

        public static class Reports
        {
            public const string View = "Reports.View";
        }

        public static class Workflow
        {
            public const string View = "Workflow.View";
            public const string Create = "Workflow.Create";
            public const string Edit = "Workflow.Edit";
            public const string Delete = "Workflow.Delete";
            public const string Assign = "Workflow.Approvers.Manage";
        }
    }
}
