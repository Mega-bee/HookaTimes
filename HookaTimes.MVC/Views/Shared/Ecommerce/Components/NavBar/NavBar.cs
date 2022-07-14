using HookaTimes.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HookaTimes.MVC.Views.Shared.Ecommerce.Components.NavBar
{
    public class NavBarViewComponent : ViewComponent
    {

        // private static Random random = new Random();
        //public static CaterMeDbContext _context;

        //public adminDashViewComponent(CaterMeDbContext context)
        //{
        //    _context = context;
        //}


        //private DbContextOptions<CaterMeDbContext> db = new DbContextOptions<CaterMeDbContext>();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //string UserName = User.Identity.Name;
            //var ss = _context.AspNetUsers.Where(x => x.Email == UserName).FirstOrDefault();
            //string USerID = ss.Id;
            //LogInAssets_VM mc = await _context.AccProfiles.Where(x => x.UserId == USerID).Select(x => new LogInAssets_VM
            //{
            //    ProfileID = x.Id,
            //    UserID = x.UserId,
            //    UserName = x.Email,
            //    UserRole = x.Role.RoleName,
            //    UserRoleID = x.RoleId,
            //}).FirstOrDefaultAsync();
            //ViewBag.Email = ss.Email;
            //return View(mc);
            return View();
        }
    }
}