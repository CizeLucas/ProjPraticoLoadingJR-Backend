using System;
using System.ComponentModel.DataAnnotations;

namespace PublicationsAPI.DTO.UserDTOs
{
	public class LoggedInUserResponse
	{
		public string Uuid { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
		public DateTime CreatedtAt { get; set; } = DateTime.Now;

	}
}
