using DataService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CapstoneProject_ODTS.Controllers
{
    public class FunctionPlus
    {
        public static string GetMD5HashString(string str)
        {
            return string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(str)).Select(s => s.ToString("x2")));
        }
    }

    public class LoginController : Controller
    {
        private AccountDomain _accountDomain;

        public LoginController()
        {
            _accountDomain = new AccountDomain();
        }
        public ActionResult Index()
        {
            Session["AdminIsLogedIn"] = null;
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Login()
        {
            if (Session["AdminIsLogedIn"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
                return View();
        }

        [HttpPost]
        public ActionResult CheckLogin(string username, string password, int roleId)
        {
            password = FunctionPlus.GetMD5HashString(password);
            var isSucess = _accountDomain.CheckLogin(username, password, roleId);
            Session["AdminIsLogedIn"] = null;
            if (!isSucess.IsError)
            {
                Session["AdminIsLogedIn"] = true;
                return Json(new { isSucess = isSucess, urlReturn = "/Request/Index" }, JsonRequestBehavior.AllowGet);                
            }
            else 
            {
                return Json(new { isSucess = isSucess }, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}