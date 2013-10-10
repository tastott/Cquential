using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tim.RideAnalysis.Models;

namespace Tim.RideAnalysis.Web.Models
{
    public class MatchViewModel
    {
        public IEnumerable<Leg> Legs { get; set; }
        public TrackMapViewModel TrackMap
        {
            get
            {
                return new TrackMapViewModel
                {
                    Points = Legs.Select(l => Tuple.Create(l.StartLat, l.StartLng))
                };
            }
        }
    }
}