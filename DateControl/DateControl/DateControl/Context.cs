using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.IO;
using Xamarin.Forms;
using DateControl.Calendar;

namespace DateControl
{
    class Context : DbContext
    {
        private const string DatabaseName = "database.db";

        public DbSet<Event> Events { get; set; }

        public Context()
        {
            Database.EnsureCreated();
            Events.CountAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string databasePath;
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseName);
                    break;
                case Device.iOS:
                    Batteries.Init();
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", DatabaseName);
                    break;
                case Device.UWP:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DatabaseName);
                    break;
                default:
                    throw new NotImplementedException("Platform not supported");
            }
            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().Property(p => p.DateTime).HasColumnType("datetime2");
        }
    }
}
