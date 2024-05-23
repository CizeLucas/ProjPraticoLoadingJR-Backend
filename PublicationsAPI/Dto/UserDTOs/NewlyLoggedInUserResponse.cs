using System;
using System.ComponentModel.DataAnnotations;

namespace PublicationsAPI.DTO.UserDTOs
{
	public class NewlyLoggedInUserResponse
	{
		public string Uuid { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
        public int expiresInMinutes { get; set; } = 0;
		public DateTime issuedAt { get; set; }

	}
}
