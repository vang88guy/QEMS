using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.TwiML;
using Twilio.AspNet.Mvc;
using System.Windows.Documents;
using static QEMS.Models.TwilioAPIKey;
using QEMS.Models;

namespace QEMS.Controllers
{
    public class SMSController : TwilioController
    {
        public ActionResult SendSMSToPlayers()
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
                    from: new PhoneNumber("+12562420890"),
                    to: new PhoneNumber(number)
                );

                Console.WriteLine($"Message to {number} has been {message.Status}.");


            }
            return View("PlayerInterestEvents", "Events");
        }
    }
}