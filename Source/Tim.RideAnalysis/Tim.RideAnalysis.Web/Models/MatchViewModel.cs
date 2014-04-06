using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tim.RideAnalysis.Models;
using Tim.RideAnalysis.Utilities;

namespace Tim.RideAnalysis.Web.Models
{
    public class MatchViewModel
    {
        public IEnumerable<Leg> Legs { get; set; }

        public double Distance { get; set; }
        public double AverageSpeed { get; set; }
        public double ElevationDifference { get; set; }
        public double ElevationGain { get; set; }
        public double Gradient { get; set; }

        public TrackMapViewModel TrackMap
        {
            get
            {
                return new TrackMapViewModel
                {
                    Points = Legs.Coarsen(4).Select(l => Tuple.Create(l.StartLat, l.StartLng))
                };
            }
        }
    }
}