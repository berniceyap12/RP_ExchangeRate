using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_ExchangeRate.Models
{
    public class EnquiryModel
    {
        public int? TransactionID { get; set; }

        public int? MessageID { get; set; }

        public string EnquirySubject { get; set; }

        public string EnquiryMessage { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CustomerID { get; set; }

        public string CustomerName { get; set; }

        public int? CreatedBy { get; set; }

        public string CreatedName { get; set; }

        public bool? RefreshPage { get; set; }

    }
}
