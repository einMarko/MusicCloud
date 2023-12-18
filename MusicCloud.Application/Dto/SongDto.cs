using MusicCloud.Application.infrastructure;
using MusicCloud.Application.models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MusicCloud.Webapp.Dto
{
    class ValidLength : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var song = validationContext.ObjectInstance as SongDto;
            if (song is null) { return null; }

            if (string.IsNullOrWhiteSpace(song.Length))
                return new ValidationResult("The Length field is required");

            if (!song.Length.Contains(":"))
                return new ValidationResult("No seperator exists");

            if (song.Length.Count(a => a == ':') > 1)
                return new ValidationResult("Too many seperator exist");

            string[] parts = song.Length.Split(':');
            if (parts.Length != 2 ||
                !int.TryParse(parts[0], out int minutes) ||
                !int.TryParse(parts[1], out int seconds) ||
                minutes < 0 || minutes > 59 ||
                seconds < 0 || seconds > 59)
                return new ValidationResult("Invalid format for Length");

            return ValidationResult.Success;
        }
    }

    public class ValidSongReleaseDate : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var song = validationContext.ObjectInstance as SongDto;

            if (song == null || value == null)
            {
                return null;
            }

            var dbContext = (CloudContext)validationContext.GetService(typeof(CloudContext));
            var albumReleaseDate = dbContext.Albums
                .Where(a => a.Guid == song.AlbumGuid)
                .Select(a => a.ReleaseDate)
                .FirstOrDefault();

            if (song.ReleaseDate > albumReleaseDate)
            {
                return new ValidationResult("The release date of the song cannot be later than the release date of the album.");
            }

            return ValidationResult.Success;
        }
    }

    public record SongDto(
        Guid Guid,
        [StringLength(100, MinimumLength = 2, ErrorMessage = "A song title must be between 2 and 100 characters long")]
        string Title,
        Guid ArtistGuid,
        Guid AlbumGuid,
        Guid GenreGuid,
        [ValidSongReleaseDate]
        DateTime ReleaseDate,
        [ValidLength]
        string Length);
}
