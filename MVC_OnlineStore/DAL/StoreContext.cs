using MVC_OnlineStore.Models.DataModels;
using System.Data.Entity;

namespace MVC_OnlineStore.DAL
{
    public class StoreContext : DbContext
    {
        public StoreContext() : base("StoreDb") { }
        public DbSet<Page> Pages { get; set; }

        public DbSet<SideBar> SideBars { get; set; }
    }
}