using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EastRiverCommune.Controllers
{
	public class MainController : BasicController
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AppCache()
        {
            UpgradeAppCache();
            return this.File(this.Request.ApplicationPath.TrimEnd('/') + AppCacheFileName, "text/cache-manifest");
        }

        public const string AppCacheFileName = @"/Manifest/Main.appcache";

        public static void UpgradeAppCache()
        {
            var http_context = System.Web.HttpContext.Current;
            string application_path = http_context.Request.ApplicationPath.TrimEnd('/');
            string physical_path = http_context.Request.MapPath(http_context.Request.ApplicationPath);
            string app_cache_file_name = '\\' + AppCacheFileName.TrimStart('/');
            if (System.IO.File.Exists(physical_path + app_cache_file_name))
                System.IO.File.Delete(physical_path + app_cache_file_name);
            string[] folders = { @"\Fonts", @"\Styles", @"\Scripts" };
            var fs = System.IO.File.Open(physical_path + app_cache_file_name, System.IO.FileMode.OpenOrCreate);
            var writer = new System.IO.StreamWriter(fs);
            writer.WriteLine("CACHE MANIFEST");
            foreach (var folder in folders)
            {
                foreach (var path in System.IO.Directory.GetFiles(physical_path + folder))
                {
                    string p = path.Substring(physical_path.Length, path.Length - physical_path.Length).Replace('\\', '/');
                    writer.WriteLine(application_path + p);
                }
                writer.Flush();
            }
            writer.WriteLine("NETWORK:");
            writer.WriteLine("*");
            writer.Close();
        }

    }
}
