using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MVC_OnlineStore.DAL
{
    public class StoreInitializer : DropCreateDatabaseIfModelChanges<StoreContext>
    {
    }
}