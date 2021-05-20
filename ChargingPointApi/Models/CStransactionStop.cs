using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChargingPointApi.Models
{
    public class CStransactionStop
    {

        public string SocketId { get; set; }

        public string Message { get; set; }

        public string TransactionId { get; set; }
    }
}
