using System;
using System.Collections.Generic;

namespace Intech.NarimePalletizingSystem.DAL.Entities
{
    public partial class Pallet
    {
        public int PalletId { get; set; }
        public DateTime DateTime { get; set; }
        public string ProductCode { get; set; } = null!;
        public string LeverCode { get; set; } = null!;
        public int ShiftId { get; set; }
        public int TeamId { get; set; }
        public int LineId { get; set; }
        public bool Reached { get; set; }
        public bool Craft { get; set; }
        public int NumOrder { get; set; }
        public string Lot { get; set; }
        public string GroupCode { get; set; }
        public string FormName { get; set; }

        public virtual Product ProductCodeNavigation { get; set; } = null!;
    }
}
