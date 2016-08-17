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
            if (TempData["shortMessage"] == null)
            {
                return View();
            }               
            ViewBag.Message = TempData["shortMessage"].ToString();
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

    }
}
