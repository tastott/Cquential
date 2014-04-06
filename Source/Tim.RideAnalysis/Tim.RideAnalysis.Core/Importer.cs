using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using com.strava.api.Client;
using com.strava.api.Authentication;
using com.strava.api.Streams;

namespace Tim.RideAnalysis.Core
{
    using Models;
    

    public class Importer
    {
        public Ride ImportRideFromStravaApi(int activityId)
        {
            var stravaAuth = new StaticAuthentication("ad49a857719b291e9e71ca45c7cc4e3a661ad505");
            var stravaClient = new com.strava.api.Client.StravaClient(stravaAuth);

            var activity = stravaClient.Activities.GetActivity(activityId.ToString(), false);
            var activityStreams = stravaClient.Streams.GetActivityStream(activityId.ToString(), StreamType.Time | StreamType.LatLng | StreamType.Altitude);

            var startTime = DateTime.Parse(activity.StartDate);

            var waypoints = GetWaypointsFromStravaActivityStreams(activityStreams, startTime);
            var legs = GetLegsFromWaypoints(waypoints).ToList();

            return new Ride
            {
                Name = activity.Name,
                Legs = legs
            };
        }

        private IEnumerable<Waypoint> GetWaypointsFromStravaActivityStreams(IEnumerable<ActivityStream> activityStreams, DateTime startTime)
        {
            Func<StreamType, ActivityStream> GetStreamByType = 
                type => activityStreams.Single(x => x.StreamType == type);

            var timeStream = GetStreamByType(StreamType.Time);
            var latLngStream = GetStreamByType(StreamType.LatLng);
            var altitudeStream = GetStreamByType(StreamType.Altitude);
   
            for (int i = 0; i < timeStream.OriginalSize; i++)
            {
                var time = (Int64)timeStream.Data[i];
                var latLng = ((Newtonsoft.Json.Linq.JArray)latLngStream.Data[i]);
                var elevation = (double)altitudeStream.Data[i];

                yield return new Waypoint
                {
                    Time = startTime.AddSeconds(time),
                    Latitude = latLng[0].ToObject<double>(),
                    Longitude = latLng[1].ToObject<double>(),
                    Elevation = elevation
                };
            }
        }

        public Ride ImportRideFromGpxStream(Stream stream, string name)
        {
            var waypoints = GetWaypointsFromGpxStream(stream);
            var smoothedWaypoints = RideUtilities.SmoothRide(waypoints);
            var legs = GetLegsFromWaypoints(smoothedWaypoints);

            return new Ride
            {
                Name = name,
                Legs = legs
            };
        }

        public Ride ImportRideFromGpxFile(string filepath)
        {
            using (var stream = File.OpenRead(filepath))
            {
                var name = System.IO.Path.GetFileNameWithoutExtension(filepath);

                return ImportRideFromGpxStream(stream, name);
            }
        }

        public IEnumerable<Waypoint> GetWaypointsFromGpxStream(Stream stream)
        {
            var gpxDocument = XDocument.Load(stream);
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
                    
                    yield return new Waypoint
                    {
                        Latitude = lat,
                        Longitude = lng,
                        Time = time,
                        Elevation = elev
                    };
                }
                else throw new ArgumentException(String.Format("trkpt element is invalid: {0}", trkpt.ToString()));
            }
        }

        private IEnumerable<Leg> GetLegsFromWaypoints(IEnumerable<Waypoint> waypoints)
        {
            var from = waypoints.First();
            double totalMetres = 0;

            foreach (var to in waypoints.Skip(1))
            {
                var metres = DistanceBetweenTwoPoints(from.Latitude, from.Longitude, to.Latitude, to.Longitude) * 1000;
                totalMetres += metres;

                int seconds = to.Time.Subtract(from.Time).Seconds;

                yield return new Leg
                {
                    StartTime = from.Time,
                    FinishTime = to.Time,
                    StartLat = from.Latitude,
                    StartLng = from.Longitude,
                    Metres = metres,
                    TotalMetres = totalMetres,
                    Speed = (metres / seconds) * 3.6,
                    StartElevation = from.Elevation
                };

                from = to;
            }
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
