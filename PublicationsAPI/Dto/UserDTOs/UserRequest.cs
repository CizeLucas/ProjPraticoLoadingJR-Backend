using System.ComponentModel.DataAnnotations;

#pragma warning disable //disables warnings on this file

namespace PublicationsAPI.DTO.UserDTOs
{
	public class UserRequest
	{
		[Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true)]
        [MaxLength(300)]
        public string Bio { get; set; } = string.Empty;
	}
}
