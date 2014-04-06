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
        public static IList<Waypoint> SmoothRide(IEnumerable<Waypoint> originalPoints)
        {
            var originalPointsList = originalPoints.ToList();
            int pointsCount = originalPointsList.Count;
            var smoothedPoints = new Waypoint[pointsCount];

            smoothedPoints[0] = originalPointsList[0];
            smoothedPoints[pointsCount - 1] = originalPointsList[pointsCount - 1];

            for (int i = 1; i < pointsCount - 1; i++)
            {
                smoothedPoints[i] = new Waypoint
                {
                    Latitude = (originalPointsList[i-1].Latitude + originalPointsList[i+1].Latitude) / 2,
                    Longitude = (originalPointsList[i - 1].Longitude + originalPointsList[i + 1].Longitude) / 2,
                    Elevation = (originalPointsList[i - 1].Elevation + originalPointsList[i + 1].Elevation) / 2,
                    Time = originalPointsList[i].Time
                };
            }

            return smoothedPoints;
        }

        public static Leg[] Coarsen(Leg[] legs, int coarseness)
        {
            return legs;
        }
    }
}
