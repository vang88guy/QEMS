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
using System.Globalization;


namespace QEMS.Controllers
{
    public class SituationsController : Controller
    {
        private ApplicationDbContext db;
        private SMSController SMS = new SMSController();
        public SituationsController()
        {
            db = new ApplicationDbContext();
            
        }

        public async Task<ActionResult> OperatorSituationsForAdmin()
        {
            var situations = await db.SituationsOperators.Include(s => s.Situation).Include(s => s.Situation.Person).Include(s => s.Situation.Person.ApplicationUser).Include(s => s.Operator).Include(s => s.Operator.ApplicationUser).ToListAsync();
            return View(situations);
        }

        // GET: Situations
        public async Task<ActionResult> Index()
        {         
            var situations5 = await db.Situations.Include(s => s.Person).Where(s=>s.Severity >= 5 && s.Complete == false).ToListAsync();
            var situations4 = await db.Situations.Include(s => s.Person).Where(s => s.Severity == 4 && s.Complete == false).ToListAsync();
            var situations3 = await db.Situations.Include(s => s.Person).Where(s => s.Severity == 3 && s.Complete == false).ToListAsync();
            var situations2 = await db.Situations.Include(s => s.Person).Where(s => s.Severity == 2 && s.Complete == false).ToListAsync();
            var situations1 = await db.Situations.Include(s => s.Person).Where(s => s.Severity == 1 && s.Complete == false).ToListAsync();
            var situations = situations5.Concat(situations4).Concat(situations3).Concat(situations2).Concat(situations1);
            return View(situations);
        }

        public async Task<ActionResult> CompletedSituations()
        {
            var situations = await db.Situations.Include(s => s.Person).Where(s => s.Complete == true).ToListAsync();
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
        public async Task<ActionResult> Details(int id)
        {
            if (id == 0)
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
        public async Task<ActionResult> Create([Bind(Include = "Message,Severity,")] Situation situation)
        {
            var appid = StaticMethods.GetAppId();
            Person person = db.People.Include(p => p.ApplicationUser).Where(p=>p.ApplicationId == appid).FirstOrDefault();
            string datenow = StaticMethods.GetDate();
            string timenow = StaticMethods.GetTime();

            if (ModelState.IsValid)
            {
                situation.Time = timenow;
                situation.Date = datenow;
                situation.PersonId = person.PersonId;
                db.Situations.Add(situation);
                await db.SaveChangesAsync();
                if (person.MiddleName != null)
                {
                    PhoneNumbers.NameOfPerson = person.FirstName + " " + person.MiddleName + " " + person.LastName;
                }
                else 
                {
                    PhoneNumbers.NameOfPerson = person.FirstName + " " + person.LastName;
                }
                PhoneNumbers.PhoneNumbersToMessage = await db.Kins.Include(k => k.Person).Where(k => k.PersonId == person.PersonId).Select(k=>k.PhoneNumber).ToListAsync();
                SMS.SendSMSToKin();
                return RedirectToAction("PersonsSituations", "Situations");
            }

            //ViewBag.PersonId = new SelectList(db.People, "PersonId", "FirstName", situation.PersonId);
            return View(situation);
        }

        //public async void CreateFromTextMessage()
        //{           
        //    Person person = db.People.Include(p => p.ApplicationUser).Where(p => p.PhoneNumber == PhoneNumbers.InComingNumber).FirstOrDefault();
        //    Situation situation = new Situation();
        //    string datenow = System.DateTime.Now.ToString("MM/dd/yyyy");
        //    string timenow = System.DateTime.Now.ToString("h:mm tt");
        //    string message = PhoneNumbers.TextMessage;
        //    int result = 0;
        //    bool success = int.TryParse(new string(message
        //                         .SkipWhile(x => !char.IsDigit(x))
        //                         .TakeWhile(x => char.IsDigit(x))
        //                         .ToArray()), out result);
          
        //        situation.PersonId = person.PersonId;
        //        situation.Message = PhoneNumbers.TextMessage;
        //        situation.Time = timenow;
        //        situation.Date = datenow;
        //        situation.Severity = result;
        //        db.Situations.Add(situation);
        //        await db.SaveChangesAsync();
        //        if (person.MiddleName != null)
        //        {
        //            PhoneNumbers.NameOfPerson = person.FirstName + " " + person.MiddleName + " " + person.LastName;
        //        }
        //        else
        //        {
        //            PhoneNumbers.NameOfPerson = person.FirstName + " " + person.LastName;
        //        }
        //        PhoneNumbers.PhoneNumbersToMessage = await db.Kins.Include(k => k.Person).Where(k => k.PersonId == person.PersonId).Select(k => k.PhoneNumber).ToListAsync();
        //        SMS.SendSMSToKin();
                          
        //}

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
        public async Task<ActionResult> Edit(Situation situation, int id)
        {
            if (ModelState.IsValid)
            {
                
                var situationedit = await db.Situations.Include(s => s.Person).Where(s => s.SituationId == id).FirstOrDefaultAsync();
                if (situationedit.Time != situation.Time)
                {
                    var timenow = GetTimeFormat(situation.Time);
                    situationedit.Time = timenow;
                }
                if (situationedit.Date != situation.Date)
                {
                    var datenow = GetDateFormat(situation.Date);
                    situationedit.Date = datenow;
                }
                situationedit.Message = situation.Message;
                situationedit.Complete = situation.Complete;
                if (User.IsInRole("Operator") == true)
                {
                    situationedit.CallAmbulance = situation.CallAmbulance;
                    situationedit.CallFireStation = situation.CallFireStation;
                    situationedit.CallPoliceStation = situation.CallPoliceStation;
                    situationedit.InProcess = situation.InProcess;
                    SituationsOperators situationsOperators = new SituationsOperators();
                    var appid = StaticMethods.GetAppId();
                    var operatornow = await db.Operators.Include(o => o.ApplicationUser).FirstOrDefaultAsync(o=>o.ApplicationId == appid);
                    situationsOperators.Date = StaticMethods.GetDate();
                    situationsOperators.Time = StaticMethods.GetTime();
                    situationsOperators.SituationId = situationedit.SituationId;
                    situationsOperators.OperatorId = operatornow.OperatorId;
                    var situationsoperator2 = await db.SituationsOperators.Include(s => s.Situation).Include(s => s.Situation.Person).Include(s => s.Situation.Person.ApplicationUser).Include(s => s.Operator).Include(s => s.Operator.ApplicationUser).Where(s => s.OperatorId == operatornow.OperatorId && s.SituationId == situationedit.SituationId).FirstOrDefaultAsync();
                    if (situationsoperator2 == null)
                    {
                        db.SituationsOperators.Add(situationsOperators);
                    }
                    
                }
                
                await db.SaveChangesAsync();
                if (User.IsInRole("Person") == true)
                {
                    return RedirectToAction("PersonsSituations");
                }
                else 
                {
                    return RedirectToAction("Index");
                }
                
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
            return RedirectToAction("PersonsSituations", "Situations");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public string GetDateFormat(string date)
        {
            DateTime dateformat = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            string dateofevent = dateformat.ToString("MM/dd/yyyy");
            return dateofevent;
        }
        public string GetTimeFormat(string time)
        {
            DateTime timeformat = DateTime.ParseExact(time, "HH:mm", CultureInfo.InvariantCulture);
            string timeofevent = timeformat.ToString("h:mm tt");
            return timeofevent;
        }
    }
}
