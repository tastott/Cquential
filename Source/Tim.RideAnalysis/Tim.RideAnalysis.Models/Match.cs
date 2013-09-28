using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.RideAnalysis.Models
{
    public class Match
    {
        public IEnumerable<Leg> Legs { get; set; }
    }
}
