using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tim.RideAnalysis.Models;

namespace Tim.RideAnalysis.Web.Models
{
    public class AnalyseViewModel
    {
        public string Filename { get; set; }
        public string Query { get; set; }
        public IEnumerable<MatchViewModel> Matches { get; set; }
        public int MatchCount { get; set; }

        public IEnumerable<string> PreviousQueries
        {
            get
            {
                return new string[]
                {
                    "0.Speed > 20 AND ALL([x].Speed > [x-1].Speed) AND MAX(Speed) > MIN(Speed) * 2",
                    "n.StartElevation > 0.StartElevation + 40 AND ALL([x].StartElevation >= [x-1].StartElevation)"
                };
            }
        }
    }
}