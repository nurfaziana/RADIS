using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radisutm.Models
{
   
    [Table("HRDEV.PENGGUNA")]
    public class UserModel
    {
        [Key]
        [StringLength(10)]
        public string ID_PENGGUNA { get; set; }

        [StringLength(12)]
        public string KAD_PENGENALAN { get; set; }

        [StringLength(30)]
        public string NAMA { get; set; }


      
        public int STAF_FK { get; set; }


    }
}