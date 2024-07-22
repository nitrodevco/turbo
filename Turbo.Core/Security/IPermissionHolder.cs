namespace Turbo.Core.Security;

public interface IPermissionHolder
{
    public bool HasPermission(string permission);
}