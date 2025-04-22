using System.Text.Json;
using TexstsCorpus.Model.Entities;
using TexstsCorpus.Model.Enums;
using TexstsCorpus.Model;
using Microsoft.EntityFrameworkCore;
using TextsCorpus.Model.Repositories;

namespace TextsCorpus.Model;

public class TextImporter
{
    private readonly ITextsRepository _repository;

    public TextImporter(ITextsRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> ImportTextAsync(
        string jsonData,
        string title,
        string authorName,
        DateTime publicationDate)
    {
        var textData = JsonSerializer.Deserialize<TextData>(jsonData);
        var author = new Author { Name = authorName };

        var text = new Text
        {
            Title = title,
            PublicationDate = publicationDate,
            Author = author,
            Sentences = textData.Sentences.Select((s, i) => new Sentence
            {
                OrderInText = i + 1,
                Tokens = s.Tokens.Select(t => MapToken(t)).ToList()
            }).ToList()
        };

        return await _repository.AddTextAsync(text);
    }

    private Token MapToken(TokenData tokenData)
    {
        return new Token
        {
            TokenText = tokenData.Token,
            Type = MapTokenType(tokenData.Type),
            IndexInText = tokenData.Index,
            Lemma = tokenData.Lemma ?? string.Empty,
            GrammarCategories = tokenData.GrammarCategories?
                .Select(g => new GrammarCategory
                {
                    Key = g.Key,
                    Value = g.Value
                })
                .ToList() ?? new List<GrammarCategory>()
        };
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

public class TokenData
{
    public int Index { get; set; }
    public string Token { get; set; }
    public string Type { get; set; }
    public string Lemma { get; set; }
    public Dictionary<string, string> GrammarCategories { get; set; }
}
