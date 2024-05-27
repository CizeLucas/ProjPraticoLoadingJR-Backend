using System;
using System.ComponentModel.DataAnnotations;

namespace PublicationsAPI.DTO.UserDTOs
{
	public class UserRequest
	{
		//[Required]
        //[MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        //[Required]
        //[MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        //[Required(AllowEmptyStrings = true)]
        //[MaxLength(300)]
        public string Bio { get; set; } = string.Empty;

        //[Required]
        //[MaxLength(100)]
        //public string Email { get; set; } = string.Empty;

        //[Required(AllowEmptyStrings = true)]
        //[MaxLength(100)]
        //public string? ImageUrl { get; set; } = string.Empty;
	}
}
