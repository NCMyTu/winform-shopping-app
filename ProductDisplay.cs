using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using My_Shopping.Classes;

namespace My_Shopping.User_Controls
{
    public partial class ProductDisplay : UserControl
    {
        public Product? Product;

        public ProductDisplay()
        {
            InitializeComponent();
        }

        private void ProductDisplay_Load(object sender, EventArgs e)
        {
            AttachClickEvent(this);
            this.Size = new Size(290, 350);
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

        public void Set()
        {
            if (Product != null)
            {
                picProductImage.Image = this.Product.Image;
                lblProductName.Text = this.Product.Name;
                lblPrice.Text = "đ " + this.Product.FormatPrice();

                float rating = ComputeRating(LoadComment(this.Product.ID));
                if (float.IsNaN(rating))
                    lblRating.Text = "Chưa có đánh giá";
                else
                    lblRating.Text = rating.ToString("F1");
            }
        }

        public void SetProduct(Product product)
        {
            this.Product = product;
            this.Tag = product;
        }

        private List<Comment> LoadComment(string productID)
        {
            List<Comment> comments = new List<Comment>();

            string path = Path.Combine(@"C:\Users\PC MY TU\Downloads\My Shopping\data\product_comments", this.Product.ID + ".txt");
            string[] lines = File.ReadAllLines(path);

            foreach (string line in lines)
            {
                string[] values = line.Split("***");
                string username = values[0];
                string content = values[1];
                int rating = int.Parse(values[2]);

                Comment comment = new Comment(username, content, rating);
                comments.Add(comment);
            }

            return comments;
        }

        private float ComputeRating(List<Comment> comments)
        {
            if (comments.Count == 0)
                return float.NaN;

            List<int> ratings = new List<int>();

            foreach (Comment comment in comments)
                ratings.Add(comment.Rating);

            return (float)ratings.Sum() / ratings.Count;
        }
    }
}
