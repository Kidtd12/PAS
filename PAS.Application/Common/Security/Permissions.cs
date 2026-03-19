namespace Application.Common.Security;

public static class Permissions
{
    public static class Categories
    {
        public const string Create = "Categories.Create";
        public const string Edit = "Categories.Edit";
        public const string Delete = "Categories.Delete";
        public const string View = "Categories.View";
    }

    public static class Items
    {
        public const string Create = "Items.Create";
        public const string Edit = "Items.Edit";
        public const string Delete = "Items.Delete";
        public const string View = "Items.View";
        public const string ViewStock = "Items.ViewStock";
    }

    public static class Documents
    {
        public const string Upload = "Documents.Upload";
        public const string Download = "Documents.Download";
        public const string Delete = "Documents.Delete";
        public const string View = "Documents.View";
    }

    public static class Notifications
    {
        public const string View = "Notifications.View";
        public const string MarkAsRead = "Notifications.MarkAsRead";
    }

    public static class AuditTrail
    {
        public const string View = "AuditTrail.View";
    }

    public static class Properties
    {
        public const string Create = "Properties.Create";
        public const string Edit = "Properties.Edit";
        public const string View = "Properties.View";
        public const string ViewDetails = "Properties.ViewDetails";
        public const string Transfer = "Properties.Transfer";
    }

    public static class PropertyCategories
    {
        public const string Create = "PropertyCategories.Create";
        public const string Edit = "PropertyCategories.Edit";
        public const string Delete = "PropertyCategories.Delete";
        public const string View = "PropertyCategories.View";
    }

    public static class PropertyTypes
    {
        public const string Create = "PropertyTypes.Create";
        public const string Edit = "PropertyTypes.Edit";
        public const string Delete = "PropertyTypes.Delete";
        public const string View = "PropertyTypes.View";
    }

    public static class Locations
    {
        public const string Create = "Locations.Create";
        public const string Edit = "Locations.Edit";
        public const string Delete = "Locations.Delete";
        public const string View = "Locations.View";
    }

    public static class SafetyBoxes
    {
        public const string Create = "SafetyBoxes.Create";
        public const string Edit = "SafetyBoxes.Edit";
        public const string Delete = "SafetyBoxes.Delete";
        public const string View = "SafetyBoxes.View";
    }

    public static class Workflow
    {
        public const string Create = "Workflow.Create";
        public const string Edit = "Workflow.Edit";
        public const string Delete = "Workflow.Delete";
        public const string View = "Workflow.View";
        public const string Assign = "Workflow.Assign";
    }

    public static class Requisitions
    {
        public const string Create = "Requisitions.Create";
        public const string Approve = "Requisitions.Approve";
    }

    public static class Receiving
    {
        public const string Create = "Receiving.Create";
        public const string Inspect = "Receiving.Inspect";
    }

    public static class Reports
    {
        public const string View = "Reports.View";
    }

    public static class Dashboard
    {
        public const string View = "Dashboard.View";
    }
}
