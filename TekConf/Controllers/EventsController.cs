using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            return View(@event);
        }

        // GET: Events/Create
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
            return View(eventCreateViewModel);
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
