using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QEMS.Models;

namespace QEMS.Controllers
{
    public class SituationsController : Controller
    {
        private ApplicationDbContext db;
        public SituationsController()
        {
            db = new ApplicationDbContext();
        }

        // GET: Situations
        public async Task<ActionResult> Index()
        {
            var situations5 = await db.Situations.Include(s => s.Person).Where(s=>s.Severity == 5).ToListAsync();
            var situations4 = await db.Situations.Include(s => s.Person).Where(s => s.Severity == 4).ToListAsync();
            var situations3 = await db.Situations.Include(s => s.Person).Where(s => s.Severity == 3).ToListAsync();
            var situations2 = await db.Situations.Include(s => s.Person).Where(s => s.Severity == 2).ToListAsync();
            var situations1 = await db.Situations.Include(s => s.Person).Where(s => s.Severity == 1).ToListAsync();
            var situations = situations5.Concat(situations4).Concat(situations3).Concat(situations2).Concat(situations1);
            return View(situations);
        }

        public async Task<ActionResult> PersonsSituations()
        {
            var appid = StaticMethods.GetAppId();
            Person person = await db.People.Include(p => p.ApplicationUser).Where(p => p.ApplicationId == appid).FirstOrDefaultAsync();
            var situations = await db.Situations.Include(s => s.Person).Where(s=>s.PersonId == person.PersonId).ToListAsync();
            return View(situations);
        }

        // GET: Situations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Situation situation = await db.Situations.FindAsync(id);
            if (situation == null)
            {
                return HttpNotFound();
            }
            return View(situation);
        }

        // GET: Situations/Create
        public ActionResult Create()
        {
            Situation situation = new Situation();
            return View(situation);
        }

        // POST: Situations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Message,Time,Date,Severity,")] Situation situation)
        {
            var appid = StaticMethods.GetAppId();
            Person person = db.People.Include(p => p.ApplicationUser).Where(p=>p.ApplicationId == appid).FirstOrDefault();
            if (ModelState.IsValid)
            {
                situation.PersonId = person.PersonId;
                db.Situations.Add(situation);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            //ViewBag.PersonId = new SelectList(db.People, "PersonId", "FirstName", situation.PersonId);
            return View(situation);
        }

        // GET: Situations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Situation situation = await db.Situations.FindAsync(id);
            if (situation == null)
            {
                return HttpNotFound();
            }
            //ViewBag.PersonId = new SelectList(db.People, "PersonId", "FirstName", situation.PersonId);
            return View(situation);
        }

        // POST: Situations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Message,Time,Date,Severity,CallPoliceStation,CallFireStation,CallAmbulance,InProcess,Complete")] Situation situation, int id)
        {
            if (ModelState.IsValid)
            {
                db.Entry(situation).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //ViewBag.PersonId = new SelectList(db.People, "PersonId", "FirstName", situation.PersonId);
            return View(situation);
        }

        // GET: Situations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Situation situation = await db.Situations.FindAsync(id);
            if (situation == null)
            {
                return HttpNotFound();
            }
            return View(situation);
        }

        // POST: Situations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Situation situation = await db.Situations.FindAsync(id);
            db.Situations.Remove(situation);
            await db.SaveChangesAsync();
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
