using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Diagnostics;

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
            var schemaName = ConfigurationManager.AppSettings["schemaName"];
            //Debug.WriteLine(" schemaName = " + schemaName);

            modelBuilder.Entity<ClientEntity>().ToTable("CLIENTENTITY", schemaName);
            modelBuilder.Entity<WAFAAuditlog>().ToTable("WAFAAUDITLOG", schemaName);
            // modelBuilder.Conventions.Remove<ColumnTypeCasingConvention>();

        }

        public DbSet<WafaAccessWS.Models.ClientEntity> ClientEntity { get; set; }

        public DbSet<WafaAccessWS.Models.WAFAAuditlog> WAFAAuditlog { get; set; }
    }
}
