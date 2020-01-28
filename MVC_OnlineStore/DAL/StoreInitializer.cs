using System.Collections.Generic;
using System.Data.Entity;
using MVC_OnlineStore.Models.DataModels;

namespace MVC_OnlineStore.DAL
{
    public class StoreInitializer : DropCreateDatabaseIfModelChanges<StoreContext>
    {
        protected override void Seed(StoreContext context)
        {
            #region Pages ititializing
            List<Page> pages = new List<Page>
            {
                new Page
                {
                    Title = "HOME",
                    Description = "home",
                    Body = "<h1>Index</h1>",
                    Sorting = 2,
                    HasSlidebar = false
                }
            };

            foreach (Page page in pages)
                context.Pages.Add(page);

            context.SaveChanges();
            #endregion

            #region Users initializing
            List<User> users = new List<User>
            {
                new User
                {
                    FirstName = "Your",
                    SecondName= "Boss",
                    EmailAdress = "tropsshop@gmail.com",
                    Username = "admin",
                    Password = "admin",
                    Theme = "dark"
                },

                new User
                {
                    FirstName = "Сергей",
                    SecondName= "Жуков",
                    EmailAdress = "zhukov@gmail.com",
                    Username = "user1",
                    Password = "user1"
                }
            };

            foreach (User user in users)
                context.Users.Add(user);

            context.SaveChanges();

            #endregion

            #region Roles initializing

            List<Role> roles = new List<Role>
            {
                new Role
                {
                   Name = "Admin"
                },
                new Role
                {
                    Name="User"
                }
            };

            foreach (Role role in roles)
                context.Roles.Add(role);

            context.SaveChanges();
            #endregion

            #region UserRoles initializing
            List<UserRole> userRoles = new List<UserRole>
            {
                new UserRole
                {
                   UserId = 1,
                   RoleId = 1
                },
                new UserRole
                {
                    UserId = 2,
                    RoleId = 2
                }
            };

            foreach (UserRole userRole in userRoles)
                context.UserRoles.Add(userRole);

            context.SaveChanges();
            #endregion

            #region Categories initializing
            List<Category> categories = new List<Category>
            {
                new Category
                {
                   Name = "Мячи",
                   Description = "мячи",
                   Sorting = 1000
                },
                new Category
                {
                   Name = "Футболки",
                   Description = "футболки",
                   Sorting = 1000
                },
                new Category
                {
                   Name = "Кроссовки",
                   Description = "кроссовки",
                   Sorting = 1000
                }
            };

            foreach (var category in categories)
                context.Categories.Add(category);

            context.SaveChanges();
            #endregion

            #region Sidebar initializing
            List<SideBar> sideBars = new List<SideBar>
            {
                new SideBar
                {
                     Body = "<h1>qwerty</h1>"               
                }
            };

            foreach (var sideBar in sideBars)
                context.SideBars.Add(sideBar);

            context.SaveChanges();
            #endregion

            base.Seed(context);
        }
    }
}