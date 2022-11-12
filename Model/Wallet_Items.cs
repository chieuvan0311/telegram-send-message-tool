using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleMessenger.Model
{
    public class Wallet_Items
    {
        public List<string> Get() 
        {
            var listString = new List<string>();
            listString.Add("Greendot");
            listString.Add("Bluebird");
            listString.Add("Chime");
            listString.Add("PPUS");
            listString.Add("PPVN");

            return listString;
        }
    }
}
