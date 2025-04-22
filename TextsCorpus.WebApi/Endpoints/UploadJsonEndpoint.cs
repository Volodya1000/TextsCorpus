using System.Text.Json;
using System;
using TextsCorpus.Model;
using TexstsCorpus.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TextsCorpus.WebApi.DTO.TextUploadRequest;

namespace TextsCorpus.WebApi.Endpoints;
public static class UploadJsonEndpoint
{
    public static void AddUploadJsonExtensions(this WebApplication app)
    {
        app.MapPost("/api/texts", HandleUpload)
            .Accepts<TextUploadRequest>("multipart/form-data")
            .Produces<Results<Created, BadRequest>>()
            .WithTags("Texts")
            .WithOpenApi();
    }

    private static async Task<IResult> HandleUpload(
        HttpContext context,
        CorpusDbContext dbContext)
    {
        var form = await context.Request.ReadFormAsync();

        var (title, author, date, file) = ParseFormData(form);
        var validationError = ValidateInput(title, author, date, file);
        if (validationError != null) return validationError;

        var jsonContent = await ReadJsonContent(file!);
        if (!jsonContent.IsValid) return jsonContent.Error!;

        try
        {
            var importer = new TextImporter(dbContext);
            var textId = await importer.ImportTextAsync(
                jsonContent.Data!,
                title!,
                author!,
                date!.Value);

            return TypedResults.Created($"/api/texts/{textId}");
        }
        catch (JsonException ex)
        {
            return TypedResults.BadRequest($"Invalid JSON format: {ex.Message}");
        }
        catch (DbUpdateException ex)
        {
            return TypedResults.BadRequest($"Database error: {ex.InnerException?.Message}");
        }
    }

    private static (string?, string?, DateTime?, IFormFile?) ParseFormData(IFormCollection form) => (
        form["title"],
        form["author"],
        DateTime.TryParse(form["publicationDate"], out var date) ? date : null,
        form.Files.GetFile("jsonFile")
    );

    private static IResult? ValidateInput(
        string? title,
        string? author,
        DateTime? date,
        IFormFile? file)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(title))
            errors.Add("Title is required");

        if (string.IsNullOrWhiteSpace(author))
            errors.Add("Author is required");

        if (!date.HasValue)
            errors.Add("Publication date is required");

        if (file == null || file.Length == 0)
            errors.Add("JSON file is required");
        else if (Path.GetExtension(file.FileName)?.ToLower() != ".json")
            errors.Add("Only JSON files are allowed");

        return errors.Count > 0
            ? TypedResults.BadRequest(string.Join(", ", errors))
            : null;
    }

    private static async Task<(string? Data, bool IsValid, IResult? Error)> ReadJsonContent(IFormFile file)
    {
        try
        {
            using var reader = new StreamReader(file.OpenReadStream());
            return (await reader.ReadToEndAsync(), true, null);
        }
        catch
        {
            return (null, false, TypedResults.BadRequest("Error reading file content"));
        }
    }
}





