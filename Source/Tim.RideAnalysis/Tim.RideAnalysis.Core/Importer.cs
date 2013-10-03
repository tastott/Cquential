using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Tim.RideAnalysis.Core
{
    using Models;

    public class Importer
    {
        public IList<Waypoint> GetWaypointsFromGpxFile(string filepath)
        {
            var waypoints = new List<Waypoint>();

            var gpxDocument = XDocument.Load(filepath);
            var gpxNamespace = gpxDocument.Root.GetDefaultNamespace();
            var trkpts = gpxDocument.Descendants(gpxNamespace + "trkpt").ToList();

            foreach (var trkpt in trkpts)
            {
                double lat, lng, elev;
                DateTime time;

                if (
                        double.TryParse(trkpt.Attribute("lat").Value, out lat) &&
                        double.TryParse(trkpt.Attribute("lon").Value, out lng) &&
                        DateTime.TryParse(trkpt.Element(gpxNamespace + "time").Value, out time) &&
                        double.TryParse(trkpt.Element(gpxNamespace + "ele").Value, out elev)
                    )
                {
                    
                    var waypoint = new Waypoint
                    {
                        Latitude = lat,
                        Longitude = lng,
                        Time = time,
                        Elevation = elev
                    };

                    waypoints.Add(waypoint);
                }
                else throw new ArgumentException(String.Format("trkpt element is invalid: {0}", trkpt.ToString()));
            }

            return waypoints;
        }

        public Ride ImportRideFromGpxFile(string filepath)
        {
            var legs = new List<Leg>();

            var trkpts = GetWaypointsFromGpxFile(filepath);
            trkpts = RideUtilities.SmoothRide(trkpts);

            var from = trkpts.First();

            foreach (var to in trkpts.Skip(1))
            {
                var metres = DistanceBetweenTwoPoints(from.Latitude, from.Longitude, to.Latitude, to.Longitude) * 1000;
                int seconds = to.Time.Subtract(from.Time).Seconds;

                var leg = new Leg
                {
                    StartTime = from.Time,
                    FinishTime = to.Time,
                    StartLat = from.Latitude,
                    StartLng = from.Longitude,
                    Metres = metres,
                    Speed = (metres / seconds) * 3.6,
                    StartElevation = from.Elevation
                };

                legs.Add(leg);

                from = to;
            }

            return new Ride
            {
                Legs = legs
            };
        }

        private double DistanceBetweenTwoPoints(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            double e = (3.1415926538 * latitude1 / 180);
            double f = (3.1415926538 * longitude1 / 180);
            double g = (3.1415926538 * latitude2 / 180);
            double h = (3.1415926538 * longitude2 / 180);
            double i = (Math.Cos(e) * Math.Cos(g) * Math.Cos(f) * Math.Cos(h) + Math.Cos(e) * Math.Sin(f) * Math.Cos(g) * Math.Sin(h) + Math.Sin(e) * Math.Sin(g));
            double j = (Math.Acos(i));
            double k = (6371 * j);

            return k;
        }
    }
}
