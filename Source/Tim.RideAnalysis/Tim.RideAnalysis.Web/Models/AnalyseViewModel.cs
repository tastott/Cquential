using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tim.RideAnalysis.Models;

namespace Tim.RideAnalysis.Web.Models
{
    public class AnalyseViewModel
    {
        public string Filename { get; set; }
        public string Query { get; set; }
        public IEnumerable<Match> Matches { get; set; }
    }
}