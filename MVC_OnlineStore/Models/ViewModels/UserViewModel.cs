using MVC_OnlineStore.Models.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_OnlineStore.Models.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel() { }

        public UserViewModel(User model)
        {
            Id = model.Id;
            FirstName = model.FirstName;
            SecondName = model.SecondName;
            EmailAdress = model.EmailAdress;
            Username = model.Username;
            Password = model.Password;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecondName { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAdress { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}