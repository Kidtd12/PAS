using Domain.Receiving;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Receiving
{
    public class InspectionLogConfiguration : IEntityTypeConfiguration<InspectionLog>
    {
        public void Configure(EntityTypeBuilder<InspectionLog> builder)
        {
            builder.ToTable("InspectionLogs");

            builder.Property(i => i.DeviationNotes)
                .HasMaxLength(1000);

            builder.HasOne<ReceivingNote>()
                .WithMany()
                .HasForeignKey(i => i.ReceivingNoteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<UserLogin>()
                .WithMany()
                .HasForeignKey(i => i.InspectorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(i => i.ReceivingNoteId).IsUnique();
        }
    }
}