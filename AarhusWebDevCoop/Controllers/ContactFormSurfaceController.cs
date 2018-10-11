using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using AarhusWebDevCoop.ViewModels;
using System.Net.Mail;
using Umbraco.Core.Models;

namespace AarhusWebDevCoop.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: ContactFormSurface
        public ActionResult Index()
        {
            return PartialView("ContactForm", new ContactForm());
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
           if (!ModelState.IsValid) { return CurrentUmbracoPage(); }

         
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add("kasper.h90@gmail.com");
                mailMessage.Subject = model.Subject;
                mailMessage.From = new MailAddress(model.Email, model.Name);
                mailMessage.Body = model.Message + "\n my email is: " + model.Email;

                using (SmtpClient smtpClient = new SmtpClient())
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new System.Net.NetworkCredential("karl.bergen90@gmail.com", "xxx");
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mailMessage);
            }
            IContent comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "comment");
                    comment.SetValue("email", model.Email);
                    comment.SetValue("cname", model.Name);
                    comment.SetValue("subject", model.Subject);
                    comment.SetValue("message", model.Message);
                    Services.ContentService.Save(comment);

                    TempData["success"] = true;
           


            return RedirectToCurrentUmbracoPage();
        }
    }

}