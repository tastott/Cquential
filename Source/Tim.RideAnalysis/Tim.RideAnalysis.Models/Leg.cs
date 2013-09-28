using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.RideAnalysis.Models
{
    public class Leg
    {
        public double StartLat { get; set; }
        public double StartLng { get; set; }
        public double FinishLat { get; set; }
        public double FinishLng { get; set; }

        public DateTime StartTime {get; set;}
        public DateTime FinishTime {get; set;}
        
        public DateTime Duration {get; set;}
        public double Metres {get; set;}

        public double Speed { get; set; }

        public double StartElevation { get; set; }
        public double ElevationDelta { get; set; }

    }
}
