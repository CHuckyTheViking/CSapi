using System;
using System.Collections.Generic;

#nullable disable

namespace ChargingPointApi.Models
{
    public partial class ChargingPoint
    {
        public ChargingPoint()
        {
            Heartbeats = new HashSet<Heartbeat>();
        }

        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }
        public string SocketId { get; set; }
        public bool Available { get; set; }

        public virtual ICollection<Heartbeat> Heartbeats { get; set; }
    }
}
