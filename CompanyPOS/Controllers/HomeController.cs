using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace CompanyPOS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult GetIndex()
        {
            ViewBag.Title = "Home Page";

            CompanyPosRepository repository = new CompanyPosRepository();

           ViewBag.Companies =  repository.GetCompanies();

            return View();
        }
    }
}
