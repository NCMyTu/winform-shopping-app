using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Shopping.Classes
{
    public class Comment
    {
        public string Username { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }

        public Comment(string username, string content, int rating)
        {
            this.Username = username;
            this.Content = content;
            this.Rating = rating;
        }

    }
}
