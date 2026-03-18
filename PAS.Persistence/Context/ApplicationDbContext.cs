using Domain.Catalog;
using Domain.PropertyManagement;
using Domain.Requisition;
using Domain.Storage;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Identity;
using System.Reflection.Emit;

namespace Persistence.Context
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Catalog
        public DbSet<Category> Categories { get; set; }
        public DbSet<ItemMaster> ItemMasters { get; set; }

        // Storage
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<InventoryStock> InventoryStocks { get; set; }
        public DbSet<StockLedger> StockLedgers { get; set; }

        // Property
        public DbSet<Property> Properties { get; set; }

        // Requisition
        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationDbContext).Assembly
            );
        }
    }
}