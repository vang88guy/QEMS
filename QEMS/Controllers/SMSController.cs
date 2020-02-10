using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.TwiML.Messaging;
using Twilio.AspNet.Mvc;
using System.Windows.Documents;
using static QEMS.Models.TwilioAPIKey;
using QEMS.Models;
using System.Threading.Tasks;
using Twilio.TwiML;

namespace QEMS.Controllers
{
    public class SMSController : TwilioController
    {
        ApplicationDbContext db;
        public SMSController()
        {
            db = new ApplicationDbContext();
        }
        public ActionResult SendSMSToKin()
        {
            var accountSid = TwilioAcct;
            var authToken = TwilioToken;
            //var notifyServiceSid = ServiceSidKey;
            TwilioClient.Init(accountSid, authToken);

            var phonenumbers = PhoneNumbers.PhoneNumbersToMessage;



            foreach (var number in phonenumbers)
            {
                var message = MessageResource.Create(
                    body: "Your kin " + PhoneNumbers.NameOfPerson + " is in an emergency situation, we are contacting the proper authorities on the behalf of your kin.",
                    from: new PhoneNumber("+12029317370"),
                    to: new PhoneNumber(number)
                );

                Console.WriteLine($"Message to {number} has been {message.Status}.");


            }
            return View("PlayerInterestEvents", "Events");
        }
        [HttpPost]
        public async Task<ActionResult> ReceiveSMS(string From, string Body)
        {
            var messagingResponse = new MessagingResponse();
            var message = new Message();
            var body = message.Body();
            PhoneNumbers.InComingNumber = From;
            PhoneNumbers.TextMessage = Body;
            CreateFromTextMessage();
            return  RedirectToAction("CreateFromTextMessage", "Situations");
        }
        public async void CreateFromTextMessage()
        {
            Person person = db.People.Include(p => p.ApplicationUser).Where(p => p.PhoneNumber == PhoneNumbers.InComingNumber).FirstOrDefault();
            Situation situation = new Situation();
            string datenow = System.DateTime.Now.ToString("MM/dd/yyyy");
            string timenow = System.DateTime.Now.ToString("h:mm tt");
            string message = PhoneNumbers.TextMessage;
            int result = 0;
            bool success = int.TryParse(new string(message
                                 .SkipWhile(x => !char.IsDigit(x))
                                 .TakeWhile(x => char.IsDigit(x))
                                 .ToArray()), out result);

            situation.PersonId = person.PersonId;
            situation.Message = PhoneNumbers.TextMessage;
            situation.Time = timenow;
            situation.Date = datenow;
            situation.Severity = result;
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
            PhoneNumbers.PhoneNumbersToMessage = await db.Kins.Include(k => k.Person).Where(k => k.PersonId == person.PersonId).Select(k => k.PhoneNumber).ToListAsync();
            SendSMSToKin();

        }

        [HttpPost]
        public TwiMLResult Index()
        {
            var messagingResponse = new MessagingResponse();
            messagingResponse.Message("The Robots are coming! Head for the hills!");

            return TwiML(messagingResponse);
        }
    }
}