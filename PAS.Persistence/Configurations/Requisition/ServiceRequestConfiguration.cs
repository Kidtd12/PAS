
using Domain.Requisition;

﻿using Domain.Requisition;

using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Requisition
{
    public class ServiceRequestConfiguration : IEntityTypeConfiguration<ServiceRequest>
    {
        public void Configure(EntityTypeBuilder<ServiceRequest> builder)
        {
            builder.ToTable("ServiceRequests");

            builder.Property(s => s.SRNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne<UserLogin>()
                .WithMany()
                .HasForeignKey(s => s.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<UserLogin>()
                .WithMany()
                .HasForeignKey(s => s.ApprovedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => s.SRNumber).IsUnique();
            builder.HasIndex(s => s.Status);
        }
    }
}