using MVC_OnlineStore.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_OnlineStore.Models.ViewModels
{
    public class SideBarViewModel
    {
        public SideBarViewModel() { }
        public SideBarViewModel(SideBar sb)
        {
            Id = sb.Id;
            Body = sb.Body;
        }
        public int Id { get; set; }
        [AllowHtml]
        public string Body { get; set; }
    }
}