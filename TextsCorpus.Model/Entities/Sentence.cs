namespace TexstsCorpus.Model.Entities;

public class Sentence
{
    public int Id { get; set; }
    public int OrderInText { get; set; }
    public int TextId { get; set; }
    public Text Text { get; set; }
    public List<Token> Tokens { get; set; } = new List<Token>();
}
