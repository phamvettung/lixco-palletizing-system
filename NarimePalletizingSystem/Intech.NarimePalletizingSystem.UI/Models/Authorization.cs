using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.NarimePalletizingSystem.UI.Models
{
    public class Authorization
    {
        public string accessToken { get; set; }
        public object expiresIn { get; set; }
        public string tokenType { get; set; }
    }
}
