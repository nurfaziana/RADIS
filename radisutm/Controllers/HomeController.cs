using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using radisutm.DataAccess;
using radisutm.DataContext;
using radisutm.Models;
using radisutm.Helper;
using radisutm.ViewModel;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using radisutm.ClassFunction;
using System.Text.RegularExpressions;


   
namespace radisutm.Controllers
{
    public class HomeController : Controller
    {
        private BaseRepository<UserModel> _userRepository;

        ClsFunction CF = new ClsFunction();
        public HomeController()
        {
            _userRepository = new BaseRepository<UserModel>();
        }


        [Authorize]
        public ActionResult Index()
        {
            GrantDB modelPI = new GrantDB();
            return View("GrantDashboard", modelPI.GetGrantPI());
        }

        public ActionResult NewGrant()
        {
            return View("Proposal");
        }
        
        public ActionResult Home()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string ID_PENGGUNA, string KAD_PENGENALAN)
        {
            var membershipHelper = new MemberShipHelper();
            var model = new UserModel();
           // try
            //{
                if (Membership.ValidateUser(ID_PENGGUNA,KAD_PENGENALAN))
                {
                model.ID_PENGGUNA = ID_PENGGUNA;
                model.KAD_PENGENALAN = KAD_PENGENALAN;
                    FormsAuthentication.SetAuthCookie(model.ID_PENGGUNA, false);
                    Session["LoginUser"] = membershipHelper.GetUserProfile(model.ID_PENGGUNA, model.KAD_PENGENALAN);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Msg"] = "Invalid User Name and Password!";
                    return RedirectToAction("Login");
                }
            //}
            //catch (Exception e)
            //{
            //    TempData["Msg"] = e.Message;
           //     return RedirectToAction("Login");
           // }
        }


        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

    }
}