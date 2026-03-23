namespace Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        Guid? UserGuid { get; }
        string? Username { get; }
        string? UserName { get; }
        string? EmployeeCode { get; }
        string? EmployeeName { get; }
        string? Role { get; }
        Guid? RoleId { get; }
        bool IsAuthenticated { get; }
        bool IsInRole(string role);
        bool HasPermission(string permission);
        IEnumerable<string> Permissions { get; }
    }
}