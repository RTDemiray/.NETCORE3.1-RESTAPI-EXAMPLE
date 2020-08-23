using FlutterApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlutterApp.Data.EntityFramework
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Categories> Categories { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<Scores> Scores { get; set; }
    }
}
