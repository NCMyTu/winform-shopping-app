using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace My_Shopping.Classes
{
    public class Product
    {
        public string ID {  get; set; }
        public Image Image {  get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Seller {  get; set; }
        public string Category { get; set; }

        public Product(string id, Image image, string name, int price, string description, int quantity, string category, string seller)
        {
            this.ID = id;
            this.Image = image;
            this.Name = name;
            this.Price = price;
            this.Description = description;
            this.Quantity = quantity;
            this.Seller = seller;
            this.Category = category;
        }

        public string FormatPrice()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            return this.Price.ToString("N0", culture);
        }
    }
}
