using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_ExchangeRate.Models
{
    public class UserModel
    {
        //user accounts - wanli
        public int userid { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        
        public string emailaddress { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Remarks { get; set; }

        public bool IsActive { get; set; }

        public string IsActiveRemark { get; set; }

        public DateTime ModifiedDate { get; set; }

        public UserTypes usertype { get; set; }
    }

    public enum UserTypes
    {
        Admin = 1,
        staff = 2,
        Customer = 3
    }
}
