using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_OnlineStore.Models.ViewModels
{
    public class LoginUserViewModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [DisplayName("Remember me")]
        public bool RememberMe { get; set; }
    }
}