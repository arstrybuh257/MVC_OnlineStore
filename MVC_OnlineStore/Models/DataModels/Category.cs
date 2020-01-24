using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC_OnlineStore.Models.DataModels
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Sorting { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}