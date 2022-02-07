using Aspose.Cells;
using Newtonsoft.Json;
using Riots.Standard.Crypto;
using Riots.Standard.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Riots.Standard
{
    /// <summary>
    ///  统一操作入口
    /// </summary>
    public static class Base
    {
        //public static string SetConnectString(string connection)
        //{

        //}

        //数据库操作

        public static DataTable ToQueryDataTable(this string sql)
        {
            if (String.IsNullOrEmpty(sql)) return null;
            iDataMethod method = new SqlDbs();
            return method.ToQueryDataTable(sql);
        }

        public static Model.StandardJsonList<T> ToQueryDataTable<T>(this string sql)
        {
            if (String.IsNullOrEmpty(sql)) return null;
            Model.StandardJsonList<T> list = new Model.StandardJsonList<T>();

            return list;
        }

        public static string ToQueryString(this string sql, string colName, string splitChar)
        {
            if (String.IsNullOrEmpty(sql)) return "";
            string result = "";
            return result;
        }

        public static string ToExecute(this string sql)
        {
            if (String.IsNullOrEmpty(sql)) return "";
            string ret = "";
            return ret;
        }

        //字符验证、比较

        /// <summary>
        ///  验证是否为数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        /// <summary>
        ///  验证是否为日期 yyyy-MM-dd
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDate(this string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        /// <summary>
        ///  比较两个日期（年月）
        /// </summary>
        /// <param name="datetime1"></param>
        /// <param name="datetime2"></param>
        /// <returns></returns>
        public static int CompareDateTimeOfMonth(string datetime1, string datetime2)
        {
            try
            {
                DateTime dt1 = DateTime.Parse(datetime1);
                DateTime dt2 = DateTime.Parse(datetime2);
                DateTime compare1 = new DateTime(dt1.Year, dt1.Month, 1);
                DateTime compare2 = new DateTime(dt2.Year, dt2.Month, 1);
                return compare1 == compare2 ? 1 : 0;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        //内存数据操作

        /// <summary>
        ///  对DataTable 按照指定数组进行排序
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnNames"></param>
        public static void SetColumnsOrder(this DataTable table, params String[] columnNames)
        {
            int columnIndex = 0;
            foreach (var columnName in columnNames)
            {
                table.Columns[columnName].SetOrdinal(columnIndex);
                columnIndex++;
            }
        }

        /// <summary>
        ///  删除DataTable 指定列的重复行
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="field"></param>
        public static void DeleteSameRow(this DataTable dt, string field)
        {
            ArrayList indexList = new ArrayList();
            // 找出待删除的行索引   
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                if (IsContain(indexList, i)) continue;
                for (int j = i + 1; j < dt.Rows.Count; j++)
                    if (dt.Rows[i][field].ToString() == dt.Rows[j][field].ToString())
                        indexList.Add(j);
            }
            indexList.Sort();
            // 排序
            for (int i = indexList.Count - 1; i >= 0; i--) // 根据待删除索引列表删除行  
            {
                int index = Convert.ToInt32(indexList[i]);
                dt.Rows.RemoveAt(index);
            }
        }

        /// <summary>
        ///  判断数组中是否存在
        /// </summary>
        /// <param name="indexList"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static bool IsContain(ArrayList indexList, int index)
        {
            return (from object t in indexList select Convert.ToInt32(t)).Any(tempIndex => tempIndex == index);
        }

        //数据格式处理

        /// <summary>
        ///  替换 SQL语句 危险字符
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Replace(string source)
        {
            //单引号替换成两个单引号
            source = source.Replace("'", "''");

            //半角封号替换为全角封号，防止多语句执行
            source = source.Replace(";", "；");

            //半角括号替换为全角括号
            source = source.Replace("(", "（");
            source = source.Replace(")", "）");

            ///////////////要用正则表达式替换，防止字母大小写得情况////////////////////

            //去除执行存储过程的命令关键字
            source = source.Replace("Exec", "");
            source = source.Replace("Execute", "");

            //去除系统存储过程或扩展存储过程关键字
            source = source.Replace("xp_", "x p_");
            source = source.Replace("sp_", "s p_");

            //防止16进制注入
            source = source.Replace("0x", "0 x");

            return source;
        }


        //数据编码转换
        /// <summary>
        ///  Base64 解码成 字符串
        /// </summary>
        /// <param name="base64value"></param>
        /// <returns></returns>
        public static string Base64ToString(this string base64Value)
        {
            if (String.IsNullOrEmpty(base64Value)) return base64Value;
            return HttpUtility.UrlDecode(Encoding.Default.GetString(Convert.FromBase64String(base64Value)));
        }

        /// <summary>
        ///  字符串 编码成 Base64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToBase64(this string value)
        {
            return Convert.ToBase64String(Encoding.Default.GetBytes(HttpUtility.UrlEncode(value)));
        }

        /// <summary>
        ///  Json字符串 转换成 字典
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(this string json)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }

        /// <summary>
        ///  实体对象 转换成 Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonString(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }


        //实体类转换
        /// <summary>
        ///  DataRow 转换成 实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public static T ToObject<T>(this DataRow dataRow) where T : new()
        {
            T item = new T();
            foreach (DataColumn column in dataRow.Table.Columns)
            {
                if (dataRow[column] != DBNull.Value)
                {
                    PropertyInfo prop = item.GetType().GetProperty(column.ColumnName);
                    if (prop != null)
                    {
                        object result = Convert.ChangeType(dataRow[column], prop.PropertyType);
                        prop.SetValue(item, result, null);
                        continue;
                    }
                    else
                    {
                        FieldInfo fld = item.GetType().GetField(column.ColumnName);
                        if (fld != null)
                        {
                            object result = Convert.ChangeType(dataRow[column], fld.FieldType);
                            fld.SetValue(item, result);
                        }
                    }
                }
            }
            return item;
        }

        /// <summary>
        ///  DataRow 追加值到现有实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataRow"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T ToObject<T>(this DataRow dataRow, T t)
        {
            foreach (DataColumn column in dataRow.Table.Columns)
            {
                if (dataRow[column] != DBNull.Value)
                {
                    PropertyInfo prop = t.GetType().GetProperty(column.ColumnName);
                    if (prop != null)
                    {
                        object result = Convert.ChangeType(dataRow[column], prop.PropertyType);
                        prop.SetValue(t, result, null);
                        continue;
                    }
                    else
                    {
                        FieldInfo fld = t.GetType().GetField(column.ColumnName);
                        if (fld != null)
                        {
                            object result = Convert.ChangeType(dataRow[column], fld.FieldType);
                            fld.SetValue(t, result);
                        }
                    }
                }
            }
            return t;
        }

        /// <summary>
        ///  IList 实体类 转 DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> list)
        {
            if (list == null || list.Count <= 0)
                return null;
            DataTable dt = new DataTable(typeof(T).Name);
            FieldInfo[] fieldInfos = typeof(T).GetFields();
            foreach (T t in list)
            {
                if (t == null)
                    continue;
                DataRow row = dt.NewRow();
                foreach (FieldInfo fi in fieldInfos)
                {
                    string name = fi.Name;
                    if (dt.Columns[name] == null)
                    {
                        DataColumn column = new DataColumn(name, fi.FieldType);
                        dt.Columns.Add(column);
                    }
                    row[name] = fi.GetValue(t);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }


        //SQL语句生成

        /// <summary>
        ///     设置查询字符串
        /// </summary>
        /// <param name="text"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string SetWhereQuote(string text, string col)
        {
            string[] sp = text.Split(',');
            string where = "and (" + col + " in(";
            if (sp.Length > 1)
            {
                where = sp.Aggregate(where, (current, t) => current + "'" + t + "',");
                where = where.Remove(where.Length - 1);
                where += ") or " + col + " is null) ";
            }
            else
            {
                where = "and (" + col + "='" + text + "' or " + col + " is null) ";
            }

            return where;
        }

        /// <summary>
        ///     设置查询字符串
        /// </summary>
        /// <param name="text"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string SetNotNullWhereQuote(string text, string col)
        {
            string[] sp = text.Split(',');
            string where = "and (" + col + " in(";
            if (sp.Length > 1)
            {
                where = sp.Aggregate(where, (current, t) => current + "'" + t + "',");
                where = where.Remove(where.Length - 1);
                where += ")) ";
            }
            else
            {
                where = "and (" + col + "='" + text + "') ";
            }

            return where;
        }

        /// <summary>
        ///     设置查询字符串
        /// </summary>
        /// <param name="text"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string SetLikeWhereQuote(string text, string col)
        {
            string[] sp = text.Split(',');
            string where;
            if (sp.Length > 1)
                where = "and (" + col + " like '%" + sp[0] + "%' or " + col + " is null) ";
            else
                where = "and (" + col + " like '%" + text + "%' or " + col + " is null) ";
            return where;
        }

        /// <summary>
        ///     设置查询字符串
        /// </summary>
        /// <param name="text"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string SetLikeWhereNotNullQuote(string text, string col)
        {
            string[] sp = text.Split(',');
            string where;
            if (sp.Length > 1)
                where = "and (" + col + " like '%" + sp[0] + "%') ";
            else
                where = "and (" + col + " like '%" + text + "%') ";
            return where;
        }

        /// <summary>
        ///  构造更新语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string BuildUpdateSql(string tableName, decimal id, Dictionary<string, object> items)
        {
            StringBuilder sb = new StringBuilder();
            if (items.ContainsKey("ID"))
                items.Remove("ID");
            if (items.ContainsKey("id"))
                items.Remove("id");
            sb.AppendLine("update " + tableName + "  set ");
            int index = 0;
            List<string> deletedKeyList = new List<string>();
            foreach (KeyValuePair<string, object> kv in items)
            {
                if (kv.Value == null)
                    deletedKeyList.Add(kv.Key);
                else if (kv.Key.ToLower().EndsWith("date") && (kv.Value.ToString() == "" || kv.Value.ToString() == DateTime.MinValue.ToString()))
                    deletedKeyList.Add(kv.Key);
                else if (kv.Key.ToLower().EndsWith("time") && (kv.Value.ToString() == "" || kv.Value.ToString() == DateTime.MinValue.ToString()))
                    deletedKeyList.Add(kv.Key);
            }
            foreach (string k in deletedKeyList) items.Remove(k);
            foreach (KeyValuePair<string, object> kv in items)
            {
                sb.Append(kv.Key + " = ");
                if (kv.Key.ToLower() == "image")
                {
                    sb.Append("'");
                    sb.Append(kv.Value.ToString().Replace('|', '+'));
                    sb.Append("'");
                }
                else if (kv.Value.GetType().Equals(typeof(string)) || kv.Value.GetType().Equals(typeof(DateTime)))
                {
                    sb.Append("'");
                    sb.Append(Replace(kv.Value.ToString()));
                    sb.Append("'");
                }
                else
                {
                    sb.Append(kv.Value);
                }
                if (index != items.Count - 1)
                    sb.Append(",");
                index += 1;
            }
            sb.Append(" where ID = " + id);
            return sb.ToString().Replace("\r\n", ""); ;
        }

        /// <summary>
        ///  构造插入语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string BuildInsertSql(string tableName, decimal id, Dictionary<string, object> items)
        {
            StringBuilder sb = new StringBuilder();
            if (items.ContainsKey("ID"))
                items.Remove("ID");
            if (items.ContainsKey("id"))
                items.Remove("id");
            sb.AppendLine("insert into " + tableName + "(");
            int index = 0;
            List<string> deletedKeyList = new List<string>();
            foreach (KeyValuePair<string, object> kv in items)
            {
                if (kv.Value == null)
                    deletedKeyList.Add(kv.Key);
                else if (string.IsNullOrEmpty(kv.Value.ToString()))
                    deletedKeyList.Add(kv.Key);
                else if (kv.Key.ToLower().EndsWith("date") && (kv.Value.ToString() == "" || kv.Value.ToString() == DateTime.MinValue.ToString()))
                    deletedKeyList.Add(kv.Key);
                else if (kv.Key.ToLower().EndsWith("time") && (kv.Value.ToString() == "" || kv.Value.ToString() == DateTime.MinValue.ToString()))
                    deletedKeyList.Add(kv.Key);
            }
            foreach (string k in deletedKeyList) items.Remove(k);
            sb.Append("ID,");
            foreach (KeyValuePair<string, object> kv in items)
            {
                sb.Append(kv.Key);
                if (index != items.Count - 1)
                    sb.Append(",");
                index += 1;
            }
            sb.Append(") values(");
            index = 0;
            sb.Append(id);
            sb.Append(",");
            foreach (KeyValuePair<string, object> kv in items)
            {
                if (kv.Key.ToLower() == "image")
                {
                    sb.Append("'");
                    sb.Append(kv.Value.ToString());
                    sb.Append("'");
                }
                else if (kv.Value.GetType().Equals(typeof(string)) || kv.Value.GetType().Equals(typeof(DateTime)))
                {
                    sb.Append("'");
                    sb.Append(Replace(kv.Value.ToString()));
                    sb.Append("'");
                }
                else
                {
                    sb.Append(kv.Value);
                }
                if (index != items.Count - 1)
                    sb.Append(",");
                index += 1;
            }
            sb.Append(")");
            return sb.ToString().Replace("\r\n", ""); ;
        }


        //日期操作


        //Excel操作
        /// <summary>
        ///  将 DataTable 按照给定的列 导出Excel文件流
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static MemoryStream ToExportExcel(this DataTable dt, Dictionary<string, string> columns)
        {
            dt.PrimaryKey = null;
            dt.Columns.Add(new DataColumn { ColumnName = "AUTOID", DataType = typeof(int) });
            if (dt.Columns.Contains("ID"))
                dt.Columns.Remove("ID");
            columns.Add("AUTOID", "序号");
            int n = 1;
            foreach (DataRow dr in dt.Rows)
            {
                dr["AUTOID"] = n;
                n++;
            }
            List<string> delNames = new List<string>();
            foreach (DataColumn dc in dt.Columns)
            {
                if (!columns.ContainsKey(dc.ColumnName))
                {
                    delNames.Add(dc.ColumnName);
                }
            }
            foreach (string name in delNames)
            {
                dt.Columns.Remove(name);
            }
            dt.SetColumnsOrder(columns.Keys.ToArray());
            dt.Columns["AUTOID"].SetOrdinal(0);
            // 创建工作簿
            Workbook book = new Workbook();
            // 创建工作表
            Worksheet sheet = book.Worksheets[0];
            // 单元格
            Cells cells = sheet.Cells;
            // 生成行 列名行 
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                cells[0, i].PutValue(columns[dt.Columns[i].ColumnName]);
            }
            // 生成数据行 
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    cells[1 + i, k].PutValue(dt.Rows[i][k].ToString()); //添加数据
                    if (dt.Columns[k].DataType == typeof(DateTime))
                    {
                        DateTime d;
                        if (DateTime.TryParse(dt.Rows[i][k].ToString(), out d))
                            cells[1 + i, k].PutValue(d.ToString("yyyy-MM-dd"));
                        if (d == DateTime.MinValue)
                            cells[1 + i, k].PutValue("");
                    }
                    if (dt.Columns[k].DataType == typeof(Decimal))
                    {
                        cells[1 + i, k].PutValue(double.Parse(dt.Rows[i][k].ToString()));
                    }
                }
            }
            // 设置样式
            int columnCount = cells.MaxColumn + 1;
            int rowCount = cells.MaxRow + 1;
            for (int i = 0; i < rowCount; i++)
            {
                cells.SetRowHeightPixel(i, 30);
                for (int k = 0; k < columnCount; k++)
                {
                    Style style = cells[i, k].GetStyle();
                    style.HorizontalAlignment = TextAlignmentType.Center;//字体居中
                    style.VerticalAlignment = TextAlignmentType.Center;
                    style.Font.Name = "黑体";//文字字体
                    style.Font.Size = 12;//文字大小
                    cells[i, k].SetStyle(style);
                    if (cells[0, k].StringValue.EndsWith("金额"))
                    {
                        Style sCurrency = cells[i, k].GetStyle();
                        sCurrency.Number = 8;
                        cells[i, k].SetStyle(sCurrency);
                    }
                    if (cells[0, k].StringValue.EndsWith("资金计划"))
                    {
                        Style sCurrency = cells[i, k].GetStyle();
                        sCurrency.Number = 8;
                        cells[i, k].SetStyle(sCurrency);
                    }
                }
            }
            for (int k = 0; k < columnCount; k++)
            {
                Style style = cells[0, k].GetStyle();
                style.Font.IsBold = true;
                cells[0, k].SetStyle(style);
                cells.SetColumnWidthPixel(k, 140);
            }
            cells.SetColumnWidthPixel(0, 60);
            MemoryStream ms = new MemoryStream();
            book.Save(ms, SaveFormat.Xlsx);
            return ms;
        }

        //Word操作


        //加密解密

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToAESEncrypt(this string text)
        {
            string password = "aselrias38490a32";
            return AES.ByteArrayToHex(AES.AES_Encrypt(Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.Default.GetBytes(HttpUtility.UrlEncode(text)))), SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password))));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToAESDecrypt(this string text)
        {
            string password = "aselrias38490a32";
            return Encoding.UTF8.GetString(AES.AES_Decrypt(AES.HexToByteArray(text), SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password))));
        }


        //日志操作


        //短信服务

    }
}
