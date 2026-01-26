using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class GiteBedConfiguration : IEntityTypeConfiguration<GiteBed>
{
    public void Configure(EntityTypeBuilder<GiteBed> builder)
    {
        builder.ToTable("tblGiteBed");

        builder.HasKey(x => x.GiteBedId);

        builder.Property(x => x.GiteBedId)
               .HasColumnName("giteBedId");

        builder.Property(x => x.GiteId)
               .HasColumnName("giteId")
               .IsRequired();

        builder.Property(x => x.Amount1PrBed)
               .HasColumnName("amount1PrBed")
               .IsRequired();

        builder.Property(x => x.Amount2PrBed)
               .HasColumnName("amount2PrBed")
               .IsRequired();

        builder.Property(x => x.Amount3PrBed)
               .HasColumnName("amount3PrBed")
               .IsRequired();

        builder.Property(x => x.BedSort)
               .HasColumnName("bedSort")
               .HasColumnType("text")
               .IsRequired();
    }
}
