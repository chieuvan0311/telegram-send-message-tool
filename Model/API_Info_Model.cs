using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleMessenger.Model
{
    public class API_Info_Model
    {
        public string Driver_Link_Id { get; set; }

        [Column(TypeName = "nvarchar")]
        public string Sheet_Name { get; set; }

        public string GGService_Email { get; set; }
        public string Sheet_Range { get; set; }
    }
}
