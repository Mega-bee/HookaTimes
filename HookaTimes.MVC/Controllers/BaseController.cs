using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace HookaTimes.MVC.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {


            int userBuddyId = 0;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity!.IsAuthenticated)
            {
                //userBuddyId = Convert.ToInt32(identity.FindFirst("BuddyID")!.Value);

                //string uid = User.FindFirst(ClaimTypes.)!.Value;
                userBuddyId = Convert.ToInt32(User.Claims.Where(x => x.Type == "BuddyID").FirstOrDefault()!.Value);
            }
            //var UID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //var profId = _context.AccProfiles.Where(x => x.UserId == UID).FirstOrDefault().Id;
            //var roleId = _context.AccProfiles.Where(x => x.UserId == UID).FirstOrDefault().RoleId;
            //var roleName = _context.AccProfileRoles.Where(x => x.Id == roleId).FirstOrDefault().RoleName;

            //ViewBag.roleId = roleId;
            //ViewBag.roleName = $"Dashboard | {roleName}";
            //ViewBag.Notifications = _context.Notifications.Where(x => x.ProfileId == profId && x.Seen == false).OrderByDescending(x => x.DateCreated.Value).ToList();
            base.OnActionExecuting(filterContext);
        }
    }
}
