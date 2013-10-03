using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.RideAnalysis.Models;

namespace Tim.RideAnalysis.Core
{
    public static class RideUtilities
    {
        public static IList<Waypoint> SmoothRide(IList<Waypoint> originalPoints)
        {
            int pointsCount = originalPoints.Count;
            var smoothedPoints = new Waypoint[pointsCount];

            smoothedPoints[0] = originalPoints[0];
            smoothedPoints[pointsCount - 1] = originalPoints[pointsCount - 1];

            for (int i = 1; i < pointsCount - 1; i++)
            {
                smoothedPoints[i] = new Waypoint
                {
                    Latitude = (originalPoints[i-1].Latitude + originalPoints[i+1].Latitude) / 2,
                    Longitude = (originalPoints[i - 1].Longitude + originalPoints[i + 1].Longitude) / 2,
                    Elevation = (originalPoints[i - 1].Elevation + originalPoints[i + 1].Elevation) / 2,
                    Time = originalPoints[i].Time
                };
            }

            return smoothedPoints;
        }
    }
}
