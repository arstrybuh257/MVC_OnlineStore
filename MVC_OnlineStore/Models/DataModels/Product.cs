using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_OnlineStore.Models.DataModels
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [MinLength(3)]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        public string ShortInfo { get; set; }
        [Required]
        public string Description { get; set; }
        public int Price { get; set; }
        public string ImageName { get; set; }

        public virtual Category Category { get; set; }      
    }
}