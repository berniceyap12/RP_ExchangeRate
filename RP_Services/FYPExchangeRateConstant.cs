using System;
using System.Collections.Generic;
using System.Text;

namespace FYP_ExchangeRateDataLyaer
{
    public class FYPExchangeRateConstant
    {
        public const string ExchangeRateFYPSchema = "ExchangeRateFYP";
        public const string UsersTables = "Users";
        public const string CountriesTables = "Countries";
        public const string ExchangeRatesTables = "ExchangeRates";
		public const string TransactionTables = "Transactions";
		public const string EnquiryTables = "Enquiries";

		public partial class ExchangeRateFYPStoredProcedure
		{
			public const string InsertTransactions = "InsertTransactions";
			public const string UpdateTransactions = "UpdateTransactions";
			public const string DeleteTransactions = "DeleteTransactions";
			public const string InsertEnquiries = "InsertEnquiries";
		}

        public partial class UsersColumn
        {
			public const string UserID = "UserID";
			public const string UserType = "UserType";
			public const string Username = "Username";
			public const string Passwords = "Passwords";
			public const string EmailAddress = "EmailAddress";
			public const string Remarks = "Remarks";
			public const string IsActive = "IsActive";
			public const string CreatedBy = "CreatedBy";
			public const string CreatedDate = "CreatedDate";
			public const string ModifiedBy = "ModifiedBy";
			public const string ModifiedDate = "ModifiedDate";
		}

		public partial class CountriesColumn
		{
			public const string CountryID = "CountryID";
			public const string CountryName = "CountryName";
			public const string CurrentCurrencyRate = "CurrentCurrencyRate";
			public const string Remarks = "Remarks";
			public const string IsActive = "IsActive";
			public const string CreatedBy = "CreatedBy";
			public const string CreatedDate = "CreatedDate";
			public const string ModifiedBy = "ModifiedBy";
			public const string ModifiedDate = "ModifiedDate";
		}

		public partial class ExchangeRatesColumn
		{
			public const string ExchangeRateID = "ExchangeRateID";
			public const string FromCountryID = "FromCountryID";
			public const string FromCountryRate = "FromCountryRate";
			public const string ToCountryID = "ToCountryID";
			public const string ToCountryRate = "ToCountryRate";
			public const string Remarks = "Remarks";
			public const string IsActive = "IsActive";
			public const string CreatedBy = "CreatedBy";
			public const string CreatedDate = "CreatedDate";
			public const string ModifiedBy = "ModifiedBy";
			public const string ModifiedDate = "ModifiedDate";
		}

		public partial class TransactionColumn
		{
			public const string TransactionID = "TransactionID";
			public const string FromCountryID = "FromCountryID";
			public const string FromCountryRate = "FromCountryRate";
			public const string ToCountryID = "ToCountryID";
			public const string ToCountryRate = "ToCountryRate";
			public const string CustomerID = "CustomerID";
			public const string PaymentMethod = "PaymentMethod";
			public const string InputMoneyAmount = "InputMoneyAmount";
			public const string TotalCostAmount = "TotalCostAmount";
			public const string IsConfirmed = "IsConfirmed";
			public const string Remarks = "Remarks";
			public const string IsActive = "IsActive";
			public const string CreatedBy = "CreatedBy";
			public const string CreatedDate = "CreatedDate";
			public const string ModifiedBy = "ModifiedBy";
			public const string ModifiedDate = "ModifiedDate";
		}

		public partial class EnquiryColumn
		{
			public const string EnquiryID = "TransactionID";
			public const string EnquirySubject = "EnquirySubject";
			public const string MessageID = "MessageID";
			public const string EnquiryMessage = "EnquiryMessage";
			public const string TransactionID = "TransactionID";
			public const string CustomerID = "CustomerID";
			public const string Remarks = "Remarks";
			public const string IsActive = "IsActive";
			public const string CreatedBy = "CreatedBy";
			public const string CreatedDate = "CreatedDate";
			public const string ModifiedBy = "ModifiedBy";
			public const string ModifiedDate = "ModifiedDate";
		}
	}
}
