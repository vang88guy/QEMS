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
        private ApplicationDbContext db;
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

        // GET: Kins/Details/5
        public async Task<ActionResult> Details(int? id)
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

        // GET: Kins/Create
        public ActionResult Create()
        {
            ViewBag.PersonId = new SelectList(db.People, "PersonId", "FirstName");
            return View();
        }

        // POST: Kins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "KinId,Name,Relation,PhoneNumber,Address,City,State,ZipCode,PersonId")] Kin kin)
        {
            if (ModelState.IsValid)
            {
                db.Kins.Add(kin);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.PersonId = new SelectList(db.People, "PersonId", "FirstName", kin.PersonId);
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
            ViewBag.PersonId = new SelectList(db.People, "PersonId", "FirstName", kin.PersonId);
            return View(kin);
        }

        // POST: Kins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "KinId,Name,Relation,PhoneNumber,Address,City,State,ZipCode,PersonId")] Kin kin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kin).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PersonId = new SelectList(db.People, "PersonId", "FirstName", kin.PersonId);
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
