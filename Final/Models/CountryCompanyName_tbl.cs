//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Final.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CountryCompanyName_tbl
    {
        public int countryID { get; set; }
        public int companyID { get; set; }
        public string companyName { get; set; }
        public string countryName { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
    
        public virtual CompanyName_tbl CompanyName_tbl { get; set; }
        public virtual Country_tbl Country_tbl { get; set; }
    }
}