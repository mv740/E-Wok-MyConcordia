//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OracleEntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class PICTURE
    {
        public decimal ID_PK { get; set; }
        public byte[] PICTURE_DATA { get; set; }
        public string STATUS { get; set; }
        public System.DateTime CREATED { get; set; }
        public Nullable<System.DateTime> UPDATED { get; set; }
        public string STUDENT_NETNAME { get; set; }
        public string ADMINISTRATOR { get; set; }
        public string COMMENTS { get; set; }
    
        public virtual STUDENT STUDENT_ADMIN_FK { get; set; }
        public virtual STUDENT STUDENT_FK { get; set; }
    }
}