using TexstsCorpus.Model.Enums;

namespace TexstsCorpus.Model.Entities;

public class Token
{
    public int Id { get; set; }
    public string TokenText { get; set; }
    public TokenType Type { get; set; }
    public int IndexInText { get; set; }
    public string Lemma { get; set; }

    public List<GrammarCategory> GrammarCategories { get; set; } = new();
    public int SentenceId { get; set; }
    public Sentence Sentence { get; set; }
}