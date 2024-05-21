using System;
using System.ComponentModel.DataAnnotations;

namespace PublicationsAPI.DTO.UserDTOs
{
	public class LoggedOutUserResponse
	{
		public string Uuid { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
	}
}
