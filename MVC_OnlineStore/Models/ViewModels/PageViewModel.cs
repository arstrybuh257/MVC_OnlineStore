using MVC_OnlineStore.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_OnlineStore.Models.ViewModels
{
    public class PageViewModel
    {
        public PageViewModel()
        {

        }
        public PageViewModel(Page page)
        {
            PageId = page.PageId;
            Title = page.Title;
            Description = page.Description;
            Body = page.Body;
            Sorting = page.Sorting;
        }
        public int PageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public int Sorting { get; set; }
    }
}