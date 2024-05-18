using System;

namespace PublicationsAPI.Models
{
	public class Users
	{
        public int Id { get; set; }

		public string Uuid { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public DateTime CreatetAt { get; set; } = DateTime.Now;

        public string Bio { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        //  ONE-TO-MANY DB relationship
        public ICollection<Publications> UserPublications { get; set; }
    }
}
