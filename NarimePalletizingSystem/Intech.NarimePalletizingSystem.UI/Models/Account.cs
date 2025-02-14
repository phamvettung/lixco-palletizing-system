using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.NarimePalletizingSystem.UI.Models
{
    public class Account
    {
        public string userName {  get; set; }
        public string passWord { get; set; }

        public Account()
        {
            this.userName = string.Empty;
            this.passWord = string.Empty;
        }

        public Account(string userName, string passWord)
        {
            this.userName = userName;
            this.passWord = passWord;
        }
    }
}
