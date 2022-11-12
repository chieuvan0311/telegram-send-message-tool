using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleMessenger.Model
{
    public class STT_GrName_GrLink_Model
    {
        public int STT { get; set; }

        [DisplayName("Tên Group")]
        [Column(TypeName ="nvarchar")]
        public string GrName { get; set; }

        [DisplayName("Kết Quả")]
        public string SessionResult { get; set; }

        [DisplayName("Link Group (Web)")]
        public string GrLink { get; set; }
    }
}
