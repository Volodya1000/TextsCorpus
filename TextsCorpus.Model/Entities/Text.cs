namespace TexstsCorpus.Model.Entities;

public class Text
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime PublicationDate { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public List<Sentence> Sentences { get; set; } = new List<Sentence>();
}
