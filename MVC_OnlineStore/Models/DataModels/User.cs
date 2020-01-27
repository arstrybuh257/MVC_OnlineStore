using System.ComponentModel.DataAnnotations;

namespace MVC_OnlineStore.Models.DataModels
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecondName { get; set; }
        [EmailAddress]
        [Required]
        public string EmailAdress { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string Theme { get; set; }
    }
}