using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TexstsCorpus.Model.Entities;

namespace TexstsCorpus.Model.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasMany(a => a.Texts)
            .WithOne(t => t.Author)
            .HasForeignKey(t => t.AuthorId);
    }
}
