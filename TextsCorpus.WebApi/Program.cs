using Microsoft.EntityFrameworkCore;
using TexstsCorpus.Model;
using TextsCorpus.Model;
using TextsCorpus.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddDbContext<CorpusDbContext>(options =>
        options.UseSqlite("TextsCorpus.db"));


services.AddScoped<TextImporter>();


services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

app.AddApplicationEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();

