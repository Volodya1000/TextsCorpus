using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexstsCorpus.Model.Entities;

namespace TexstsCorpus.Model.Configurations;

public class GrammarCategoryConfiguration
    : IEntityTypeConfiguration<GrammarCategory>
{
    public void Configure(EntityTypeBuilder<GrammarCategory> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Key)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(g => g.Value)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(g => new { g.Key, g.Value });
    }
}