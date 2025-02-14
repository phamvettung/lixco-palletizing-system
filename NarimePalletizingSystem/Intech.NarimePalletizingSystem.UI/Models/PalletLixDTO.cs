using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.NarimePalletizingSystem.UI.Models
{
    public class PalletLixDTO
    {
        public string productCode { get; set; }
        // id ca
        public int shiftId { get; set; }
        // id tổ
        public int teamId { get; set; }
        public int lineId { get; set; }
        // dat chat luong hay khong
        public bool reached { get; set; }
        // 0: đóng máy, 1: đóng tay
        public bool craft { get; set; }
        // so thung/pallet
        public int numPallet { get; set; }
        //số thứ tự pallet
        public int numOrder { get; set; }
        // số lot
        public string lot { get; set; }
        //mã nhóm nhân viên tham gia line
        public string groupCode { get; set; }
    }
}
