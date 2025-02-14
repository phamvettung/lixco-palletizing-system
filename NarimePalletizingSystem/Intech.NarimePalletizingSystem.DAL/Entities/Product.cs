using System;
using System.Collections.Generic;

namespace Intech.NarimePalletizingSystem.DAL.Entities
{
    public partial class Product
    {
        public Product()
        {
            Pallets = new HashSet<Pallet>();
        }

        public string ProductCode { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public double NetWeight { get; set; }
        public int NumBinsOnPallet { get; set; }
        public int Model { get; set; }
        public double PacketWeight { get; set; }
        public int NumPacketsOnBin { get; set; }
        public string ProductImages { get; set; } = null!;

        public virtual ICollection<Pallet> Pallets { get; set; }
    }
}
