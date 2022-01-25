using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvideoParse
{
    public class Category
    {
        private long? id;
        private long? idParent;
        private DataAdapter dataAdapter;
        public string Title { get; set; }
        public string URL { get; set; }
        public List<ProductCard> ProductCards { get; set; }
        public Category(DataAdapter dataAdapter, long? id, string title, string url, long? idParent)
        {
            this.dataAdapter = dataAdapter;
            this.id = id;
            Title = title;
            URL = url;
            this.idParent = idParent;
            ProductCards = new List<ProductCard>();
        }
        public void AddProductCard(string title, string url)
        {
            //long? id = db.InsertRow($"insert into `Students` (`Name`) values ('{name}');");
            long? id_card = dataAdapter.InsertRow("ProductCard", new Dictionary<string, object>() { { "Title", title }, { "URL", url }, { "ID_Parent", id } });
            ProductCards.Add(new ProductCard(dataAdapter, id_card, title, url, id));

        }
    }
}
