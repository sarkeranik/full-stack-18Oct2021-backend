using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.DbEntities;

namespace Data.Mapping
{
    public class RestaurantDetailsMap : MappingEntityTypeConfiguration<RestaurantDetails>
    {
        public override void Configure(EntityTypeBuilder<RestaurantDetails> builder)
        {
            builder.ToTable("RestaurantDetails");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasMaxLength(255);
            builder.Property(p => p.DayOfWeeKId);
            builder.Property(p => p.OpeningTime).HasMaxLength(255);
            builder.Property(p => p.ClosingTime).HasMaxLength(255);
            builder.Property(p => p.TimePeriod);
            builder.Property(p => p.CreateUTC).HasColumnType("DateTime").HasDefaultValueSql("GetUtcDate()");
            base.Configure(builder);
        }
    }
}
