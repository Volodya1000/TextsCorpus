using System.Text.Json;
using TexstsCorpus.Model.Entities;
using TexstsCorpus.Model.Enums;
using TexstsCorpus.Model;
using Microsoft.EntityFrameworkCore;

namespace TextsCorpus.Model;

public class TextImporter
{
    private readonly CorpusDbContext _context;

    public TextImporter(CorpusDbContext context)
    {
        _context = context;
    }

    public async Task<int> ImportTextAsync(
        string jsonData,
        string title,
        string authorName,
        DateTime publicationDate)
    {
        var textData = JsonSerializer.Deserialize<TextData>(jsonData);

        var author = await _context.Authors
            .FirstOrDefaultAsync(a => a.Name == authorName)
            ?? new Author { Name = authorName };

        var text = new Text
        {
            Title = title,
            PublicationDate = publicationDate,
            Author = author,
            Sentences = new List<Sentence>()
        };

        int globalTokenIndex = 0;

        foreach (var sentenceData in textData.Sentences)
        {
            var sentence = new Sentence
            {
                OrderInText = text.Sentences.Count + 1,
                Tokens = new List<Token>()
            };

            foreach (var tokenData in sentenceData.Tokens)
            {
                var token = new Token
                {
                    TokenText = tokenData.Token,
                    Type = MapTokenType(tokenData.Type),
                    IndexInText = globalTokenIndex++,
                    Lemma = tokenData.Lemma ?? string.Empty,
                    GrammarCategories = new List<GrammarCategory>()
                };

                if (tokenData.GrammarCategories != null)
                {
                    foreach (var category in tokenData.GrammarCategories)
                    {
                        token.GrammarCategories.Add(new GrammarCategory
                        {
                            Key = category.Key,
                            Value = category.Value
                        });
                    }
                }

                sentence.Tokens.Add(token);
            }

            text.Sentences.Add(sentence);
        }

        await _context.Texts.AddAsync(text);
        await _context.SaveChangesAsync();

        return text.Id;
    }

    private TokenType MapTokenType(string type) => type switch
    {
        "russian" => TokenType.Russian,
        "number" => TokenType.Number,
        "punct" => TokenType.Punct,
        _ => TokenType.Foreign
    };
}

file class TextData
{
    public List<SentenceData> Sentences { get; set; }
}

file class SentenceData
{
    public List<TokenData> Tokens { get; set; }
}

file class TokenData
{
    public int Index { get; set; }
    public string Token { get; set; }
    public string Type { get; set; }
    public string Lemma { get; set; }
    public Dictionary<string, string> GrammarCategories { get; set; }
}
