using System.ComponentModel.DataAnnotations;

namespace PublicationsAPI.DTO.Publication
{
	public class PublicationDTO
	{

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } //Title of the publication

        [MaxLength(300)]
		public string Description { get; set; } //Description of the publication

        [Required]
        public int PublicationType { get; set; } //Integer for specifying publication type of the publication

        [MaxLength(150)]
        public string? ImageURL { get; set; } //Image url of the publication

	}
}
