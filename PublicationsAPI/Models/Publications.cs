using System.ComponentModel.DataAnnotations;

namespace PublicationsAPI.Models { 
	public class Publications
	{   
        [Key]
        [Required]
		public int Id { get; set; } //ID auto-incremented by the DB for internal use (like pagination)
        
        [Required]
        [MaxLength(40)]
		public string Uuid { get; set; } = string.Empty; //UUID V4 for publicly accessing and identifying users

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty; //Title of the publication

        [MaxLength(300)]
		public string Description { get; set; } = string.Empty; //Description of the publication

        [Required]
        public int PublicationType { get; set; } //Integer for specifying publication type of the publication

        [MaxLength(150)]
        public string? ImageURL { get; set; } = string.Empty; //Image url of the publication

        [Required]
        public DateTime CreatedAt { get; set; } //Date of the creation of the publication

        [Required]
        public DateTime UpdatedAt { get; set; } //Date of the lastest update of the publication

        //  ONE-TO-ONE DB relationship
        [Required]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "The UUID must have a exact lenght of 36 characters")]
        public string? AuthorUuid { get; set; } //Foreing Key
        public Users? Author { get; set; } //Navigation proprierty

    }
}
