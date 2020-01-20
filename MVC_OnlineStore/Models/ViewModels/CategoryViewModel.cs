using MVC_OnlineStore.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_OnlineStore.Models.ViewModels
{
    public class CategoryViewModel
    {
        public CategoryViewModel()
        {
        }
        public CategoryViewModel(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            Description = category.Description;
            Sorting = category.Sorting;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Sorting { get; set; }
    }
}