using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Riots.Standard.Data
{
    public class SqlDbs : iDataMethod
    {
        public DataTable ToQueryDataTable(string sql)
        {
            //得到表名
            string tablename = GetTableNameFromSql(sql);
            DataSet rst = new DataSet();
            //创建数据库命令
            SqlConnection myConnection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
            try
            {
                //打开数据库
                if (myConnection.State == ConnectionState.Closed) myConnection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sql, myConnection);
                adapter.Fill(rst, tablename);
            }
            catch (Exception Ex) { Console.WriteLine(Ex.Message); }
            //关闭数据库
            if (myConnection.State == ConnectionState.Open) myConnection.Close();
            return rst.Tables[0];
        }

        public string SetConnectionString(string text)
        {
            ConfigurationManager.AppSettings["ConnectionString"] = text;
            return new Model.MessageResultJson("设置成功").ToJsonString();
        }

        public string SetConnectionType(string text)
        {
            ConfigurationManager.AppSettings["ConnectionType"] = "sql";
            return new Model.MessageResultJson("设置成功").ToJsonString();
        }

        private string GetTableNameFromSql(object qselect)
        {
            throw new NotImplementedException();
        }
    }
}
