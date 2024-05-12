//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Mvc;
//using ProjectMangaSmurf.Data;
//using System.Security.Claims;

//public class RBACAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
//{
//    public int PermissionId { get; set; }

//    // This method uses the database to verify permissions when session data is not sufficient
//    private bool UserHasPermission(ProjectDBContext db, string userId, int permissionId)
//    {
//        return db.StaffPermissionsDetails.Any(m => m.IdUser == userId && m.IdPermissions == permissionId && m.Active == true);
//    }

//    public void OnAuthorization(AuthorizationFilterContext context)
//    {
//        var httpContext = context.HttpContext;

//        // First, attempt to get user permissions from the session
//        var userPermissions = httpContext.Session.GetString("IdNV");

//        if (string.IsNullOrEmpty(userPermissions))
//        {
//            RedirectToLogin(context);
//            return;
//        }

//        // Permissions in session are stored as comma-separated values
//        var permissionsList = userPermissions.Split(',').Select(int.Parse).ToList();

//        if (permissionsList.Contains(PermissionId))
//        {
//            return;  // User has the required permission from session data
//        }

//        // If session does not confirm permission, fallback to database verification
//        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
//        if (userId != null && UserHasPermission(httpContext.RequestServices.GetRequiredService<ProjectDBContext>(), userId, PermissionId))
//        {
//            return;  // User has the required permission from database check
//        }

//        // If neither session nor database confirms permission, deny access
//        RedirectToAccessDenied(context);
//    }

//    private void RedirectToLogin(AuthorizationFilterContext context)
//    {
//        context.Result = new RedirectToRouteResult(new
//        {
//            area = "Admin",
//            controller = "AdminLogin",
//            action = "Login"
//        });
//    }

//    private void RedirectToAccessDenied(AuthorizationFilterContext context)
//    {
//        context.Result = new RedirectToRouteResult(new
//        {
//            area = "Admin",
//            controller = "AdminLogin",
//            action = "AccessDenied"
//        });
//    }
//}
