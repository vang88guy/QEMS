using System;
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
            return  RedirectToAction("CreateFromTextMessage", "Situations");
        }
    }
}