using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using TexstsCorpus.Model.Entities;

namespace TexstsCorpus.Model.Configurations;

public class TokenConfiguration : IEntityTypeConfiguration<Token>
{
    public void Configure(EntityTypeBuilder<Token> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.TokenText)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.IndexInText)
            .IsRequired();

        // Настройка связи один-ко-многим
        builder.HasMany(t => t.GrammarCategories)
            .WithOne(g => g.Token)
            .HasForeignKey(g => g.TokenId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.Sentence)
            .WithMany(s => s.Tokens)
            .HasForeignKey(t => t.SentenceId);
    }
}
