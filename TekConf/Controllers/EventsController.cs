using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TekConf;
using TekConf.Models;

namespace TekConf.Controllers
{
    public class EventsController : Controller
    {
        private teckconfdbEntities db = new teckconfdbEntities();

        // GET: Events
        public ActionResult Index()
        {
            return View(db.Event.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Event.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            EventDetailsViewModel eventDetailsViewModel = new EventDetailsViewModel();
            eventDetailsViewModel.Event = @event;
            List<string> speaker_event_user = @event.Event_AspNetUsers.Where(u => u.type == "speaker").Select(u => u.user_id).ToList();
            List<string> attendee_event_user = @event.Event_AspNetUsers.Where(u => u.type == "attendee").Select(u => u.user_id).ToList();

            eventDetailsViewModel.speakers = db.AspNetUsers.Where(u => speaker_event_user.Contains(u.Id)).ToList();
            eventDetailsViewModel.attendees = db.AspNetUsers.Where(u => attendee_event_user.Contains(u.Id)).ToList();

            eventDetailsViewModel.technologies = @event.Technology.ToList();
            Regex youtubeRegex = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)");
            Regex vimeoRegex = new Regex(@"vimeo\.com/(?:.*#|.*/videos/)?([0-9]+)");
            Match youtubeMatch = youtubeRegex.Match(@event.video_link);
            Match vimeoMatch = vimeoRegex.Match(@event.video_link);
            string videoId = "";
            string videoLink = "";

            if (youtubeMatch.Success)
            {
                videoId = youtubeMatch.Groups[1].Value;
                videoLink = "https://www.youtube.com/embed/" + videoId;
            }
            if (vimeoMatch.Success)
            {
                videoId = vimeoMatch.Groups[1].Value;
                videoLink = "https://www.player.vimeo.com/video/" + videoId;
            }

            ViewBag.videoLink = videoLink;
            return View(eventDetailsViewModel);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            EventCreateViewModel eventCreateViewModel = new EventCreateViewModel();
            eventCreateViewModel.Event = new Event();
            eventCreateViewModel.technologies = DataAccess.Technology.GetTechnologiesList();
            return View(eventCreateViewModel);
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,description,location,time,event_link,video_link")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Event.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Event.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,description,location,time,event_link,video_link")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Event.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Event @event = db.Event.Find(id);
            db.Event.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
