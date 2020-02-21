using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using uStoreMVC.Data.EF;
using uStoreMVC.Models;

namespace uStoreMVC.Controllers
{

    public class HomeController : Controller
    {
        private uStoreEntities1 db = new uStoreEntities1();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel contact)
        {
            if (ModelState.IsValid)
            {
                //Create the body of the email (written letter)
                string body = string.Format("Name: {0}<br/>Email: {1}<br/>Subject: {2}<br/>Message: {3}",
                contact.Name,
                contact.Email,
                contact.Subject,
                contact.Message);



                //Create and configure the email message (found address)

                MailMessage msg = new MailMessage("jorge@jpietro.com", "pietrogiovanna.1611@outlook.com", contact.Subject, body);

                //Configure the MailMessage object (picked card to include with it)(ex. do we want to cc: someone on this, forward this, etc.)

                msg.IsBodyHtml = true; //This treats the text as html
                                       // msg.Priority = MailPriority.High; //Sets ,essage priority in the recipient's email
                                       // msg.CC.Add("jbecket@centriq.com"); //this is to add a cc recipient




                //Create and configure the SMTP client (chooisng the post office to be the one to send the letter)
                SmtpClient client = new SmtpClient("mail.jpietro.com"); //(who is handling the email?)
                client.Credentials = new NetworkCredential("jorge@jpietro.com", "Centriq123!"); //(how to log in)
                //client.Port = 25;
                //client.EnableSsl = false;

                using (client) //we use Try here in case server is down, etc. 
                {
                    try
                    {
                        client.Send(msg); //got "msg" from above in "Create and configure email message"
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = "There was a problem sending your email. Please try again later." + ex.Message;
                        return View(contact);
                    }
                }

                return View("ContactConfirmation", contact);
            }
            return View(contact);
        }

        public PartialViewResult ProductsPV() //Added a Partial view for the index to show the products.
        {
            var products = db.Products.ToList();
            return PartialView(products);
        }

        public PartialViewResult TripsPV()
        {
            var trips = db.Trips.ToList();
            return PartialView(trips);
        }
    }
}