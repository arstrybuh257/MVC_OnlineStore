using MVC_OnlineStore.Models.DataModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVC_OnlineStore.Models.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel() { }
        public ProductViewModel(Product product)
        {
            ProductId = product.ProductId;
            Name = product.Name;
            ShortInfo = product.ShortInfo;
            if (product.Category != null)
                CategoryId = product.Category.Id;
            else
                CategoryId = 0;
            Description = product.Description;
            Price = product.Price;
            ImageName = product.ImageName;
        }
        public int ProductId { get; set; }
        [MinLength(3)]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        public string ShortInfo { get; set; }
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [Required]
        public string Description { get; set; }
        public double Price { get; set; }
        [DisplayName("Image")]
        public string ImageName { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<string> Images { get; set; }
    }
}