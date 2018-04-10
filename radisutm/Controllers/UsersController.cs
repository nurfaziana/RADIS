using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using radisutm.DataAccess;
using radisutm.Models;
using radisutm.ViewModel;

namespace radisutm.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users

        private BaseRepository<UserModel> userRepository;
        public UsersController()
        {
            userRepository = new BaseRepository<UserModel>();
        }
        public ActionResult Index()
        {
            var user = LoginUser.GetLogin();
            if (user != null)
            {
                user.KAD_PENGENALAN = "";
                return View(user);
            }

            return View("Error");
        }

    }
}