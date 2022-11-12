using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleMessenger.Model
{
    public class Gift_Items
    {
        public List<string> Get() 
        {
            List<string> listString = new List<string>();

            listString.Add("Vanilla");
            listString.Add("Amz_bill");
            listString.Add("Amz_cash");
            listString.Add("Amz_Ecode");
            listString.Add("Amz_Offer");
            listString.Add("Sephora");
            listString.Add("Walmart");
            listString.Add("Target");
            listString.Add("Ebay");
            listString.Add("Adidas");
            listString.Add("Macys");
            listString.Add("Saks");
            listString.Add("Carter");
            listString.Add("BBW");

            return listString;
        }
    }
}
