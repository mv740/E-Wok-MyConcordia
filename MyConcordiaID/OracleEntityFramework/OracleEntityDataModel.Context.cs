﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    public partial class DatabaseEntities : DbContext
    {
        public DatabaseEntities()
            : base("DatabaseEntities")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<STUDENT> STUDENTS { get; set; }
        public virtual DbSet<PICTUREUPDATESETTING> PICTUREUPDATESETTINGs { get; set; }
        public virtual DbSet<PICTUREARCHIVE> PICTUREARCHIVEs { get; set; }
        public virtual DbSet<LOG> LOGs { get; set; }
    }
}
