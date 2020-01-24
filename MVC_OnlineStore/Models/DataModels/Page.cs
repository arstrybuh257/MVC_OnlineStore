using System.ComponentModel.DataAnnotations;

namespace MVC_OnlineStore.Models.DataModels
{
    public class Page
    {
        public int PageId { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        [MinLength(3)]
        public string Body { get; set; }
        public int Sorting { get; set; }
        public bool HasSlidebar { get; set; }
    }
}