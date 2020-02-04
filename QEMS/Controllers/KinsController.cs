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
    public class KinsController : Controller
    {
        public ApplicationDbContext db;
        public KinsController()
        {
            db = new ApplicationDbContext();
        }

        // GET: Kins
        public async Task<ActionResult> Index()
        {
            var kins = db.Kins.Include(k => k.Person);
            return View(await kins.ToListAsync());
        }

        public async Task<ActionResult> ListOfKinForThePersonToSee()
        {
            var appid = StaticMethods.GetAppId();
            var personid = await db.People.Include(p => p.ApplicationUser).Where(p => p.ApplicationId == appid).Select(p => p.PersonId).FirstOrDefaultAsync();
            var kins = await db.Kins.Include(k => k.Person).Where(k=>k.PersonId == personid).ToListAsync();
            return View(kins);
        }


        // GET: Kins/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kin kin = await db.Kins.Include(k=>k.Person).FirstOrDefaultAsync();
            if (kin == null)
            {
                return HttpNotFound();
            }
            return View(kin);
        }

        // GET: Kins/Create
        public ActionResult Create()
        {
            Kin kin = new Kin();
            return View(kin);
        }

        // POST: Kins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,Relation,PhoneNumber,Address,City,State,ZipCode")] Kin kin)
        {
            var appid = StaticMethods.GetAppId();
            if (ModelState.IsValid)
            {
                var person = await db.People.Include(p => p.ApplicationUser).Where(p => p.ApplicationId == appid).FirstOrDefaultAsync();
                kin.PersonId = person.PersonId; 
                db.Kins.Add(kin);
                await db.SaveChangesAsync();
                return RedirectToAction("Details","People");
            }

            //ViewBag.PersonId = new SelectList(db.People, "PersonId", "FirstName", kin.PersonId);
            return View(kin);
        }

        // GET: Kins/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kin kin = await db.Kins.FindAsync(id);
            if (kin == null)
            {
                return HttpNotFound();
            }
            //ViewBag.PersonId = new SelectList(db.People, "PersonId", "FirstName", kin.PersonId);
            return View(kin);
        }

        // POST: Kins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Name,Relation,PhoneNumber,Address,City,State,ZipCode")] Kin kin, int id)
        {
            Kin kinnow = await db.Kins.Include(k => k.Person).Where(k => k.KinId == id).FirstOrDefaultAsync();
            if (ModelState.IsValid)
            {
                kinnow.Name = kin.Name;
                kinnow.Relation = kin.Relation;
                kinnow.PhoneNumber = kin.Address;
                kinnow.City = kin.City;
                kinnow.State = kin.State;
                kinnow.ZipCode = kin.ZipCode;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //ViewBag.PersonId = new SelectList(db.People, "PersonId", "FirstName", kin.PersonId);
            return View(kin);
        }

        // GET: Kins/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kin kin = await db.Kins.FindAsync(id);
            if (kin == null)
            {
                return HttpNotFound();
            }
            return View(kin);
        }

        // POST: Kins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Kin kin = await db.Kins.FindAsync(id);
            kin.PersonId = 0;
            db.Kins.Remove(kin);
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
