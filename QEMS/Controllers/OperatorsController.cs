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
    public class OperatorsController : Controller
    {
        private ApplicationDbContext db;
        public OperatorsController()
        {
            db = new ApplicationDbContext();
        }

        // GET: Operators
        public async Task<ActionResult> Index()
        {
            return View(await db.Operators.ToListAsync());
        }

        // GET: Operators/Details/5
        public async Task<ActionResult> Details()
        {
            var appid = StaticMethods.GetAppId();
            if (appid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operator @operator = await db.Operators.Include(o=>o.ApplicationUser).FirstOrDefaultAsync(o=>o.ApplicationId == appid);
            if (@operator == null)
            {
                return HttpNotFound();
            }
            return View(@operator);
        }

        // GET: Operators/Create
        public ActionResult Create()
        {
            Operator @operator = new Operator();
            return View(@operator);
        }

        // POST: Operators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FirstName,LastName,Addresss,City,State,ZipCode")] Operator @operator)
        {
            if (ModelState.IsValid)
            {
                @operator.ApplicationId = StaticMethods.GetAppId();
                db.Operators.Add(@operator);
                await db.SaveChangesAsync();
                return RedirectToAction("LogOut", "Account");
            }

            return View(@operator);
        }

        // GET: Operators/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operator @operator = await db.Operators.Include(o=>o.ApplicationUser).Where(o=>o.OperatorId == id).FirstOrDefaultAsync();
            if (@operator == null)
            {
                return HttpNotFound();
            }
            return View(@operator);
        }

        // POST: Operators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ApplicationUser.UserName,ApplicationUser.Email,FirstName,LastName,Addresss,City,State,ZipCode")] Operator @operator)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@operator).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(@operator);
        }

        // GET: Operators/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Operator @operator = await db.Operators.FindAsync(id);
            if (@operator == null)
            {
                return HttpNotFound();
            }
            return View(@operator);
        }

        // POST: Operators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Operator @operator = await db.Operators.FindAsync(id);
            db.Operators.Remove(@operator);
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
