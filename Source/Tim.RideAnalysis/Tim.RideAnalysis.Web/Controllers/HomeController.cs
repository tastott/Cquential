using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tim.RideAnalysis.Web.Models;
using Tim.RideAnalysis.Core;
using System.IO;
using Tim.RideAnalysis.Models;

namespace Tim.RideAnalysis.Web.Controllers
{
    public class HomeController : Controller
    {
        private static Ride _ride;

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
            var bytes = new byte[file.InputStream.Length];
            file.InputStream.Read(bytes, 0, bytes.Length);

            using (var stream = new MemoryStream(bytes))
            {
                _ride = _importer.ImportRideFromGpxStream(stream, file.FileName);
            }

            return RedirectToAction("Analyse");
        }

        [HttpPost]
        public ActionResult GetRideFromStrava(string StravaActivityId)
        {
            _ride = _importer.ImportRideFromStravaApi(int.Parse(StravaActivityId));

            return RedirectToAction("Analyse");
        }

        [HttpGet]
        public ActionResult Analyse(string query)
        {
            var model = new AnalyseViewModel
            {
                Filename = _ride.Name,
                Query = query
            };

            if (String.IsNullOrEmpty(query)) return View(model);
            else return Analyse(model);
        }

        [HttpPost]
        public ActionResult Analyse(AnalyseViewModel model)
        {
            var matches = _analyser.SearchRide(_ride, model.Query);

            model.Matches = matches.Select(m => ToMatchViewModel(m));

            return View(model);
        }

        private MatchViewModel ToMatchViewModel(Match match)
        {
            var first = match.Legs.First();
            var last = match.Legs.Last();

            var distance = (last.TotalMetres - first.TotalMetres);
            var elevationDifference = last.StartElevation - first.StartElevation;

            return new MatchViewModel
            {
                Legs = match.Legs,
                AverageSpeed = distance * 0.001 / (last.FinishTime - first.StartTime).TotalHours,
                Distance = distance,
                ElevationDifference = elevationDifference,
                Gradient = elevationDifference / distance
            };
        }
    }
}
