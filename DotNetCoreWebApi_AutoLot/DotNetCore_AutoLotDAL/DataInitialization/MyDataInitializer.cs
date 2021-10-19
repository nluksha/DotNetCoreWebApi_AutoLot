using DotNetCore_AutoLotDAL.EF;
using DotNetCore_AutoLotDAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore_AutoLotDAL.DataInitialization
{
    public class MyDataInitializer
    {
        public static void RecreateDatabase(AutoLotContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }

        public static void ClearData(AutoLotContext context)
        {
            ExecureDeleteSql(context, "Orders");
            ExecureDeleteSql(context, "Customers");
            ExecureDeleteSql(context, "Inventory");
            ExecureDeleteSql(context, "CreditRisks");
            ResetIdentity(context);

        }

        public static void ExecureDeleteSql(AutoLotContext context, string tableName)
        {
            var rawSqlString = $"Delete from dbo.{tableName}";
            context.Database.ExecuteSqlCommand(rawSqlString);
        }

        public static void ResetIdentity(AutoLotContext context)
        {
            var tables = new[]
            {
                "Inventory", "Orders", "Customers", "CreditRisks"
            };

            foreach (var item in tables)
            {
                var rawSqlString = $"DBCC CHECKIDENT (\"dbo.{item}\", RESEED, -1);";
                context.Database.ExecuteSqlCommand(rawSqlString);
            }

        }

        public static void InitializeData(AutoLotContext context)
        {
            var customers = new List<Customer>
            {
                new Customer { FirstName = "Dave", LastName = "Brenner"},
                new Customer { FirstName = "Matt", LastName = "Walton"},
                new Customer { FirstName = "Steve", LastName = "Hagen"},
                new Customer { FirstName = "Pat", LastName = "Walton"},
                new Customer { FirstName = "Bad", LastName = "Customer"},
            };
            customers.ForEach(x => context.Customers.Add(x));
            context.SaveChanges();

            var cars = new List<Inventory>
            {
                new Inventory { Make = "VW", Color = "Black", PetName = "Zippy"},
                new Inventory { Make = "Ford", Color = "Rust", PetName = "Rusty"},
                new Inventory { Make = "Saab", Color = "Back", PetName = "Mel"},
                new Inventory { Make = "Yugo", Color = "Yellow", PetName = "Clunker"},
                new Inventory { Make = "BMW", Color = "Black", PetName = "Bimmer"},
                new Inventory { Make = "BMW", Color = "Green", PetName = "Hank"},
                new Inventory { Make = "BMW", Color = "Pink", PetName = "Pinky"},
                new Inventory { Make = "Pinto", Color = "Black", PetName = "Pete"},
                new Inventory { Make = "Yugo", Color = "Brown", PetName = "Brownie"},
            };
            context.Cars.AddRange(cars);
            context.SaveChanges();

            var orders = new List<Order>
            {
                new Order{Car = cars[0], Customer = customers[0]},
                new Order{Car = cars[1], Customer = customers[1]},
                new Order{Car = cars[2], Customer = customers[2]},
                new Order{Car = cars[3], Customer = customers[3]},
            };
            orders.ForEach(x => context.Orders.Add(x));
            context.SaveChanges();

            context.CreditRisks.Add(new CreditRisk
            {
                Id = customers[4].Id,
                FirstName = customers[4].FirstName,
                LastName = customers[4].LastName
            });

            context.Database.OpenConnection();

            try
            {
                var tableName = context.GetTableName(typeof(CreditRisk));

                var rawSqlString = $"SET IDENTITY_INSERT dbo.{tableName} ON;";
                context.Database.ExecuteSqlCommand(rawSqlString);
                context.SaveChanges();

                rawSqlString = $"SET IDENTITY_INSERT dbo.{tableName} OFF;";
                context.Database.ExecuteSqlCommand(rawSqlString);
            }
            finally
            {
                context.Database.CloseConnection();
            }
        }
    }
}
