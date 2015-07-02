using Newtonsoft.Json;
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
using Microsoft.AspNet.Identity;

namespace TekConf.Controllers
{
    public class EventsController : Controller
    {
        private teckconfdbEntities db = new teckconfdbEntities();

        // GET: Events
        public ActionResult Index()
        {
            ViewBag.technologies = DataAccess.Technology.GetTechnologiesList();
            return View();
        }

        // POST: Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(string Near, int? Month, int? Year, long[] technologies, string Topic)
        {
            IQueryable<Event> queryable = db.Event.Where(e => e.time > System.DateTime.Now);
            if (Month != null)
                queryable = queryable.Where(e => e.time.Month == Month);
            if (Year != null)
                queryable = queryable.Where(e => e.time.Year == Year);
            if (!String.IsNullOrEmpty(Topic))
                queryable = queryable.Where(e => e.name.ToLower().Contains(Topic));
            if (technologies != null)
                queryable = queryable.Where(e => technologies.Any(t => e.Technology.Select(et => et.id).Contains(t)));
            var list = queryable.Select(ev => new
            {
                ev.id,
                ev.name,
                ev.time,
                ev.description,
                ev.location
            }).ToList();
            return Content(JsonConvert.SerializeObject(list), "application/json");
        }
        // POST : Events/Details/5 (to participate to an event)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Participate(long? id)
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
            string currentUserId = User.Identity.GetUserId();
            if (currentUserId == null)
            {
                return RedirectToAction("Index");
            }
            Event_AspNetUsers event_AspNetUsers = db.Event_AspNetUsers.Where(u => u.event_id == @event.id && u.user_id == currentUserId).FirstOrDefault();
            if (event_AspNetUsers == null)
            {
                event_AspNetUsers = new Event_AspNetUsers()
                {
                    event_id = @event.id,
                    type = "attendee",
                    user_id = currentUserId
                };
                db.Event_AspNetUsers.Add(event_AspNetUsers);
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = id });
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
            eventDetailsViewModel.googleCalendarLink = "http://www.google.com/calendar/event?action=TEMPLATE&text=" +
                HttpUtility.UrlEncode(@event.name) + "&dates=" + @event.time.ToString("yyyyMMddTHHmmss") + "/" + @event.time.AddHours(1).ToString("yyyyMMddTHHmmss") + "&location=" + HttpUtility.UrlEncode(@event.location) + "&details=" + HttpUtility.UrlEncode(@event.description) + "&sprop=website:tekconf.fr&sprop=name:TekConf";
            ViewBag.videoLink = videoLink;
            return View(eventDetailsViewModel);
        }

        // GET: Events/Create
        [Authorize]
        public ActionResult Create()
        {
            EventCreateViewModel eventCreateViewModel = new EventCreateViewModel();
            eventCreateViewModel.Event = new Event();
            eventCreateViewModel.technologies = DataAccess.Technology.GetTechnologiesList();
            eventCreateViewModel.speakers = DataAccess.AspNetUsers.GetUsersList();
            return View(eventCreateViewModel);
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Event @event, long[] technologies, string[] speakers)
        {
            if (technologies == null)
                technologies = new long[] { };
            if (speakers == null)
                speakers = new string[] { };
            if (ModelState.IsValid)
            {
                @event.Technology = db.Technology.Where(t => technologies.Contains(t.id)).ToList();
                db.Event.Add(@event);
                db.SaveChanges();
                @event.Event_AspNetUsers = speakers.Select(s => new Event_AspNetUsers { user_id = s, event_id = @event.id, type = "speaker" }).ToList();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            EventCreateViewModel eventCreateViewModel = new EventCreateViewModel();
            eventCreateViewModel.Event = @event;
            eventCreateViewModel.technologies = DataAccess.Technology.GetTechnologiesList();
            eventCreateViewModel.speakers = DataAccess.AspNetUsers.GetUsersList();
            return View(eventCreateViewModel);
        }

        // GET: Events/Edit/5
        [Authorize]
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
            string currentUserId = User.Identity.GetUserId();
            Event_AspNetUsers event_AspNetUsers = db.Event_AspNetUsers.Where(u => u.event_id == @event.id && u.user_id == currentUserId).FirstOrDefault();
            if (event_AspNetUsers == null)
            {
                return RedirectToAction("Index");
            }
            EventEditViewModel eventEditViewModel = new EventEditViewModel();
            eventEditViewModel.Event = @event;
            eventEditViewModel.selectedTechnologies = @event.Technology.ToList();
            eventEditViewModel.technologies = DataAccess.Technology.GetTechnologiesList().Where(t => !@event.Technology.Select(e => e.id).Contains(t.id)).ToList();
            List<string> speaker_event_user = @event.Event_AspNetUsers.Where(u => u.type == "speaker").Select(u => u.user_id).ToList();
            eventEditViewModel.selectedSpeakers = db.AspNetUsers.Where(u => speaker_event_user.Contains(u.Id)).ToList();
            eventEditViewModel.speakers = DataAccess.AspNetUsers.GetUsersList().Where(u => !speaker_event_user.Contains(u.Id)).ToList();
            return View(eventEditViewModel);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Event @event, long[] technologies, string[] speakers)
        {
            if (technologies == null)
                technologies = new long[] { };
            if (speakers == null)
                speakers = new string[] { };

            db.Event.Attach(@event);
            db.Entry(@event).Collection("Technology").Load();
            db.Entry(@event).Collection("Event_AspNetUsers").Load();
            @event.Technology = db.Technology.Where(t => technologies.Contains(t.id)).ToList();
            @event.Event_AspNetUsers = speakers.Select(s => new Event_AspNetUsers { user_id = s, event_id = @event.id, type = "speaker" }).ToList();
            
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            EventEditViewModel eventEditViewModel = new EventEditViewModel();
            eventEditViewModel.Event = @event;
            eventEditViewModel.selectedTechnologies = @event.Technology.ToList();
            eventEditViewModel.technologies = DataAccess.Technology.GetTechnologiesList().Where(t => !@event.Technology.Select(e => e.id).Contains(t.id)).ToList();
            List<string> speaker_event_user = @event.Event_AspNetUsers.Where(u => u.type == "speaker").Select(u => u.user_id).ToList();
            eventEditViewModel.selectedSpeakers = db.AspNetUsers.Where(u => speaker_event_user.Contains(u.Id)).ToList();
            eventEditViewModel.speakers = DataAccess.AspNetUsers.GetUsersList().Where(u => !speaker_event_user.Contains(u.Id)).ToList();
            return View(eventEditViewModel);
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
