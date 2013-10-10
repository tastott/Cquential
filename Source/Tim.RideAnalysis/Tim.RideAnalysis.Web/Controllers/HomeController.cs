using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tim.RideAnalysis.Web.Models;
using Tim.RideAnalysis.Core;
using System.IO;

namespace Tim.RideAnalysis.Web.Controllers
{
    public class HomeController : Controller
    {
        private static HttpPostedFileBase _uploadedFile;
        private static Byte[] _bytes;

        private Analyser _analyser;
        private Importer _importer;

        public HomeController()
        {
            _analyser = new Analyser();
            _importer = new Importer();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoadGpx(HttpPostedFileBase file)
        {
            _uploadedFile = file;
            _bytes = new byte[_uploadedFile.InputStream.Length];
            _uploadedFile.InputStream.Read(_bytes, 0, _bytes.Length);


            return RedirectToAction("Analyse");
        }

        [HttpGet]
        public ActionResult Analyse()
        {
            var model = new AnalyseViewModel
            {
                Filename = _uploadedFile.FileName
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Analyse(AnalyseViewModel model)
        {

            using (var stream = new MemoryStream(_bytes))
            {
                var ride = _importer.ImportRideFromGpxFile(stream);
                var matches = _analyser.SearchRide(ride, model.Query);

                model.Matches = matches.Select(m => new MatchViewModel { Legs = m.Legs });
            }

            return View(model);
        }
    }
}
