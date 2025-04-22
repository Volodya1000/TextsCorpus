using TexstsCorpus.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace TexstsCorpus.Model;

public class CorpusDbContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Text> Texts { get; set; }
    public DbSet<Sentence> Sentences { get; set; }
    public DbSet<Token> Tokens { get; set; }

    public CorpusDbContext(DbContextOptions<CorpusDbContext> options)
             : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}

