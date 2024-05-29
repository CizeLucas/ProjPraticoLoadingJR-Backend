using System.ComponentModel.DataAnnotations;

#pragma warning disable //disables warnings on this file

namespace PublicationsAPI.DTO.PublicationDTOs
{
	public class PublicationResponseDTO
	{
        [Required]
        [MaxLength(40)]
		public string Uuid { get; set; } //UUID V4 for publicly accessing and identifying users

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } //Title of the publication

        [MaxLength(300)]
		public string Description { get; set; } //Description of the publication

        [Required]
        public string PublicationType { get; set; } //String for specifying publication type of the publication

        [MaxLength(150)]
        public string? ImageURL { get; set; } //Image url of the publication

        [Required]
        public DateTime CreatedAt { get; set; } //Date of the creation of the publication

        [Required]
        public DateTime UpdatedAt { get; set; } //Date of the lastest update of the publication]

        [Required]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "The UUID must have a exact lenght of 36 characters")]
        public string? AuthorUuid { get; set; } //Foreing Key

	}
}
