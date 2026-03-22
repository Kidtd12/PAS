namespace Application.Common.Security;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class AuthorizeAttribute : Attribute
{
    public string Roles { get; init; } = string.Empty;
    public string Policy { get; init; } = string.Empty;
    public string Permissions { get; init; } = string.Empty;
}
