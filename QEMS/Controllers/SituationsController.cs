﻿using System;
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
            var situations = db.Situations.Include(s => s.Person);
            return View(await situations.ToListAsync());
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
            ViewBag.PersonId = new SelectList(db.People, "PersonId", "FirstName");
            return View();
        }

        // POST: Situations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SituationId,Message,Time,Date,Severity,CallPoliceStation,CallFireStation,CallAmbulance,InProcess,Complete,PersonId")] Situation situation)
        {
            if (ModelState.IsValid)
            {
                db.Situations.Add(situation);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.PersonId = new SelectList(db.People, "PersonId", "FirstName", situation.PersonId);
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
            ViewBag.PersonId = new SelectList(db.People, "PersonId", "FirstName", situation.PersonId);
            return View(situation);
        }

        // POST: Situations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SituationId,Message,Time,Date,Severity,CallPoliceStation,CallFireStation,CallAmbulance,InProcess,Complete,PersonId")] Situation situation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(situation).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PersonId = new SelectList(db.People, "PersonId", "FirstName", situation.PersonId);
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