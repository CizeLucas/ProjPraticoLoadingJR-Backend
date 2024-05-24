using System.ComponentModel.DataAnnotations;
using PublicationsAPI.Models;

namespace PublicationsAPI.DTO.Publication
{
	public class LoggedInPublicationDTO
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
        public int PublicationType { get; set; } //Integer for specifying publication type of the publication

        [MaxLength(150)]
        public string? ImageURL { get; set; } //Image url of the publication

        [Required]
        public DateTime CreatedAt { get; set; } //Date of the creation of the publication

        [Required]
        public DateTime UpdatedAt { get; set; } //Date of the lastest update of the publication

        //  ONE-TO-ONE DB relationship
        public int? AuthorId { get; set; } //Foreing Key
        public Users? Author { get; set; } //Navigation proprierty

	}
}