﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 6/14/2020 10:13:20 PM
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;
using System.Collections.Generic;

namespace FYPExchangeRateModel
{
    public partial class Enquiry {

        public Enquiry()
        {
            OnCreated();
        }

        public virtual int EnquiryID
        {
            get;
            set;
        }

        public virtual string EnquirySubject
        {
            get;
            set;
        }

        public virtual int? MessageID
        {
            get;
            set;
        }

        public virtual string EnquiryMessage
        {
            get;
            set;
        }

        public virtual int? CustomerID
        {
            get;
            set;
        }

        public string CustomerName
        {
            get;set;
        }

        public virtual int? TransactionID
        {
            get;
            set;
        }

        public virtual string Remarks
        {
            get;
            set;
        }

        public virtual bool? IsActive
        {
            get;
            set;
        }

        public virtual int? CreatedBy
        {
            get;
            set;
        }

        public string CreatedName
        {
            get;set;
        }

        public virtual System.DateTime? CreatedDate
        {
            get;
            set;
        }

        public virtual int? ModifiedBy
        {
            get;
            set;
        }

        public virtual System.DateTime? ModifiedDate
        {
            get;
            set;
        }

        public virtual User User
        {
            get;
            set;
        }

        public virtual Transaction Transaction
        {
            get;
            set;
        }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}