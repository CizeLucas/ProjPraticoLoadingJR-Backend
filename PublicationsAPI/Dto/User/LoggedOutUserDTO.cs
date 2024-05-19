using System;
using System.ComponentModel.DataAnnotations;

namespace PublicationsAPI.DTO.User
{
	public class LoggedOutUserDTO
	{
		//[Required]
        //[StringLength(36, MinimumLength = 36, ErrorMessage = "The UUID must have a exact lenght of 36 characters")]
		public string Uuid { get; set; } = string.Empty;

		//[Required]
        //[MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        //[Required]
        //[MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        //[Required(AllowEmptyStrings = true)]
        //[MaxLength(300)]
        public string Bio { get; set; } = string.Empty;

        //[Required(AllowEmptyStrings = true)]
        //[MaxLength(100)]
        public string? ImageUrl { get; set; } = string.Empty;

	}
}
