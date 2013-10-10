using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tim.RideAnalysis.Web.Models
{
    public class TrackMapViewModel
    {
        public IEnumerable<Tuple<double, double>> Points { get; set; }
    }
}