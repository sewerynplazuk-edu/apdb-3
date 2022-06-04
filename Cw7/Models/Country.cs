﻿using System;
using System.Collections.Generic;

namespace Cw7.Models
{
    public partial class Country
    {
        public Country()
        {
            IdTrips = new HashSet<Trip>();
        }

        public int IdCountry { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Trip> IdTrips { get; set; }
    }
}
