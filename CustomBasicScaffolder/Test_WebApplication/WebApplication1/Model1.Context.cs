﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SchoolDBEntities1 : DbContext
    {
        public SchoolDBEntities1()
            : base("name=SchoolDBEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BOOKS> BOOKS { get; set; }
    
        public virtual ObjectResult<QueryBooks_Result> QueryBooks(string queryBookName)
        {
            var queryBookNameParameter = queryBookName != null ?
                new ObjectParameter("QueryBookName", queryBookName) :
                new ObjectParameter("QueryBookName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<QueryBooks_Result>("QueryBooks", queryBookNameParameter);
        }
    }
}