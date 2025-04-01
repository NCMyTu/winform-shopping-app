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
    public partial class CommentControl : UserControl
    {
        public CommentControl()
        {
            InitializeComponent();
        }

        private void CommentControl_Load(object sender, EventArgs e)
        {

        }

        public void Set(string username, string comment, int rating)
        {
            lblUsername.Text = File.ReadAllText(Path.Combine(@"C:\Users\PC MY TU\Downloads\My Shopping\data\user_info", username + ".txt")).Split("***")[2];
            picAvatar.ImageLocation = Path.Combine(@"C:\Users\PC MY TU\Downloads\My Shopping\data\user_avatars", username + ".jpg");
            SetStars(rating);
            lblComment.Text = comment;
            lblComment.Refresh();
            this.Size = new Size(this.Size.Width, lblComment.Bottom + 15);
        }

        public void SetStars(int rating)
        {
            List<PictureBox> stars = new List<PictureBox> { picStar1, picStar2, picStar3, picStar4, picStar5 };

            for (int i = 0; i < rating; i++)
                stars[i].ImageLocation = @"C:\Users\PC MY TU\Downloads\My Shopping\assets\icons\yellow_star.png";
        }
    }
}
