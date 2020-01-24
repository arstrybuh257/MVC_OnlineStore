using System.ComponentModel.DataAnnotations;

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
        public double Price { get; set; }
        public string ImageName { get; set; }

        public virtual Category Category { get; set; }      
    }
}