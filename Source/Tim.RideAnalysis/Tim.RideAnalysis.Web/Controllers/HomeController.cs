using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tim.RideAnalysis.Web.Models;

namespace Tim.RideAnalysis.Web.Controllers
{
    public class HomeController : Controller
    {
        public static AnalyseViewModel AnalyseModel { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoadGpx(HttpPostedFileBase file)
        {
            AnalyseModel = new AnalyseViewModel { Filename = file.FileName };

            return RedirectToAction("Analyse");
        }

        public ActionResult Analyse()
        {
            return View(AnalyseModel);
        }
    }
}
