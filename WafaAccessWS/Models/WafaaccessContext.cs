using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Configuration;
using Oracle.ManagedDataAccess.Client; 

namespace WafaAccessWS.Models
{
   public class WafaaccessContext : DbContext
    {
        public WafaaccessContext()
            : base(new OracleConnection(ConfigurationManager.ConnectionStrings["WafaaccessContext"].ConnectionString), true)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientEntity>().ToTable("CLIENTENTITY", "BANK");
            modelBuilder.Entity<Auditlog>().ToTable("AUDITLOG", "BANK");
            // modelBuilder.Conventions.Remove<ColumnTypeCasingConvention>();

        }

        public DbSet<WafaAccessWS.Models.ClientEntity> ClientEntity { get; set; }

        public DbSet<WafaAccessWS.Models.Auditlog> Auditlog { get; set; }
    }
}
