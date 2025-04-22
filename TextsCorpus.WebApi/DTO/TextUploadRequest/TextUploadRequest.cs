using System.ComponentModel.DataAnnotations;

namespace TextsCorpus.WebApi.DTO.TextUploadRequest;

public record TextUploadRequest(
    [Required] string Title,
    [Required] string Author,
    [Required] DateTime PublicationDate,
    [Required] IFormFile JsonFile
);