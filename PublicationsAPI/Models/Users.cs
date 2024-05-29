using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PublicationsAPI.Models
{
    /*
    *   Users have all those properties below, but the ones that are commented mean 
    *   that they do not come from here, but from the IdentityUser ASP.NET Core class
    */
	public class Users : IdentityUser<int>
	{
        //[Key]
        //public int Id { get; set; }

        [Required]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "The UUID must have a exact lenght of 36 characters")]
		public string Uuid { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        //[Required]
        //[MaxLength(50)]
        //public string Username { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true)]
        [MaxLength(300)]
        public string Bio { get; set; }

        //[Required]
        //[MaxLength(100)]
        //public string Email { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true)]
        public string? ImageUrl { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        //  ONE-TO-MANY DB relationship
        public ICollection<Publications>? UserPublications { get; set; }
    }

}
