using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ProjectMangaSmurf.Data;
using System.Security.Claims;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

public class RBACAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public byte PermissionId { get; set; } // Change the type to byte

    private bool UserHasPermission(ProjectDBContext db, string userId, byte permissionId)
    {
        return db.StaffPermissionsDetails.Any(m => m.IdUser == userId && m.IdPermissions == permissionId && m.Active == true);
    }

    private bool IsPermissionActive(ProjectDBContext db, byte permissionId)
    {
        return db.PermissionsLists.Any(p => p.IdPermissions == permissionId && p.Active == true);
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;
        var dbContext = httpContext.RequestServices.GetRequiredService<ProjectDBContext>();

        if (!IsPermissionActive(dbContext, PermissionId))
        {
            RedirectToAccessDenied(context);
            return;
        }

        var userPermissions = httpContext.Session.GetString("UserPermissions");

        if (string.IsNullOrEmpty(userPermissions))
        {
            RedirectToLogin(context);
            return;
        }

        // Validate the permissions list
        var permissionsList = userPermissions
            .Split(',')
            .Select(p =>
            {
                byte.TryParse(p, out byte result);
                return result;
            })
            .ToList();

        if (permissionsList.Contains(PermissionId))
        {
            return;
        }

        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null && UserHasPermission(dbContext, userId, PermissionId))
        {
            return;
        }

        RedirectToAccessDenied(context);
    }

    private void RedirectToLogin(AuthorizationFilterContext context)
    {
        context.Result = new RedirectToRouteResult(new
        {
            area = "Admin",
            controller = "AdminLogin",
            action = "Login"
        });
    }

    private void RedirectToAccessDenied(AuthorizationFilterContext context)
    {
        context.Result = new RedirectToRouteResult(new
        {
            area = "Admin",
            controller = "AdminLogin",
            action = "AccessDenied"
        });
    }
}
