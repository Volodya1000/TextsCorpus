namespace TexstsCorpus.Model.Entities;

public class GrammarCategory
{
    public int Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public int TokenId { get; set; }
    public Token Token { get; set; }
}