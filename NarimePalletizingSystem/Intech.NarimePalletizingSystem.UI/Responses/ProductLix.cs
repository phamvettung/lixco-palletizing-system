using Intech.NarimePalletizingSystem.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.NarimePalletizingSystem.UI.Responses
{
    public class ProductLix
    {
        public int total { get; set; }
        public int pageSize { get; set; }
        public int currentPage { get; set; }
        public int totalPage { get; set; }
        public List<ProductLixDTO> productLixDTOs { get; set; }
    }
}
