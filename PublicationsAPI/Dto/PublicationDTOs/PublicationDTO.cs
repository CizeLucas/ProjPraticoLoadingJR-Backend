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
        public string PublicationType { get; set; } //Integer for specifying publication type of the publication

	}
}
