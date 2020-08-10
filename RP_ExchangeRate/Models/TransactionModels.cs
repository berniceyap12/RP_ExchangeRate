using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_ExchangeRate.Models
{
    public class TransactionModels
    {
        public int TransactionID
        {
            get;
            set;
        }

        public int? FromCountryID
        {
            get;
            set;
        }

        public decimal? FromCountryRate
        {
            get;
            set;
        }

        public int? ToCountryID
        {
            get;
            set;
        }

        public string CountryTo
        {
            get;set;
        }

        public string CountryFrom
        {
            get;set;
        }

        public  decimal? ToCountryRate
        {
            get;
            set;
        }

        public int? CustomerID
        {
            get;
            set;
        }

        public string CustomerName
        {
            get;
            set;
        }

        public string PaymentMethod
        {
            get;
            set;
        }

        public decimal? InputMoneyAmount
        {
            get;
            set;
        }

        public decimal? TotalCostAmount
        {
            get;
            set;
        }

        public bool? IsConfirmed
        {
            get;
            set;
        }

        public string Remarks
        {
            get;
            set;
        }

        public bool? IsActive
        {
            get;
            set;
        }

        public string IsActiveRemark
        {
            get;
            set;
        }


        public System.DateTime? CreatedDate
        {
            get;
            set;
        }

        public int? ModifiedBy
        {
            get;
            set;
        }

        public System.DateTime? ModifiedDate
        {
            get;
            set;
        }

        public bool? GenerateExcel
        {
            get;
            set;
        }
    }
}
