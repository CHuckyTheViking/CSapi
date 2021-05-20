using System;
using System.Collections.Generic;

#nullable disable

namespace ChargingPointApi.Models
{
    public partial class Heartbeat
    {
        public int Id { get; set; }
        public DateTime Hbtime { get; set; }
        public int Cpid { get; set; }

        public virtual ChargingPoint Cp { get; set; }
    }
}
