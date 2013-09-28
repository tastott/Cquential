﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.RideAnalysis.Console
{
    using Models;
    using Core;

    class Program
    {
        private static Ride ride;
        private static Analyser analyser;

        static void Main(string[] args)
        {
            string command = null;

            while (command != "x")
            {
                if (command != null) ExecuteCommand(command);

                System.Console.WriteLine("Type a query or 'x' to exit");
                command = System.Console.ReadLine();
            }
        }

        private static void ExecuteCommand(string command)
        {
            if (ride == null) ride = new Importer().ImportRideFromGpxFile(@"C:\users\tim\downloads\evening ride.gpx");
            if (analyser == null) analyser = new Analyser();

            //string queryString =
            //"[0].Speed > 20 AND ALL([x].Speed > [x-1].Speed) AND MAX([*].Speed) > MIN([*].Speed) * 2 AND ALL([x].StartElevation >= [x-1].StartElevation)";

            try
            {
                var matches = analyser.SearchRide(ride, command);

                WriteMatches(matches);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Oh dear. Something bad happened.\n{0}", ex.Message);
            }
        }

        private static void WriteMatches(IEnumerable<Match> matches)
        {
            int counter = 0;

            foreach (var match in matches)
            {
                System.Console.WriteLine("---- Match {0} ----", counter);

                foreach (var leg in match.Legs)
                {
                    System.Console.Out.WriteLine("{0} | Speed: {1:F1}kph | Distance:{3:F0}m | Altitude:{2}m ",
                        leg.StartTime, leg.Speed, leg.StartElevation, leg.Metres);
                }

                System.Console.WriteLine("\n\n");

                ++counter;
            }
        }
    }
}
