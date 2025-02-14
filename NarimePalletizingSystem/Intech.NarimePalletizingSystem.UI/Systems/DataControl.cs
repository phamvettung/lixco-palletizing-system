using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.NarimePalletizingSystem.UI.Systems
{
    public class DataControl
    {
        public ushort model { get; set; }
        public ushort order { get; set; }
        public ushort lineId { get; set; }
        public bool print { get; set; }
        public bool printError { get; set; }
        public bool reprint { get; set; }

        public DataControl()
        {
            this.lineId = 0;
            this.model = 0;
            this.order = 0;
            this.print = false;
            this.printError = false;
            this.reprint = false;
        }
    }
}
