using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TekConf.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Web;
using System.IO;

namespace TekConf.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private teckconfdbEntities db = new teckconfdbEntities();
        // GET: Dashboard
        public ActionResult Index()
        {
            DashboardViewModel dashboardViewModel = new DashboardViewModel();
            string userId = User.Identity.GetUserId<string>();
            List<long> attendee_event_user = db.Event_AspNetUsers.Where(u => u.user_id == userId).Select(u => u.event_id).ToList();
            dashboardViewModel.events = db.Event.Where(e => attendee_event_user.Contains(e.id) && e.time >= DateTime.Today).ToList();

            dashboardViewModel.user = db.AspNetUsers.Where(u => u.Id == userId).FirstOrDefault();
            return View(dashboardViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile([Bind(Include = "Id,Firstname,Lastname,Email,Address")]AspNetUsers user,
            HttpPostedFileBase Picture)
        {
            if (ModelState.IsValid)
            {
                AspNetUsers u = db.AspNetUsers.Find(user.Id);
                u.Firstname = user.Firstname;
                u.Lastname = user.Lastname;
                u.Email = user.Email;
                u.Address = user.Address;
                if (Picture != null)
                {
                    u.Picture = user.Id + Path.GetExtension(Picture.FileName);
                    string downloadFilePath = System.Web.HttpContext.Current.Server.MapPath("/Downloads");
                    string savedFileName = Path.Combine(downloadFilePath, user.Id + Path.GetExtension(Picture.FileName));
                    Picture.SaveAs(savedFileName);
                }

                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}