using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    internal class Menu
    {
        public Menu(string name, int count, float price, float totalPrice = 0)
        {
            this.Name = name;
            this.Count = count;
            this.Price = price;
            this.TotalPrice = totalPrice;
        }
        public Menu(DataRow row)
        {
            this.Name = row["name"].ToString();
            this.Count = (int)row["count"];
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
            this.TotalPrice = (float)Convert.ToDouble(row["totalPrice"].ToString());
        }
        private float totalPrice;

        private float price;

        private int count;

        private string name;

        public int Count { get { return count; } set { count = value; } }
        public float Price { get { return price; } set { price = value; } }
        public float TotalPrice { get { return totalPrice; } set { totalPrice = value; } }
        public string Name { get { return name; } set { name = value; } }
    }
}
