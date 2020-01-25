using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace MVC_OnlineStore.Areas.Admin.Infrastructure
{
    public class ImageHelper
    {
        private DirectoryInfo originalDirectory;
        private string pathString1;
        private string pathString2;
        private string pathString3;
        private string pathString4;
        private string pathString5;

        public ImageHelper(DirectoryInfo originalDirectory, int id)
        {
            this.originalDirectory = originalDirectory;
            pathString1 = Path.Combine(originalDirectory.ToString(), "Products");
            pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
            pathString3 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");
            pathString4 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
            pathString5 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");
        }

        public static bool IsImage(string type)
        {
            if (type != "image/jpg"
                && type != "image/jpg"
                && type != "image/jpeg"
                && type != "image/png"
                && type != "image/svg"
                && type != "image/gif")
            {
                return false;
            }

            return true;
        }

        public void CreateDirectoriesIfNotExist(DirectoryInfo originalDirectory, int id)
        {
            if (!Directory.Exists(pathString1)) Directory.CreateDirectory(pathString1);
            if (!Directory.Exists(pathString2)) Directory.CreateDirectory(pathString2);
            if (!Directory.Exists(pathString3)) Directory.CreateDirectory(pathString3);
            if (!Directory.Exists(pathString4)) Directory.CreateDirectory(pathString4);
            if (!Directory.Exists(pathString5)) Directory.CreateDirectory(pathString5);
        }

        public void SaveFiles(string imageName, HttpPostedFileBase file)
        {
            var path = string.Format($"{pathString2}\\{imageName}");
            var path2 = string.Format($"{pathString3}\\{imageName}");

            file.SaveAs(path);

            WebImage img = new WebImage(file.InputStream);
            img.Resize(200, 200).Crop(1,1);
            img.Save(path2);
        }

        public void ChangeImages()
        {
            DirectoryInfo di1 = new DirectoryInfo(pathString2);
            DirectoryInfo di2 = new DirectoryInfo(pathString3);

            foreach (var item in di2.GetFiles())
            {
                item.Delete();
            }

            foreach (var item in di1.GetFiles())
            {
                item.Delete();
            }
        }
    }
}