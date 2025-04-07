using Microsoft.EntityFrameworkCore;
using TexstsCorpus.Model;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddDbContext<CorpusDbContext>(options =>
        options.UseSqlite("TextsCorpus.db"));


services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();

