using MusicCloud.Application.models;
using System.ComponentModel.DataAnnotations;

namespace MusicCloud.Webapp.Dto
{
    class ValidAlbumReleaseDate : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var album = validationContext.ObjectInstance as AlbumDto;
            if (album is null) { return null; }

            if (album.ReleaseDate > DateTime.Now)
                return new ValidationResult("The release date can not be in the future.");

            return ValidationResult.Success;
        }
    }

    public record AlbumDto(
        Guid Guid,
        Guid ArtistGuid,
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The album title must be between 2 and 100 characters long")]
        string Title,
        [ValidAlbumReleaseDate]
        DateTime ReleaseDate)
    {
    }
}
