using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace gLog
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            const string dbName = @"C:\Data\AppData\plog.db";
            if (System.IO.File.Exists(dbName) == false)
            {
                string connectionStr = String.Format(@"Data Source={0};Version=3;", dbName);
                SQLiteConnection.CreateFile(dbName);

                SQLiteConnection connection = new SQLiteConnection(connectionStr);
                connection.Open();

                // CURRENT_TIMESTAMP doesn't store milleseconds, therefore it has been changes to the 'strftime' function.

                string tableStr = "create table LogDetail (logdetail_pk integer primary key autoincrement, logdetail_macaddress text, logdetail_latitude real, " +
                        " logdetail_longtitude real, logdetail_batterylevel integer, when_created datetime default (strftime('%Y-%m-%d %H:%M:%f', 'now')), " +
                        " is_deleted bool default (0), user_modified int default (0));";

                SQLiteCommand cmd = connection.CreateCommand();
                cmd.CommandText = tableStr;

                cmd.ExecuteNonQuery();

                connection.Close();
            }

        }
    }
}
