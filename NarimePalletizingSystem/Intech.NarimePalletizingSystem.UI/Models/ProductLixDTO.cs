using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.NarimePalletizingSystem.UI.Models
{
    public class ProductLixDTO
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string unit { get; set; }
        public double numPallet { get; set; }
    }
}
