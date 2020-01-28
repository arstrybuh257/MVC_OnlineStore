using MVC_OnlineStore.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_OnlineStore.Models.ViewModels
{
    public class AdminViewModel : UserProfileViewModel
    {
        public AdminViewModel() { }
        public AdminViewModel(User user): base(user)
        {
            Theme = user.Theme;
        }
        public string Theme { get; set; }
    }
}