using System;

namespace PublicationsAPI.Models { 
	public class Publications
	{
		public int Id { get; set; } //ID auto-incremented by the DB for internal use (like pagination)

		public string Uuid { get; set; } //UUID V4 for publicly accessing and identifying users

        public string Title { get; set; } //Title of the publication

		public string Description { get; set; } //Description of the publication

        public DateTime CreatedAt { get; set; } = DateTime.Now; //Date of the creation of the publication

        public DateTime UpdatedAt { get; set; } = DateTime.Now; //Date of the lastest update of the publication

        public int PublicationType { get; set; } //Integer for specifying publication type of the publication

        public string ImageURL { get; set; } = string.Empty; //Image url of the publication

        //  ONE-TO-ONE DB relationship
        public int? AuthorId { get; set; } //Foreing Key
        public Users? Author { get; set; } //Navigation proprierty

    }
}
