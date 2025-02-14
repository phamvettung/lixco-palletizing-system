using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.NarimePalletizingSystem.UI.Configurations
{
    public class PlcConfig
    {
        public CpuType CpuType { get; set; }
        public string? IpAddress { get; set; }
        public short Rack { get; set; }
        public short Slot { get; set; }
    }
}
