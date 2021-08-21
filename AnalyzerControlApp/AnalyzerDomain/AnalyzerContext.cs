using AnalyzerDomain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerDomain
{
    public class AnalyzerContext : DbContext
    {
        public DbSet<Cartridge> Cartridges { get; set; }
        public DbSet<AnalysisType> AnalysisTypes { get; set; }
        public DbSet<Analysis> SheduledAnalyzes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=Analyzer.db");
    }
}
