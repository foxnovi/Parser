﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MvideoParse
{
    public static class DataModel
    {
        private static DataAdapter dataAdapter = new DataAdapter();
        public static List<Category> Categories { get; set; }
        static DataModel()
        {
            Categories = new List<Category>();
            dataAdapter.Connect(new MySqlConnectionStringBuilder()
            {
                Server = "http://s2.kts.tu-bryansk.ru/",
                Port = 3306,
                Database = "17IAS-AMISI_ZagoriychukKU",
                UserID = "17IAS-AMISI.ZagoriychukKU",
                Password = "@*oQW^p5r[01PSL!",
                CharacterSet = "utf8"
            });
        }

        static public void AddCategory(string title, string url, long? idParent)
        {
            long? id = dataAdapter.InsertRow($"insert into `Category` (`Title`, `URL`" +
                idParent != null ? "(, `ID_Parent`)" : "``" +
                $") values ({title}, {url}" +
                idParent != null ? $"(, {idParent})" : "``" +
                $");");
            if (id != null) Categories.Add(new Category(dataAdapter, id, title, url, idParent));
        }
    }
}
