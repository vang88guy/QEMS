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
    public class PeopleController : Controller
    {
        private ApplicationDbContext db;
        public PeopleController()
        {
            db = new ApplicationDbContext();
        }

        // GET: People
        public async Task<ActionResult> Index()
        {
            var people = db.People.Include(p => p.ApplicationUser);
            return View(await people.ToListAsync());
        }

        // GET: People/Details/5
        public async Task<ActionResult> Details()
        {
            var appid = StaticMethods.GetAppId();
            if (appid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = await db.People.FindAsync(appid);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            //ViewBag.ApplicationId = new SelectList(db.Users, "Id", "Email");
            Person person = new Person();
            return View(person);
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FirstName,MiddleName,LastName,DateOfBirth,PhoneNumber,Addresss,City,State,ZipCode,LicenseNumber")] Person person)
        {
            if (ModelState.IsValid)
            { 
                person.ApplicationId = StaticMethods.GetAppId(); ;
                db.People.Add(person);
                await db.SaveChangesAsync();
                return RedirectToAction("LogOut", "Account");
            }
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<ActionResult> Edit(int? id)
        { 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = await db.People.Include(p => p.ApplicationUser).Where(p => p.PersonId == id).FirstOrDefaultAsync();
            if (person == null)
            {
                return HttpNotFound();
            }
           
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ApplicationUser.UserName,ApplicationUser.Email,FirstName,MiddleName,LastName,DateOfBirth,PhoneNumber,Addresss,City,State,ZipCode,LicenseNumber")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = await db.People.FindAsync(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Person person = await db.People.FindAsync(id);
            db.People.Remove(person);
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
