using Microsoft.EntityFrameworkCore;
using TexstsCorpus.Model;
using TexstsCorpus.Model.Entities;

namespace TextsCorpus.Model.Repositories;
public interface ITextsRepository
{
    Task<int> AddTextAsync(Text text);
    Task<Text?> GetTextWithDetailsAsync(int id);
    Task<(string Content, int TotalPages, int TotalTokens)> GetPaginatedContentAsync(
        int textId,
        int page,
        int pageSize);
}

public class TextsRepository : ITextsRepository
{
    private readonly CorpusDbContext _context;

    public TextsRepository(CorpusDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddTextAsync(Text text)
    {
        await _context.Texts.AddAsync(text);
        await _context.SaveChangesAsync();
        return text.Id;
    }

    public async Task<Text?> GetTextWithDetailsAsync(int id)
    {
        return await _context.Texts
            .Include(t => t.Author)
            .Include(t => t.Sentences)
                .ThenInclude(s => s.Tokens)
                    .ThenInclude(t => t.GrammarCategories)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<(string Content, int TotalPages, int TotalTokens)> GetPaginatedContentAsync(
        int textId,
        int page,
        int pageSize)
    {
        var text = await _context.Texts
            .Include(t => t.Sentences)
                .ThenInclude(s => s.Tokens)
            .FirstOrDefaultAsync(t => t.Id == textId);

        if (text == null) throw new KeyNotFoundException("Text not found");

        var allTokens = text.Sentences
            .SelectMany(s => s.Tokens)
            .OrderBy(t => t.IndexInText)
            .ToList();

        var totalTokens = allTokens.Count;
        var totalPages = (int)Math.Ceiling(totalTokens / (double)pageSize);

        var pagedTokens = allTokens
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var content = string.Join("", pagedTokens.Select(t => t.TokenText));

        return (content, totalPages, totalTokens);
    }
}
