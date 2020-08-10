using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_ExchangeRate.Models
{
    public class CountriesModel
    {
        public int CountryID { get; set; }
        [DataType(DataType.Text)]
        public string CountryName { get; set; }

        [DataType(DataType.Currency)]
        public decimal CurrentCurrencyRate { get; set; }

        [DataType(DataType.Currency)]
        public decimal CurrencyInSGD { get; set; }
        public string Remarks { get; set; }

        public bool IsActive { get; set; }

        public string IsActiveRemark { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
