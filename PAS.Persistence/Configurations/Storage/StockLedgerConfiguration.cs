using Domain.Catalog;
using Domain.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Storage
{
    public class StockLedgerConfiguration : IEntityTypeConfiguration<StockLedger>
    {
        public void Configure(EntityTypeBuilder<StockLedger> builder)
        {
            builder.ToTable("StockLedgers");

            builder.Property(s => s.TransactionType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne<ItemMaster>()
                .WithMany()
                .HasForeignKey(s => s.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ShelfLocation>()
                .WithMany()
                .HasForeignKey(s => s.ShelfId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => s.ReferenceId);
            builder.HasIndex(s => s.CreatedDate);
        }
    }
}