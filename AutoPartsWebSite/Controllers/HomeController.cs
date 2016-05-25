using Postal;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

using IdentityAutoPart.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace IdentityAutoPart.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
                
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Send(string message)
        {
            dynamic email = new Email("Example");
            email.To = "nick@x-tend.kiev.ua";
            email.Message = message;
            email.Send();            

            return RedirectToAction("About");
        }

        public async Task<ActionResult> SendAsync(string message)
        {
            dynamic email = new Email("Example");
            email.To = "nick@x-tend.kiev.ua";
            email.Message = message;
            await email.Send();

            return RedirectToAction("About");
        }

        public ActionResult SendzMail()
        {

            var username = "site@alfa-parts.com";
            var sentFrom = "site@alfa-parts.com";
            var pwd = "G0l0qltN";

            var msg = new MailMessage(sentFrom, "nick@x-tend.kiev.ua", "subj", "body");
            var smtpClient = new SmtpClient("smtp.yandex.ru", 25);
            smtpClient.Credentials = new NetworkCredential(username, pwd);
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);
            return RedirectToAction("About");
        }


        public async Task<ActionResult> SendzMailAsync()
        {
            // Credentials:
            var credentialUserName = "site@alfa-parts.com";
            var sentFrom = "site@alfa-parts.com";
            var pwd = "G0l0qltN";

            // Configure the client:
            System.Net.Mail.SmtpClient client =
                new System.Net.Mail.SmtpClient("smtp.yandex.com");

            client.Port = 25; // 587 ;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            // Creatte the credentials:
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential(credentialUserName, pwd);

            client.EnableSsl = true;
            client.Credentials = credentials;

            //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            //var sentFrom = "site@alfa-parts.com";


            // Create the message:
            var mail =
                new System.Net.Mail.MailMessage(sentFrom, "nick@x-tend.kiev.ua");


            mail.Subject = "Subject";
            mail.Body = "Body";
            mail.IsBodyHtml = true;
            await client.SendMailAsync(mail);

            // Send:
            // return client.SendMailAsync(mail);
            return RedirectToAction("About");
        }
    }
}
