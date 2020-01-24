using MVC_OnlineStore.Models.DataModels;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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
            HasSlidebar = page.HasSlidebar;
        }
        public int PageId { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        [MinLength(3)]
        [AllowHtml]
        public string Body { get; set; }
        public int Sorting { get; set; }
        public bool HasSlidebar { get; set; }
    }
}