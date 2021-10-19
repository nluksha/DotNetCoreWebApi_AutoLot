using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore_AutoLotDAL.EF
{
    public class AutoLotContextFactory : IDesignTimeDbContextFactory<AutoLotContext>
    {
        public AutoLotContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AutoLotContext>();

            var connectionString = @"server=.; database=AutoLotCore2; integrated security=True; MultipleActiveResultSets=True; App=EntityFramework";
            optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure())
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));

            return new AutoLotContext(optionsBuilder.Options);
        }
    }
}
