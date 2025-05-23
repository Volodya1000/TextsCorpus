﻿namespace TexstsCorpus.Model.Entities;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Text> Texts { get; set; } = new List<Text>();
}
