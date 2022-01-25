using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MvideoParse
{
    public class DataAdapter
    {
        private MySqlConnection _connection;
        private string valueToString(object o)
        {
            if (o == null) return "NULL";
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Boolean: return ((bool)o) ? "1" : "0";
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal: return MySqlHelper.EscapeString(o.ToString());
                case TypeCode.String:
                case TypeCode.Char: return $"'{MySqlHelper.EscapeString(o.ToString())}'";
                default: throw new Exception("Value is not simple type.");
            }
        }
        public bool IsConnected { get; private set; }
        public DataAdapter()
        {
            _connection = new MySqlConnection();
        }
        public bool Connect(MySqlConnectionStringBuilder stringBuilder)
        {
            _connection.ConnectionString = "Server=" + stringBuilder.Server + ";port=" + stringBuilder.Port
                                        + ";User Id=" + stringBuilder.UserID
                                        + (!String.IsNullOrWhiteSpace(stringBuilder.Password) ? ";password=" + stringBuilder.Password : "")
                                        + ";Database=" + stringBuilder.Database
                                        + ";CharSet=" + stringBuilder.CharacterSet;
            try
            {
                _connection.Open();
                IsConnected = true;
                return true;
            }
            catch (Exception)
            {
                IsConnected = false;
                return false;
            }
        }
        public void Disconnect()
        {
            _connection.Close();
            IsConnected = false;
        }

        public void Dispose()
        {
            _connection.Close();
        }
        public long InsertRow(string query)
        {
            MySqlCommand cmd = new MySqlCommand(query, _connection);
            cmd.ExecuteNonQuery();
            return cmd.LastInsertedId;
        }
        public long InsertRow(string tableName, Dictionary<string, object> values)
        {
            StringBuilder q = new StringBuilder($"INSERT INTO `{tableName}` (");
            bool isFirst = true;
            foreach (KeyValuePair<string, object> v in values)
            {
                if (!isFirst) q.Append(" ,"); else isFirst = false;
                q.Append($"`{v.Key}`");
            }
            q.Append(") VALUES (");
            isFirst = true;
            foreach (KeyValuePair<string, object> v in values)
            {
                if (!isFirst) q.Append(" ,"); else isFirst = false;
                q.Append(valueToString(v.Value));
            }
            q.Append(");");
            return InsertRow(q.ToString());
        }
    }
}
