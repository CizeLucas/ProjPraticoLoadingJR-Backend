using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
  //[Required]
        //[StringLength(36, MinimumLength = 36, ErrorMessage = "The UUID must have a exact lenght of 36 characters")]
namespace PublicationsAPI.Models
{
	public class Users
	{
        [Key]
        public int Id { get; set; }
        //[Required]
        //[StringLength(36, MinimumLength = 36, ErrorMessage = "The UUID must have a exact lenght of 36 characters")]
		public string Uuid { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true)]
        [MaxLength(300)]
        public string Bio { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        public string? ImageUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(100)]
        public string PasswordHash { get; set; } = string.Empty;

        //  ONE-TO-MANY DB relationship
        public ICollection<Publications>? UserPublications { get; set; }
    }
}
