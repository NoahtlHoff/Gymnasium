using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Gymnasium.Data
{
    internal class GymDbContextFactory : IDesignTimeDbContextFactory<GymDbContext>
    {
        public GymDbContext CreateDbContext(string[] args)
        {
            // Read the connection string from your file
            string connectionString = File.ReadAllText(@"C:\connectionString.txt").Trim();

            var optionsBuilder = new DbContextOptionsBuilder<GymDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new GymDbContext(optionsBuilder.Options);
        }
    }
}
