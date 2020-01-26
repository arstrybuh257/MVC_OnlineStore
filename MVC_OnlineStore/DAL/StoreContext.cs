using MVC_OnlineStore.Models.DataModels;
using System.Data.Entity;

namespace MVC_OnlineStore.DAL
{
    public class StoreContext : DbContext
    {
        public StoreContext() : base("StoreDb") { }
        public DbSet<Page> Pages { get; set; }
        public DbSet<SideBar> SideBars { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
    }
}