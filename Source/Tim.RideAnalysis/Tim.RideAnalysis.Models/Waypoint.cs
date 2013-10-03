using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.RideAnalysis.Models
{
    public class Waypoint
    {
        public DateTime Time { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }

        public override bool Equals(object obj)
        {
            var that = obj as Waypoint;

            if (that != null)
            {
                return this.Time.Equals(that.Time)
                    && this.Elevation.Equals(that.Elevation)
                    && this.Latitude.Equals(that.Latitude)
                    && this.Longitude.Equals(that.Longitude);
            }
            else return false;
        }
    }
}
