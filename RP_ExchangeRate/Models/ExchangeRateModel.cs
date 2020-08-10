using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_ExchangeRate.Models
{
    public class ExchangeRateModel
    {
        public int ExchangeRateID { get; set; }

        public int FromCountryID { get; set; }

        [DataType(DataType.Currency)]
        public decimal FromCountryRate { get; set; }

        public string FromCountryName { get; set; }

        public int ToCountryID { get; set; }

        [DataType(DataType.Currency)]
        public decimal ToCountryRate { get; set; }

        public string ToCountryName { get; set; }

        public string Remarks { get; set; }

        public bool IsActive { get; set; }

        public string IsActiveRemark { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
