using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexstsCorpus.Model.Entities;

namespace TexstsCorpus.Model.Configurations;

public class SentenceConfiguration : IEntityTypeConfiguration<Sentence>
{
    public void Configure(EntityTypeBuilder<Sentence> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.OrderInText)
            .IsRequired();

        builder.HasOne(s => s.Text)
            .WithMany(t => t.Sentences)
            .HasForeignKey(s => s.TextId);
    }
}
