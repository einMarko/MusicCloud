using System.ComponentModel.DataAnnotations;

namespace MusicCloud.Webapp.Dto
{
    class ValidBirthday : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var artist = validationContext.ObjectInstance as ArtistDto;
            if (artist is null) { return null; }

            if (artist.Birthday > DateTime.Now)
                return new ValidationResult("The birthdate can not be in the future.");

            return ValidationResult.Success;
        }
    }
    public record ArtistDto(
        Guid Guid,
        [StringLength(30, MinimumLength = 2, ErrorMessage = "The artist name must be between 2 and 30 characters long")]
        string ArtistName,
        [StringLength(30, MinimumLength = 2, ErrorMessage = "The first name must be between 2 and 30 characters long")]
        string FirstName,
        [StringLength(30, MinimumLength = 2, ErrorMessage = "The last name must be between 2 and 30 characters long")]
        string LastName,
        [ValidBirthday]
        DateTime Birthday);
}
