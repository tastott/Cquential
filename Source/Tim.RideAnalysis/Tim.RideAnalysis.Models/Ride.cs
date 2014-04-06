using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tim.RideAnalysis.Models
{
    public class Ride
    {
        public string Name { get; set; }
        public IEnumerable<Leg> Legs { get; set; }
    }
}
