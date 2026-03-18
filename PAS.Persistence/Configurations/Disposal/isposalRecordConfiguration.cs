using Domain.Catalog;
using Domain.Disposal;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Disposal
{
    public class DisposalRecordConfiguration : IEntityTypeConfiguration<DisposalRecord>
    {
        public void Configure(EntityTypeBuilder<DisposalRecord> builder)
        {
            builder.ToTable("DisposalRecords");

            builder.Property(d => d.Reason)
                .HasMaxLength(500);

            builder.HasOne<ItemMaster>()
                .WithMany()
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne<UserLogin>()
            //    .WithMany()
            //    .HasForeignKey(d => d.DisposedBy)
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(d => d.DisposalDate);
        }
    }
}