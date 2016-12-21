using gLog.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Web.Http.Cors;


namespace gLog.Controllers
{
    [EnableCors("*", "*", "GET,POST")]
    public class LogController : ApiController
    {
        // GET: api/Log
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Log/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Log
        [HttpPost]
        public IHttpActionResult Post(double latitude, double longtitude)
        {
            // $.post('api/Log?Latitude=4.99&Longtitude=-13.12')
            string connectionStr = @"Data Source=C:\Data\AppData\gLog.db;Version=3;";
            SQLiteConnection connection = new SQLiteConnection(connectionStr);
            connection.Open();

            string tableStr = String.Format("insert into LogDetail (logdetail_latitude, logdetail_longtitude) values ({0}, {1})", latitude, longtitude);

            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = tableStr;

            cmd.ExecuteNonQuery();

            connection.Close();
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult Post(LocationModel location)
        {
            // $.post('api/Log?Latitude=4.99&Longtitude=-13.12')
            string connectionStr = @"Data Source=C:\Data\AppData\plog.db;Version=3;";
            SQLiteConnection connection = new SQLiteConnection(connectionStr);
            connection.Open();

            string tableStr = String.Format("insert into LogDetail (logdetail_latitude, logdetail_longtitude) values ({0}, {1})", location.Latitude, location.Longitude);

            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = tableStr;

            cmd.ExecuteNonQuery();

            connection.Close();
            return Ok();
        }

        [HttpPost()]
        [Route("api/plog/{macaddress}/{latitude}/{longtitude}/{battery}")]
        public IHttpActionResult Post(string macAddress, string latitude, string longtitude, string battery)
        {
            // $.post('api/Log?Latitude=4.99&Longtitude=-13.12')
            string connectionStr = @"Data Source=C:\Data\AppData\plog.db;Version=3;";
            SQLiteConnection connection = new SQLiteConnection(connectionStr);
            connection.Open();

            string tableStr = String.Format("insert into LogDetail (logdetail_macaddress, logdetail_latitude, logdetail_longtitude, logdetail_batterylevel) " + 
                    "values ('{0}', {1}, {2}, {3})", macAddress, latitude, longtitude, battery);

            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = tableStr;

            cmd.ExecuteNonQuery();

            connection.Close();
            return Ok();
        }

        [HttpGet()]
        [Route("api/plog/list")]
        public IList<LogDetail> List()
        {
            // Todo: Put this in a mapper
            

            string connectionStr = @"Data Source=C:\Data\AppData\plog.db;Version=3;";
            SQLiteConnection connection = new SQLiteConnection(connectionStr);
            connection.Open();

            string selectStr = "select logdetail_pk, datetime(when_created, 'localtime') as when_created, logdetail_macaddress, logdetail_latitude, " +
                "logdetail_longtitude, logdetail_batterylevel from LogDetail order by logdetail_pk desc;";

            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandType =  System.Data.CommandType.Text;
            cmd.CommandText = selectStr;


            SQLiteDataReader dr = cmd.ExecuteReader();
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Load(dr);

            IList<LogDetail> logDetailList = new List<LogDetail>();
            foreach (System.Data.DataRow drow in dt.Rows)
            {
                Console.WriteLine(drow[0].ToString());
                LogDetail logDetail = new LogDetail();
                logDetail.MACAddress = drow["logdetail_macaddress"].ToString();
                logDetail.Latitude = Double.Parse(drow["logdetail_latitude"].ToString());
                logDetail.Longitude = Double.Parse(drow["logdetail_longtitude"].ToString());
                logDetail.BatteryLevel = Int32.Parse(drow["logdetail_batterylevel"].ToString());

                logDetailList.Add(logDetail);
            }
            connection.Close();

            return logDetailList;
    
        }

        [HttpGet()]
        [Route("api/plog/list/{toprecords}")]
        public HttpResponseMessage GetTopRecords(int topRecords)
        {
            string connectionStr = @"Data Source=C:\Data\AppData\plog.db;Version=3;";
            SQLiteConnection connection = new SQLiteConnection(connectionStr);
            connection.Open();

            string selectStr = String.Format("select logdetail_pk, datetime(when_created, 'localtime') as when_created, logdetail_macaddress, logdetail_latitude, " +
                "logdetail_longtitude, logdetail_batterylevel from LogDetail order by logdetail_pk desc limit {0};", topRecords);

            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = selectStr;


            SQLiteDataReader dr = cmd.ExecuteReader();
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Load(dr);

            IList<LogDetail> logDetailList = new List<LogDetail>();
            foreach (System.Data.DataRow drow in dt.Rows)
            {
                Console.WriteLine(drow[0].ToString());
                LogDetail logDetail = new LogDetail();
                logDetail.ID = Int32.Parse(drow["logdetail_pk"].ToString());
                logDetail.WhenCreated = DateTime.Parse(drow["when_created"].ToString());
                logDetail.MACAddress = drow["logdetail_macaddress"].ToString();
                logDetail.Latitude = Double.Parse(drow["logdetail_latitude"].ToString());
                logDetail.Longitude = Double.Parse(drow["logdetail_longtitude"].ToString());
                logDetail.BatteryLevel = Int32.Parse(drow["logdetail_batterylevel"].ToString());

                logDetailList.Add(logDetail);
            }
            connection.Close();

            return new HttpResponseMessage()
            {
                Content = new StringContent(Newtonsoft.Json.Linq.JArray.FromObject(logDetailList).ToString(), System.Text.Encoding.UTF8, "application/json")
            };
        }
        // PUT: api/Log/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Log/5
        public void Delete(int id)
        {
        }
    }
}
