using MVC_OnlineStore.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVC_OnlineStore.DAL
{
    public class StoreContext : DbContext
    {
        public StoreContext() : base("StoreDb") { }
        public DbSet<Page> Pages { get; set; }

    }
}