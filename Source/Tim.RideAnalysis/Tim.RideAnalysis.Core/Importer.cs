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
        public Ride ImportRideFromGpxFile(string filepath)
        {
            var legs = new List<Leg>();

            var gpxDocument = XDocument.Load(filepath);
            var gpxNamespace = gpxDocument.Root.GetDefaultNamespace();
            var trkpts = gpxDocument.Descendants(gpxNamespace + "trkpt").ToList();

            var from = trkpts.First();

            foreach (var to in trkpts.Skip(1))
            {
                double startLat, startLng, finishLat, finishLng, startElev, finishElev;
                DateTime startTime, finishTime;

                if (
                        double.TryParse(from.Attribute("lat").Value, out startLat) &&
                        double.TryParse(from.Attribute("lon").Value, out startLng) &&
                        double.TryParse(to.Attribute("lat").Value, out finishLat) &&
                        double.TryParse(to.Attribute("lon").Value, out finishLng) &&
                        DateTime.TryParse(from.Element(gpxNamespace + "time").Value, out startTime) &&
                        DateTime.TryParse(to.Element(gpxNamespace + "time").Value, out finishTime) &&
                        double.TryParse(from.Element(gpxNamespace + "ele").Value, out startElev) &&
                        double.TryParse(to.Element(gpxNamespace + "ele").Value, out finishElev)
                    )
                {
                    var metres = DistanceBetweenTwoPoints(startLat, startLng, finishLat, finishLng) * 1000;
                    int seconds = finishTime.Subtract(startTime).Seconds;

                    var leg = new Leg
                    {
                        StartTime = startTime,
                        FinishTime = finishTime,
                        StartLat = startLat,
                        StartLng = startLng,
                        Metres = metres,
                        Speed = (metres / seconds) * 3.6,
                        StartElevation = startElev
                    };

                    legs.Add(leg);
                }
                else throw new ArgumentException("One or both trkpt elements is invalid");

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
