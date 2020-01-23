using System;
using System.Collections.Generic;
using System.Data.Entity;
using MVC_OnlineStore.Models.DataModels;

namespace MVC_OnlineStore.DAL
{
    public class StoreInitializer : DropCreateDatabaseIfModelChanges<StoreContext>
    {
        protected override void Seed(StoreContext context)
        {
            List<Page> pages = new List<Page>
            {
                new Page { Title = "HOME", Description = "home",
                    Body = "<h1>Index</h1>", Sorting = 2, HasSlidebar = false}
            };

            foreach (Page page in pages)
                context.Pages.Add(page);

            context.SaveChanges();
            base.Seed(context);

        }
    }
}