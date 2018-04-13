using cruzDDAC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cruzDDAC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;
                //	ApplicationDbContext context = new ApplicationDbContext();
                //	var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                //var s=	UserManager.GetRoles(user.GetUserId());
                ViewBag.displayMenu = "No";

                if (isAdminUser())
                {
                    ViewBag.displayMenu = "Yes";
                }
                return View();
            }
            else
            {
                ViewBag.Name = "Not Logged in";
            }


            return View();


        }

        public ActionResult Schedule()
        {
            return View();
        }
        public ActionResult Agent()
        {
            
            using (var context = new ApplicationDbContext())
            {
        

                var mypart = @"
             SELECT  AspNetUsers.Email As Email,AspNetUsers.PhoneNumber As Phone, AspNetUsers.Fullname As Fullname
 from AspNetUsers,AspNetUserRoles,AspNetRoles
  WHERE AspNetRoles.Name='Agent'
  AND AspNetRoles.Id=AspNetUserRoles.RoleId
  AND AspNetUsers.Id=AspNetUserRoles.UserId
  
 ";
               
                var result = context.Database.SqlQuery<UserAgent>(mypart).ToList();

                if (result == null)
                {

                    return View();
                }


                return View(result);
            }

        }

    }
}