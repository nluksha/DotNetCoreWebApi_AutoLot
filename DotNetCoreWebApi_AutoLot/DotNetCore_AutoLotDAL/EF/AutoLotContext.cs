using DotNetCore_AutoLotDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore_AutoLotDAL.EF
{
    public class AutoLotContext: DbContext
    {
        public DbSet<CreditRisk> CreditRisks { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Inventory> Cars { get; set; }
        public DbSet<Order> Orders { get; set; }

        internal AutoLotContext()
        {
        }

        public AutoLotContext(DbContextOptions options): base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = @"server=.; database=AutoLotCore2; integrated security=True; MultipleActiveResultSets=True; App=EntityFramework";
                optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure())
                    .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CreditRisk>(entity =>
            {
                entity.HasIndex(e => new { e.FirstName, e.LastName }).IsUnique();
            });

            modelBuilder.Entity<Order>()
                .HasOne(e => e.Car)
                .WithMany(e => e.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }

        public string GetTableName(Type type)
        {
            return Model.FindEntityType(type).SqlServer().TableName;
        }
    }
}
