using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.DbEntities;

namespace Data.Mapping
{
    public class CollectionMap : MappingEntityTypeConfiguration<Collection>
    {
        public override void Configure(EntityTypeBuilder<Collection> builder)
        {
            builder.ToTable("Collection");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.RestaurantId).IsRequired();
            builder.Property(p => p.RestaurantName).HasMaxLength(255).IsRequired();
            builder.Property(p => p.FavoriteCollectionId).IsRequired(false);
            builder.Property(p => p.CreateUTC).HasColumnType("DateTime").HasDefaultValueSql("GetUtcDate()");
            base.Configure(builder);
        }
    }
}
