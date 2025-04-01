using My_Shopping.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace My_Shopping
{
    public partial class ProductDisplayWarehouse : UserControl
    {
        public Product? Product;

        public ProductDisplayWarehouse()
        {
            InitializeComponent();
        }

        private void ProductDisplayWarehouse_Load(object sender, EventArgs e)
        {
            AttachClickEvent(this);
            this.Size = new Size(1280, 120);
        }

        private void AttachClickEvent(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                control.Click += (s, e) => OnClick(e);
                if (control.HasChildren)
                    AttachClickEvent(control);
            }
        }

        public void SetProduct(Product product)
        {
            this.Product = product;
            picImage.Image = product.Image;
            lblName.Text = product.Name;
            lblPrice.Text = "đ " + product.FormatPrice();
            lblQuantity.Text = product.Quantity.ToString();
            DynamicSizing();
        }

        public void DynamicSizing()
        {
            int middlePointName = 435, middlePointPrice = 860, middlePointQuantity = 1157;

            lblName.Location = new Point(middlePointName - lblName.MaximumSize.Width / 2, lblName.Location.Y);
            lblPrice.Location = new Point(middlePointPrice - lblPrice.Size.Width / 2, lblPrice.Location.Y);
            lblQuantity.Location = new Point(middlePointQuantity - lblQuantity.Size.Width / 2, lblQuantity.Location.Y);
        }
    }
}
