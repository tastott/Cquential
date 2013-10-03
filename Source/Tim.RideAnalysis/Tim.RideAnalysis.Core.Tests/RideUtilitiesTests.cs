using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace Tim.RideAnalysis.Core
{
    using Models;

    [TestClass]
    public class RideUtilitiesTests
    {
        [TestMethod]
        public void SmoothLeavesFirstPointUnchanged()
        {
            var points = new Waypoint[]
            {
                new Waypoint
                {
                    Latitude = 0,
                    Longitude = 0,
                    Elevation = 0
                },
                new Waypoint
                {
                    Latitude = 1,
                    Longitude = 1,
                    Elevation = 1
                },
                 new Waypoint
                {
                    Latitude = 2,
                    Longitude = 2,
                    Elevation = 2
                }
            };

            var smoothed = RideUtilities.SmoothRide(points);

            smoothed.First()
                .Should()
                .Be(points.First());
        }

        [TestMethod]
        public void SmoothLeavesLastPointUnchanged()
        {
            var points = new Waypoint[]
            {
                new Waypoint
                {
                    Latitude = 0,
                    Longitude = 0,
                    Elevation = 0
                },
                new Waypoint
                {
                    Latitude = 1,
                    Longitude = 1,
                    Elevation = 1
                },
                 new Waypoint
                {
                    Latitude = 2,
                    Longitude = 2,
                    Elevation = 2
                }
            };

            var smoothed = RideUtilities.SmoothRide(points);

            smoothed.Last()
                .Should()
                .Be(points.Last());
        }

        [TestMethod]
        public void SmoothReturnsSameNumberOfPoints()
        {
            var points = new Waypoint[]
            {
                new Waypoint
                {
                    Latitude = 0,
                    Longitude = 0,
                    Elevation = 0
                },
                new Waypoint
                {
                    Latitude = 1,
                    Longitude = 1,
                    Elevation = 1
                },
                 new Waypoint
                {
                    Latitude = 2,
                    Longitude = 2,
                    Elevation = 2
                }
            };

            var smoothed = RideUtilities.SmoothRide(points);

            smoothed.
                Should()
                .HaveCount(3);
        }

        [TestMethod]
        public void SmoothCalculatesLatitudeLongitudeAndElevationAsAverageOfAdjacentValues()
        {
            var points = new Waypoint[]
            {
                new Waypoint
                {
                    Latitude = 0,
                    Longitude = 0,
                    Elevation = 0
                },
                new Waypoint
                {
                    Latitude = 1,
                    Longitude = 1,
                    Elevation = 1
                },
                 new Waypoint
                {
                    Latitude = 4,
                    Longitude = 4,
                    Elevation = 4
                },
                 new Waypoint
                {
                    Latitude = 5,
                    Longitude = 5,
                    Elevation = 5
                }
            };

            var smoothed = RideUtilities.SmoothRide(points).ToList();

            smoothed[1]
                .Should()
                .Match<Waypoint>(wp => wp.Latitude == 2 && wp.Longitude == 2 && wp.Elevation == 2);

            smoothed[2]
                .Should()
                .Match<Waypoint>(wp => wp.Latitude == 3 && wp.Longitude == 3 && wp.Elevation == 3);
        }

        [TestMethod]
        public void SmoothLeavesTimesUnchanged()
        {
            var points = new Waypoint[]
            {
                new Waypoint
                {
                    Latitude = 0,
                    Longitude = 0,
                    Elevation = 0
                },
                new Waypoint
                {
                    Latitude = 1,
                    Longitude = 1,
                    Elevation = 1
                },
                 new Waypoint
                {
                    Latitude = 4,
                    Longitude = 4,
                    Elevation = 4
                },
                 new Waypoint
                {
                    Latitude = 5,
                    Longitude = 5,
                    Elevation = 5
                }
            };

            var smoothed = RideUtilities.SmoothRide(points).ToList();

            for (int i = 0; i < points.Length; i++)
            {
                smoothed[i].Time.Should().Be(points[i].Time);
            }
        }
    }
}
