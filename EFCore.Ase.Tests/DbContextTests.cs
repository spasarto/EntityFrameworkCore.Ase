using EntityFrameworkCore.Ase.Tests.Infastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using EntityFrameworkCore.Ase;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Diagnostics;
using System.IO;

namespace EntityFrameworkCore.Ase.Tests
{
    [TestClass]
    public class DbContextTests
    {
        private static AseOptions _options;
        private static PoorMansMigration _migrations;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestConfiguration.Initialize();
            _options = TestConfiguration.GetOptions<AseOptions>().Value;
            _migrations = new PoorMansMigration(_options);

            _migrations.Up();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _migrations.Down();
        }

        [TestMethod]
        public void TestDbContextConnection()
        {
            var context = new TestDbContext(_options.ConnectionString);
            var orders = context.Set<Models.Order>().ToList();
            Assert.AreEqual(1, orders.Count);
        }


        [TestMethod]
        public void TestSaveChanges()
        {
            var newName = Guid.NewGuid().ToString();
            using (var context = new TestDbContext(_options.ConnectionString))
            {
                var set = context.Set<Models.Order>();
                var order = set.First();
                order.Name = newName;

                context.SaveChanges();
            }

            // requery to ensure data was actually changed.
            using (var context = new TestDbContext(_options.ConnectionString))
            {
                var set = context.Set<Models.Order>();
                var order = set.First();

                Assert.AreEqual(newName, order.Name);
            }
        }

        class TestDbContext : DbContext
        {
            private readonly string _connString;

            public TestDbContext(string connString)
            {
                _connString = connString;
            }
            
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                base.OnConfiguring(optionsBuilder);

                optionsBuilder.UseAse(_connString);
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.ApplyConfiguration(new Models.OrderConfiguration());
            }
        }
    }
}
