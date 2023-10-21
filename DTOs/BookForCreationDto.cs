using System.ComponentModel.DataAnnotations;

namespace books.DTOs
{
    public class BookForCreationDto
    {
        [Required]
        [MaxLength(32)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [MaxLength(32)]
        public string Title { get; set; } = string.Empty;
    }
}
