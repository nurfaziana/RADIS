using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using radisutm.Models;
using System.Data;
using radisutm.ClassFunction;

namespace radisutm.Controllers
{
    public class GrantController : Controller
    {
       
        // GET: Grant
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GrantProfile(string id)
        {

            GrantDB modelGrant = new GrantDB();
            return View("GrantProfile", modelGrant.GetGrantInfo(id));
        }


        public ActionResult Proposal()
        {
            return View("GrantProfile");
        }
        public ActionResult FinancialInfo(string id)
        {
            GrantDB modelGrant = new GrantDB();
            return View("FinancialInfo", modelGrant.GetGrantInfo(id));
        }
    }
}