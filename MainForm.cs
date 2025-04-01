using My_Shopping.Classes;
using My_Shopping.User_Controls;
using System.Drawing.Imaging;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.Threading;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace My_Shopping
{
    public partial class MainForm : Form
    {
        string productInfoPath = @"C:\Users\PC MY TU\Downloads\My Shopping\data\product_info";
        string productImagePath = @"C:\Users\PC MY TU\Downloads\My Shopping\data\product_images";
        string productDescriptionPath = @"C:\Users\PC MY TU\Downloads\My Shopping\data\product_descriptions";
        string productCommentPath = @"C:\Users\PC MY TU\Downloads\My Shopping\data\product_comments";
        string userInfoPath = @"C:\Users\PC MY TU\Downloads\My Shopping\data\user_info";
        string userAvatarPath = @"C:\Users\PC MY TU\Downloads\My Shopping\data\user_avatars";
        List<User> users = new List<User>();
        List<Product> products = new List<Product>();
        List<Product> productsByCategory = new List<Product>();
        Dictionary<string, int> cart = new Dictionary<string, int>();
        string currentUsername = "";
        Product currentProduct;
        int currentUserRating = 0;

        private enum SignInStatus
        {
            Success,
            Invalid,
            NotEnoughFields
        }

        private enum SignUpStatus
        {
            Success,
            UsernameExisted,
            WeakPassword,
            PasswordMismatch,
            NotEnoughFields
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cbxAddProductCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            lblCartNumItems.Text = "";
            LoadUserInfo();
            LoadProducts();
            productsByCategory = products;

            SetCategoriesEvents();

            SwitchToSignIn();

            lblForgetPassword_sign_in.Visible = false;
        }

        private void LoadUserInfo()
        {
            List<User> result = new List<User>();
            string[] fileNames = Directory.GetFiles(userInfoPath);

            foreach (string file in fileNames)
            {
                string[] values = File.ReadAllText(file).Split("***");
                string username = Path.GetFileNameWithoutExtension(file);
                string password = values[0];
                string email = values[1];

                User user = new User(username, password, email);

                result.Add(user);
            }

            users = result;
        }

        private void LoadProducts()
        {
            products = new List<Product>();
            string[] fileNames = Directory.GetFiles(productInfoPath);

            foreach (string file in fileNames)
            {
                string fileName = Path.GetFileName(file);

                string id, name, description, category, seller;
                int price, quantity;
                Image image;

                // read Product's name, price and quantity
                string[] values = File.ReadAllText(file).Split("***");
                id = Path.GetFileNameWithoutExtension(file);
                name = values[0];
                price = int.Parse(values[1]);
                quantity = int.Parse(values[2]);
                category = values[3];
                seller = values[4];

                description = File.ReadAllText(Path.Combine(productDescriptionPath, fileName));

                // Image.FromFile doesnt release memory, fucking retarded 
                using (var bmpTemp = new Bitmap(Path.Combine(productImagePath, id + ".jpg")))
                    image = new Bitmap(bmpTemp);

                Product product = new Product(id, image, name, price, description, quantity, category, seller);

                products.Add(product);
            }
        }

        private string CreateID()
        {
            return DateTime.Now.ToString("ddMMyyyyHHmmssfff");
        }

        private void WriteUserInfo(User user)
        {
            string path = Path.Combine(userInfoPath, user.Username + ".txt");

            using (StreamWriter writer = new StreamWriter(path, false))
                writer.Write(user.Password + "***" + user.Email + "***" + user.Name);

            File.Copy(@"C:\Users\PC MY TU\Downloads\My Shopping\assets\icons\default_avatar.png",
                Path.Combine(userAvatarPath, user.Username + ".jpg"));
        }

        private void AlignCenterLabel(Label label, Panel parentPanel)
        {
            int labelWidth = label.Size.Width;
            int panelWidth = parentPanel.Size.Width;

            int x = (panelWidth - labelWidth) / 2;

            label.Location = new Point(x, label.Location.Y);
        }

        private void SwitchToSignIn()
        {
            lblWarning_sign_in.Text = "";

            txtUsername_sign_in.Text = "";
            txtPassword_sign_in.Text = "";
            txtUsername_sign_in.TabIndex = 0;
            txtPassword_sign_in.TabIndex = 1;
            btnSignIn_sign_in.TabIndex = 2;

            pnlSignIn.BringToFront();
        }

        private void SwitchToSignUp()
        {
            lblWarning_sign_up.Text = "";

            txtEmail_sign_up.TabIndex = 0;
            txtUsername_sign_up.TabIndex = 1;
            txtPassword_sign_up.TabIndex = 2;
            txtPasswordAgain_sign_up.TabIndex = 3;
            btnSignUp_sign_up.TabIndex = 4;

            pnlSignUp.BringToFront();
        }

        private void SwitchToShopping()
        {
            pnlShopping.BringToFront();
            pnlShoppingUser.BringToFront();
        }

        private void SwitchToUser()
        {
            picUserAvatar.ImageLocation = Path.Combine(userAvatarPath, currentUsername + ".jpg");

            lblUserName.Text = File.ReadAllText(Path.Combine(userInfoPath, currentUsername + ".txt")).Split("***")[2];

            lblUserName.Location = new Point((pnlUser.Width - lblUserName.Size.Width) / 2, lblUserName.Location.Y);
            picUserNameEdit.Location = new Point(lblUserName.Right + 10, picUserNameEdit.Location.Y);
            txtNewUserName.Visible = false;
            txtNewUserName.SendToBack();
            txtNewUserName.Text = "";

            pnlUser.BringToFront();
        }

        private void SwitchToWarehouse(string username)
        {
            DisplayWarehouse(currentUsername);

            pnlWarehouseUser.BringToFront();
        }

        private void SwitchToCart()
        {
            pnlCart.BringToFront();
        }

        private void SwitchToAddProduct(string username)
        {
            ResetAddProductComponents();

            btnAdd.Text = "Thêm";
            lblAddProductWarning.Visible = false;
            cbxAddProductCategory.SelectedIndex = -1;

            pnlAddProduct.BringToFront();
        }

        private void SwitchToAdjustProduct(Product product)
        {
            DisplayProductToAdjust(product);

            btnAdd.Text = "Sửa";
            lblAddProductWarning.Visible = false;

            pnlAddProduct.BringToFront();

            btnAdd.Tag = product;
        }

        private void DisplayProductToAdjust(Product product)
        {
            picAddProductImage.Image = product.Image;
            txtAddProductName.Text = product.Name;
            txtAddProductDescription.Text = product.Description;
            txtAddProductPrice.Text = product.Price.ToString();
            txtAddProductQuantity.Text = product.Quantity.ToString();

            foreach (string category in cbxAddProductCategory.Items)
            {
                if (category == product.Category)
                    cbxAddProductCategory.SelectedItem = category;
            }
        }

        private SignInStatus CheckSignIn(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return SignInStatus.NotEnoughFields;

            foreach (User user in users)
            {
                if (user.Username == username && user.Password == password)
                    return SignInStatus.Success;
            }

            return SignInStatus.Invalid;
        }

        private SignUpStatus CheckSignUp(string username, string password, string password2)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(password2))
                return SignUpStatus.NotEnoughFields;

            foreach (User user in users)
            {
                if (user.Username == username)
                    return SignUpStatus.UsernameExisted;
            }

            if (password.Length < 6)
                return SignUpStatus.WeakPassword;

            if (password != password2)
                return SignUpStatus.PasswordMismatch;

            return SignUpStatus.Success;
        }

        private void SignIn()
        {
            string username = txtUsername_sign_in.Text;
            string password = txtPassword_sign_in.Text;

            SignInStatus result = CheckSignIn(username, password);

            if (result == SignInStatus.Success)
            {
                SwitchToShopping();
                LoadProducts();
                DisplayAllProducts(products);
            }
            else if (result == SignInStatus.NotEnoughFields)
            {
                lblWarning_sign_in.Text = "Chưa điền đủ các trường";
                lblWarning_sign_in.Visible = true;
            }
            else
            {
                lblWarning_sign_in.Text = "Sai tên đăng nhập hoặc mật khẩu";
                lblWarning_sign_in.Visible = true;
            }

            AlignCenterLabel(lblWarning_sign_in, pnlSignIn_sign_in);

            currentUsername = username;
        }

        private void SignUp()
        {
            string username = txtUsername_sign_up.Text;
            string password = txtPassword_sign_up.Text;
            string password2 = txtPasswordAgain_sign_up.Text;
            string email = txtEmail_sign_up.Text;

            SignUpStatus result = CheckSignUp(username, password, password2);

            if (result == SignUpStatus.NotEnoughFields)
            {
                lblWarning_sign_up.Text = "Chưa điền đủ các trường";
                lblWarning_sign_up.Visible = true;
            }
            else if (result == SignUpStatus.PasswordMismatch)
            {
                lblWarning_sign_up.Text = "Mật khẩu không khớp";
                lblWarning_sign_up.Visible = true;
            }
            else if (result == SignUpStatus.UsernameExisted)
            {
                lblWarning_sign_up.Text = "Tên đăng nhập đã tồn tại";
                lblWarning_sign_up.Visible = true;
            }
            else if (result == SignUpStatus.WeakPassword)
            {
                lblWarning_sign_up.Text = "Mật khẩu phải dài tối thiểu 6 kí tự";
                lblWarning_sign_up.ForeColor = Color.Red;
                lblWarning_sign_up.Visible = true;
            }
            else
            {
                lblWarning_sign_up.Text = "        Đăng kí thành công         ";
                lblWarning_sign_up.ForeColor = Color.Green;
                lblWarning_sign_up.Visible = true;

                WriteUserInfo(new User(username, password, email));
            }

            AlignCenterLabel(lblWarning_sign_up, pnlSignUp_sign_up);
        }

        private void ToggleCategoriesMenu()
        {
            int dy = pnlCategoriesHolder.Size.Height;
            bool isExpanded = pnlCategoriesHolder.Tag != null && (bool)pnlCategoriesHolder.Tag;

            if (isExpanded)
            {
                // collapse
                pnlCategoriesHolder.Visible = false;
                pnlWarehouse.Location = new Point(pnlWarehouse.Location.X, pnlWarehouse.Location.Y - dy);
            }
            else
            {
                // expand
                pnlCategoriesHolder.Visible = true;
                pnlWarehouse.Location = new Point(pnlWarehouse.Location.X, pnlWarehouse.Location.Y + dy);
            }

            pnlCategoriesHolder.Tag = !isExpanded;
        }

        private void SetCategoriesEvents()
        {
            pnlCategoriesHolder.Tag = true;

            ToggleCategoriesMenu();

            picCategories.Click += (sender, e) => ToggleCategoriesMenu();
            lblCategories.Click += (sender, e) => ToggleCategoriesMenu();
            pnlCategories.Click += (sender, e) => ToggleCategoriesMenu();
        }

        private void DisplayAllProducts(List<Product> productList)
        {
            List<Control> controlsToRemove = new List<Control>();

            foreach (Control control in pnlProductByCategory.Controls)
                if (control is ProductDisplay)
                    controlsToRemove.Add(control);

            foreach (Control control in controlsToRemove)
            {
                pnlProductByCategory.Controls.Remove(control);
                control.Dispose();
            }

            int dx = 15, dy = 15;
            int controlWidth = 290, controlHeight = 350;
            int maxControlsPerRow = 5;
            int x = 30, y = 0;

            int count = 0;
            foreach (Product product in productList)
            {
                ProductDisplay productDisplay = new ProductDisplay();

                productDisplay.SetProduct(product);
                productDisplay.Set();

                productDisplay.Location = new Point(x, y);

                pnlProductByCategory.Controls.Add(productDisplay);

                count += 1;

                x += controlWidth + dx;
                if (count % maxControlsPerRow == 0)
                {
                    x = lblProductType.Location.X;
                    y += controlHeight + dy;
                }

                productDisplay.Click += uctProductDisplay_Click;
            }
        }

        private void AddProduct(string username)
        {
            if (!CanAddProduct())
            {
                lblAddProductWarning.Text = "Chưa đủ các trường";
                lblAddProductWarning.ForeColor = Color.Red;
                lblAddProductWarning.Visible = true;
                return;
            }

            string productID = CreateID();
            string productName = txtAddProductName.Text.Trim();
            string productPrice = txtAddProductPrice.Text.Trim();
            string productQuantity = txtAddProductQuantity.Text.Trim();
            string productDescription = txtAddProductDescription.Text.Trim();
            string productCategory = cbxAddProductCategory.SelectedItem.ToString();

            string imagePath = Path.Combine(productImagePath, productID + ".jpg");
            string infoPath = Path.Combine(productInfoPath, productID + ".txt");
            string descriptionPath = Path.Combine(productDescriptionPath, productID + ".txt");
            string commentPath = Path.Combine(productCommentPath, productID + ".txt");

            // save Product image
            picAddProductImage.Image.Save(imagePath, ImageFormat.Jpeg);
            // save Product name, price and quantity
            using (StreamWriter writer = new StreamWriter(infoPath))
                writer.Write(productName + "***" + productPrice + "***" + productQuantity + "***" + productCategory + "***" + currentUsername);
            // save Product description
            using (StreamWriter writer = new StreamWriter(descriptionPath))
                writer.Write(productDescription);
            // add Product comment
            File.Create(commentPath).Dispose();

            ResetAddProductComponents();

            lblAddProductWarning.Text = "  Thêm thành công  ";
            lblAddProductWarning.ForeColor = Color.Green;
            lblAddProductWarning.Visible = true;
            cbxAddProductCategory.SelectedIndex = -1;
        }

        private void AdjustProduct(Product product)
        {
            if (!CanAddProduct())
            {
                lblAddProductWarning.Text = "Chưa đủ các trường";
                lblAddProductWarning.ForeColor = Color.Red;
                lblAddProductWarning.Visible = true;
                return;
            }

            string productName = txtAddProductName.Text.Trim();
            string productPrice = txtAddProductPrice.Text.Trim();
            string productQuantity = txtAddProductQuantity.Text.Trim();
            string productDescription = txtAddProductDescription.Text.Trim();
            string productCategory = cbxAddProductCategory.SelectedItem.ToString();

            string imagePath = Path.Combine(productImagePath, product.ID + ".jpg");
            string infoPath = Path.Combine(productInfoPath, product.ID + ".txt");
            string descriptionPath = Path.Combine(productDescriptionPath, product.ID + ".txt");

            // delete and save new Product image
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
            picAddProductImage.Image.Save(imagePath, ImageFormat.Jpeg);
            // save Product name, price and quantity
            using (StreamWriter writer = new StreamWriter(infoPath))
                writer.Write(productName + "***" + productPrice + "***" + productQuantity + "***" + productCategory + "***" + currentUsername);
            // save Product description
            using (StreamWriter writer = new StreamWriter(descriptionPath))
                writer.Write(productDescription);

            lblAddProductWarning.Text = "  Sửa thành công  ";
            lblAddProductWarning.ForeColor = Color.Green;
            lblAddProductWarning.Visible = true;
        }

        private void uctProductDisplay_Click(object sender, EventArgs e)
        {
            if (sender != null && sender is ProductDisplay productDisplay)
            {
                Product product = productDisplay.Product;

                if (product != null)
                    DisplayProduct(product);

                currentProduct = product;
            }

            if (sender != null && sender is ProductDisplayWarehouse productDisplayWarehouse)
            {
                Product product = productDisplayWarehouse.Product;

                if (product != null)
                    DisplayProduct(product);

                currentProduct = product;
            }
        }

        private List<Comment> LoadComment(string productID)
        {
            List<Comment> comments = new List<Comment>();

            string path = Path.Combine(productCommentPath, productID + ".txt");
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

        private List<Product> GetProductOfType(string category)
        {
            List<Product> result = new List<Product>();

            foreach (Product product in products)
            {
                if (product.Category == category)
                    result.Add(product);
            }

            return result;
        }

        private float ComputeRating(List<Comment> comments)
        {
            if (comments.Count == 0)
                return float.NaN; ;

            List<int> ratings = new List<int>();

            foreach (Comment comment in comments)
                ratings.Add(comment.Rating);

            return (float)ratings.Sum() / ratings.Count;
        }

        private void SetStars(float rating)
        {
            int integer_part = (int)Math.Truncate(rating);
            float fractional_part = rating - integer_part;

            List<PictureBox> stars = new List<PictureBox> { picStar1, picStar2, picStar3, picStar4, picStar5 };

            for (int i = 0; i < stars.Count; i++)
            {
                if (i < integer_part)
                    stars[i].ImageLocation = @"C:\Users\PC MY TU\Downloads\My Shopping\assets\icons\yellow_star.png";
                else
                    stars[i].ImageLocation = @"C:\Users\PC MY TU\Downloads\My Shopping\assets\icons\hollow_star.png";
            }

            if (integer_part < stars.Count && fractional_part >= 0.5)
                stars[integer_part].ImageLocation = @"C:\Users\PC MY TU\Downloads\My Shopping\assets\icons\half_star.png";
        }

        private bool CanAddProduct()
        {
            if (picAddProductImage.Image == null)
                return false;

            if (cbxAddProductCategory.SelectedIndex == -1)
                return false;

            if (string.IsNullOrWhiteSpace(txtAddProductName.Text))
                return false;

            if (string.IsNullOrWhiteSpace(txtAddProductDescription.Text))
                return false;

            if (string.IsNullOrWhiteSpace(txtAddProductQuantity.Text))
                return false;

            return true;
        }

        private void ResetUserRating()
        {
            currentUserRating = 0;
            List<PictureBox> stars = new List<PictureBox> { picRating1, picRating2, picRating3, picRating4, picRating5 };
            foreach (PictureBox star in stars)
                star.ImageLocation = @"C:\Users\PC MY TU\Downloads\My Shopping\assets\icons\hollow_star.png";
        }

        private void Rate(int score)
        {
            ResetUserRating();

            List<PictureBox> stars = new List<PictureBox> { picRating1, picRating2, picRating3, picRating4, picRating5 };

            for (int i = 0; i < score; i++)
                stars[i].ImageLocation = @"C:\Users\PC MY TU\Downloads\My Shopping\assets\icons\yellow_star.png";

            currentUserRating = score;
        }

        private void DisplayProduct(Product product)
        {
            ResetUserRating();
            txtComment.Text = "";

            pnlProductDisplay.Tag = product.ID;
            picProductImage.Image = product.Image;
            lblProductName.Text = product.Name;
            lblProductPrice.Text = "đ " + product.FormatPrice();
            lblProductDescriptionDetail.Text = product.Description;
            lblProductQuantity.Text = "Còn lại " + product.Quantity + " sản phẩm";
            lblSellerName.Text = File.ReadAllText(Path.Combine(userInfoPath, product.Seller + ".txt")).Split("***")[2];
            picSellerAvatar.ImageLocation = Path.Combine(userAvatarPath, product.Seller + ".jpg");
            txtQuantity.Text = "1";
            lblCategory.Text = "Thể loại: " + product.Category;

            lblProductName.Refresh();
            lblProductPrice.Refresh();

            lblProductPrice.Location = new Point(lblProductPrice.Location.X, lblProductName.Location.Y + lblProductName.Height + 10);
            lblProductComments.Location = new Point(lblProductComments.Location.X, lblProductDescriptionDetail.Location.Y + lblProductDescriptionDetail.Height + 40);
            txtComment.Location = new Point(txtComment.Location.X, lblProductComments.Bottom + 15);
            List<PictureBox> stars = new List<PictureBox> { picRating1, picRating2, picRating3, picRating4, picRating5 };
            foreach (PictureBox star in stars)
            {
                star.Location = new Point(star.Location.X, txtComment.Location.Y);
            }

            List<Comment> comments = LoadComment(product.ID);
            int x = lblProductComments.Location.X, y = txtComment.Location.Y + txtComment.Height + 10;

            foreach (Comment comment in comments)
            {
                CommentControl commentControl = new CommentControl();
                commentControl.Set(comment.Username, comment.Content, comment.Rating);

                commentControl.Location = new Point(x, y);
                y += commentControl.Size.Height + 15;

                pnlProductDisplay.Controls.Add(commentControl);
            }

            float rating = ComputeRating(comments);
            if (float.IsNaN(rating))
                lblProductStars.Text = "Chưa có đánh giá";
            else
                lblProductStars.Text = rating.ToString("F1");

            SetStars(rating);

            pnlProductDisplay.AutoScrollPosition = new Point(0, 0);
            pnlProductDisplay.BringToFront();

            if ((product.Seller == currentUsername) || (product.Quantity == 0))
                btnAddToCart.Enabled = false;
            else
                btnAddToCart.Enabled = true;

            if (product.Seller == currentUsername)
            {
                txtComment.Enabled = false;
                foreach (PictureBox star in stars)
                    star.Enabled = false;
            }
            else
            {
                txtComment.Enabled = true;
                foreach (PictureBox star in stars)
                    star.Enabled = true;
            }
        }

        private void ResetAddProductComponents()
        {
            // release image
            if (picAddProductImage.Image != null)
            {
                picAddProductImage.Image.Dispose();
                picAddProductImage.Image = null;
            }

            txtAddProductName.Clear();
            txtAddProductDescription.Clear();
            txtAddProductQuantity.Clear();
            txtAddProductPrice.Clear();
            lblAddProductWarning.Visible = false;
        }

        private void DecreaseProductQuantityInCart(string productID, TextBox txtBox)
        {
            int value = int.Parse(txtBox.Text);
            if (value > 0)
                value--;
            txtBox.Text = value.ToString();
            cart[productID] = value;
            DisplayProductsInCart();
        }

        private void IncreaseProductQuantityInCart(string ProductID, TextBox txtBox, int maxQuantity)
        {
            int value = int.Parse(txtBox.Text);
            if (value < maxQuantity)
                value++;
            txtBox.Text = value.ToString();
            cart[ProductID] = value;
            DisplayProductsInCart();
        }

        private void UpdateProductQuantityInCart(string productID, int quantity, int maxQuantity)
        {
            if (quantity <= maxQuantity)
                cart[productID] = quantity;
            else
                cart[productID] = maxQuantity;
            DisplayProductsInCart();
        }

        private void RemoveProductFromCart(string ProductID)
        {
            cart.Remove(ProductID);
            DisplayProductsInCart();
        }

        private void txtQuantity_KeyDown(object sender, KeyEventArgs e, string productID, int maxQuantity)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int quantity = int.Parse(((TextBox)sender).Text);
                UpdateProductQuantityInCart(productID, quantity, maxQuantity);
                e.SuppressKeyPress = true;
            }
        }

        private void DisplayProductsInCart()
        {
            pnlCart.AutoScrollPosition = new Point(0, 0);

            List<Control> controlsToRemove = new List<Control>();

            foreach (Control control in pnlCart.Controls)
                if (control.Tag == null || control.Tag != "component")
                    controlsToRemove.Add(control);

            foreach (Control control in controlsToRemove)
            {
                pnlCart.Controls.Remove(control);
                control.Dispose();
            }

            if (cart.Count == 0)
            {
                btnPurchase.Visible = false;
                lblTotalPrice.Visible = false;
            }
            else
            {
                btnPurchase.Visible = true;
                lblTotalPrice.Visible = true;
            }

            int x = 53, y = 136;
            int totalPrice = 0;

            foreach (KeyValuePair<string, int> element in cart)
            {
                CultureInfo culture = new CultureInfo("de-DE");
                string[] info = File.ReadAllText(Path.Combine(productInfoPath, element.Key + ".txt")).Split("***");
                string productName = info[0];
                int productPrice = int.Parse(info[1]);
                int productMaxQuantity = int.Parse(info[2]);
                int priceMiddlePoint = lblCartPrice.Left + lblCartPrice.Width / 2;
                int totalPriceMiddlePoint = lblTotalPriceOfProduct.Left + lblTotalPriceOfProduct.Width / 2;

                Font font = new Font("Segoe UI", 12);

                int componentX, componentY = 34;

                // display each product
                // panel to hold product's info
                Panel panel = new Panel();
                panel.Tag = element.Key;
                panel.Size = new Size(1397, 100);
                panel.Location = new Point(x, y);
                panel.BorderStyle = BorderStyle.FixedSingle;
                pnlCart.Controls.Add(panel);

                // product image
                PictureBox pic = new PictureBox();
                panel.Controls.Add(pic);
                pic.Size = new Size(100, 100);
                pic.Location = new Point(0, 0);
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.ImageLocation = Path.Combine(productImagePath, element.Key + ".jpg");

                // product name
                Label labelName = new Label();
                panel.Controls.Add(labelName);
                labelName.Text = productName;
                labelName.Font = font;
                labelName.AutoSize = true;
                labelName.MaximumSize = new Size(420, 40);
                labelName.Location = new Point(115, componentY);

                // product price
                Label labelPrice = new Label();
                panel.Controls.Add(labelPrice);
                labelPrice.Text = "đ " + productPrice.ToString("N0", culture);
                labelPrice.Font = font;
                labelPrice.ForeColor = Color.Red;
                labelPrice.AutoSize = true;
                componentX = priceMiddlePoint - panel.Left - labelPrice.Width / 2;
                labelPrice.Location = new Point(componentX, componentY);

                // product quantity in cart
                TextBox txtQuantity = new TextBox();
                panel.Controls.Add(txtQuantity);
                txtQuantity.Font = new Font("Segoe UI", 10);
                txtQuantity.TextAlign = HorizontalAlignment.Center;
                txtQuantity.Text = element.Value.ToString();
                txtQuantity.Size = new Size(89, 34);
                txtQuantity.Location = new Point(910, 32);
                txtQuantity.KeyDown += (sender, e) => txtQuantity_KeyDown(sender, e, element.Key, productMaxQuantity);

                // picture box to decrease product quantity in cart
                PictureBox picDecrease = new PictureBox();
                panel.Controls.Add(picDecrease);
                picDecrease.SizeMode = PictureBoxSizeMode.StretchImage;
                picDecrease.BorderStyle = BorderStyle.FixedSingle;
                picDecrease.Size = new Size(34, 34);
                picDecrease.ImageLocation = @"C:\Users\PC MY TU\Downloads\My Shopping\assets\icons\minus.png";
                picDecrease.Location = new Point(876, 32);
                picDecrease.Click += (sender, e) => DecreaseProductQuantityInCart(element.Key, txtQuantity);

                // picture box to increase product quantity in cart
                PictureBox picIncrease = new PictureBox();
                panel.Controls.Add(picIncrease);
                picIncrease.SizeMode = PictureBoxSizeMode.StretchImage;
                picIncrease.BorderStyle = BorderStyle.FixedSingle;
                picIncrease.Size = new Size(34, 34);
                picIncrease.ImageLocation = @"C:\Users\PC MY TU\Downloads\My Shopping\assets\icons\plus.png";
                picIncrease.Location = new Point(999, 32);
                picIncrease.Click += (sender, e) => IncreaseProductQuantityInCart(element.Key, txtQuantity, productMaxQuantity);

                // total price per product
                Label labelTotalPrice = new Label();
                panel.Controls.Add(labelTotalPrice);
                int totalPricePerProduct = productPrice * int.Parse(txtQuantity.Text);
                labelTotalPrice.Text = "đ " + totalPricePerProduct.ToString("N0", culture);
                labelTotalPrice.Font = font;
                labelTotalPrice.ForeColor = Color.Red;
                labelTotalPrice.AutoSize = true;
                componentX = totalPriceMiddlePoint - panel.Left - labelTotalPrice.Width / 2;
                labelTotalPrice.Location = new Point(componentX, componentY);

                // picture box to remove product from cart
                PictureBox picRemove = new PictureBox();
                pnlCart.Controls.Add(picRemove);
                picRemove.SizeMode = PictureBoxSizeMode.StretchImage;
                picRemove.Size = new Size(50, 50);
                picRemove.ImageLocation = @"C:\Users\PC MY TU\Downloads\My Shopping\assets\icons\delete.png";
                picRemove.Location = new Point(panel.Right + 20, panel.Top + (panel.Height) / 2 - picRemove.Height / 2);
                picRemove.Click += (sender, e) => RemoveProductFromCart(element.Key);

                // update position and content of total price label
                totalPrice += productPrice * element.Value;
                lblTotalPrice.Text = "Tổng tiền: " + totalPrice.ToString("N0", culture) + "đ";
                lblTotalPrice.Location = new Point(panel.Right - lblTotalPrice.Width, panel.Bottom + 5);

                // update position of purchase button
                btnPurchase.Location = new Point(btnPurchase.Left, lblTotalPrice.Bottom + 20);

                y = panel.Bottom + 10;
            }
        }

        private void DeleteProductFromCart(string productID)
        {
            // xóa productID và productQuantity
        }

        private void picDeleteFromCart_Click(object sender, EventArgs e)
        {
            Control temp = (Control)sender;
            string productID = temp.Tag.ToString();

            foreach (Control control in pnlCart.Controls)
            {
                Panel panel = (Panel)control;

                if (panel.Tag.ToString() == productID)
                {
                    pnlCart.Controls.Remove(panel);
                    panel.Dispose();
                    DeleteProductFromCart(productID);
                }
            }

            DisplayProductsInCart();
            if (cart.Count == 0)
                lblCartNumItems.Text = "";
        }

        private void DeleteProductFromWarehouse(Product product)
        {
            string infoPath = Path.Combine(productInfoPath, product.ID + ".txt");
            string descriptionPath = Path.Combine(productDescriptionPath, product.ID + ".txt");
            string commentPath = Path.Combine(productCommentPath, product.ID + ".txt");
            string imagePath = Path.Combine(productImagePath, product.ID + ".jpg");

            if (File.Exists(infoPath))
                File.Delete(infoPath);
            if (File.Exists(descriptionPath))
                File.Delete(descriptionPath);
            if (File.Exists(commentPath))
                File.Delete(commentPath);
            if (File.Exists(imagePath))
                File.Delete(imagePath);

            DisplayWarehouse(currentUsername);
        }

        private void DisplayWarehouse(string username)
        {
            LoadProducts();

            List<Control> controlsToRemove = new List<Control>();

            // Collect all PictureBox controls
            foreach (Control control in pnlWarehouseUser.Controls)
                if (control is ProductDisplayWarehouse || control.Tag == "picAdjust" || control.Tag == "picDelete")
                    controlsToRemove.Add(control);

            // RemoveProductFromCart the collected PictureBox controls
            foreach (Control control in controlsToRemove)
            {
                pnlWarehouseUser.Controls.Remove(control);
                control.Dispose();
            }

            int x = 50, y = lblWarehouseProduct.Bottom + 20;

            foreach (Product product in products)
            {
                if (product.Seller == currentUsername)
                {
                    // display product
                    ProductDisplayWarehouse productDisplay = new ProductDisplayWarehouse();
                    productDisplay.SetProduct(product);
                    productDisplay.Location = new Point(x, y);
                    productDisplay.Click += uctProductDisplay_Click;
                    pnlWarehouseUser.Controls.Add(productDisplay);

                    // display adjust picture
                    PictureBox picAdjust = new PictureBox();
                    picAdjust.ImageLocation = @"C:\Users\PC MY TU\Downloads\My Shopping\assets\icons\edit.png";
                    picAdjust.Size = new Size(50, 50);
                    picAdjust.SizeMode = PictureBoxSizeMode.StretchImage;
                    int xAdjust = productDisplay.Right + 25;
                    int yAdjust = y + (productDisplay.Bottom - productDisplay.Top - picAdjust.Height) / 2;
                    picAdjust.Location = new Point(xAdjust, yAdjust);
                    picAdjust.Click += (sender, e) => SwitchToAdjustProduct(product);
                    picAdjust.Tag = "picAdjust";
                    pnlWarehouseUser.Controls.Add(picAdjust);

                    // display delete picture
                    PictureBox picDelete = new PictureBox();
                    picDelete.ImageLocation = @"C:\Users\PC MY TU\Downloads\My Shopping\assets\icons\delete.png";
                    picDelete.Size = new Size(50, 50);
                    picDelete.SizeMode = PictureBoxSizeMode.StretchImage;
                    picDelete.Location = new Point(picAdjust.Right + 30, yAdjust);
                    picDelete.Click += (sender, e) => DeleteProductFromWarehouse(product);
                    picDelete.Tag = "picDelete";
                    pnlWarehouseUser.Controls.Add(picDelete);

                    y += productDisplay.Size.Height + 10;
                }
            }
        }

        private void lblSignUpNow_Click(object sender, EventArgs e)
        {
            SwitchToSignUp();
        }

        private void lblSignIn_sign_up_Click(object sender, EventArgs e)
        {
            SwitchToSignIn();
        }

        private void btnSignIn_sign_in_Click(object sender, EventArgs e)
        {
            SignIn();
        }

        private void lblForgetPassword_sign_in_Click(object sender, EventArgs e)
        {
            // tạo 1 form mới
            lblUsername_sign_in.Text = "a";
        }

        private void btnSignUp_sign_up_Click(object sender, EventArgs e)
        {
            SignUp();
        }

        private void txtEmail_sign_up_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SignUp();
                e.SuppressKeyPress = true;
            }
        }

        private void txtUsername_sign_up_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SignUp();
                e.SuppressKeyPress = true;
            }
        }

        private void txtPassword_sign_up_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SignUp();
                e.SuppressKeyPress = true;
            }
        }

        private void txtPasswordAgain_sign_up_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SignUp();
                e.SuppressKeyPress = true;
            }
        }

        private void txtUsername_sign_in_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SignIn();
                e.SuppressKeyPress = true;
            }
        }

        private void txtPassword_sign_in_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SignIn();
                e.SuppressKeyPress = true;
            }
        }

        private void picHome_Click(object sender, EventArgs e)
        {
            lblProductType.Text = "Tất cả sản phẩm";
            SwitchToShopping();
            LoadProducts();
            productsByCategory = products;
            DisplayAllProducts(productsByCategory);
        }

        private void picDecreaseQuantity_Click(object sender, EventArgs e)
        {
            int value = int.Parse(txtQuantity.Text);
            if (value > 1)
                value--;
            txtQuantity.Text = value.ToString();
            picDecreaseQuantity.Focus();
        }

        private void picIncreaseQuantity_Click(object sender, EventArgs e)
        {
            int value = int.Parse(txtQuantity.Text);
            if (value + 1 <= currentProduct.Quantity)
                value++;
            txtQuantity.Text = value.ToString();
            picIncreaseQuantity.Focus();
        }

        private List<Product> SearchProducts(string textTosearch)
        {
            List<Product> result = new List<Product>();

            foreach (Product product in productsByCategory)
                if (product.Name.ToLower().Contains(textTosearch.ToLower()))
                    result.Add(product);

            return result;
        }

        private void lblWarehouse_Click(object sender, EventArgs e)
        {
            SwitchToWarehouse(currentUsername);
        }

        private void picWarehouse_Click(object sender, EventArgs e)
        {
            SwitchToWarehouse(currentUsername);
        }

        private void pnlWarehouse_Click(object sender, EventArgs e)
        {
            SwitchToWarehouse(currentUsername);
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            SwitchToAddProduct(currentUsername);
        }

        private void btnAddProductImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string imagePath = openFileDialog.FileName;
                picAddProductImage.Image = Image.FromFile(imagePath);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "Thêm")
            {
                AddProduct(currentUsername);
                LoadProducts();
            }
            else
            {
                Button b = (Button)sender;
                AdjustProduct((Product)(b.Tag));
                LoadProducts();
            }
        }

        private void lblCategoriesAll_Click(object sender, EventArgs e)
        {
            lblProductType.Text = "Tất cả sản phẩm";
            LoadProducts();
            productsByCategory = products;
            DisplayAllProducts(productsByCategory);
        }

        private void pnlCategoriesAll_Click(object sender, EventArgs e)
        {
            lblProductType.Text = "Tất cả sản phẩm";
            LoadProducts();
            productsByCategory = products;
            DisplayAllProducts(productsByCategory);
        }

        private void lblCategoriesToy_Click(object sender, EventArgs e)
        {
            lblProductType.Text = "Đồ chơi";
            productsByCategory = GetProductOfType("Đồ chơi");
            DisplayAllProducts(productsByCategory);
        }

        private void pnlCategoriesToy_Click(object sender, EventArgs e)
        {
            lblProductType.Text = "Đồ chơi";
            productsByCategory = GetProductOfType("Đồ chơi");
            DisplayAllProducts(productsByCategory);
        }

        private void lblCategoriesBook_Click(object sender, EventArgs e)
        {
            lblProductType.Text = "Sách";
            productsByCategory = GetProductOfType("Sách");
            DisplayAllProducts(productsByCategory);
        }

        private void pnlCategoriesBook_Click(object sender, EventArgs e)
        {
            lblProductType.Text = "Sách";
            productsByCategory = GetProductOfType("Sách");
            DisplayAllProducts(productsByCategory);
        }

        private void lblCategoriesHealth_Click(object sender, EventArgs e)
        {
            lblProductType.Text = "Sức khỏe";
            productsByCategory = GetProductOfType("Sức khỏe");
            DisplayAllProducts(productsByCategory);
        }

        private void pnlCategoriesHealth_Click(object sender, EventArgs e)
        {
            lblProductType.Text = "Sức khỏe";
            productsByCategory = GetProductOfType("Sức khỏe");
            DisplayAllProducts(productsByCategory);
        }

        private void lblCategoriesHousehold_Click(object sender, EventArgs e)
        {
            lblProductType.Text = "Thiết bị gia dụng";
            productsByCategory = GetProductOfType("Thiết bị gia dụng");
            DisplayAllProducts(productsByCategory);
        }

        private void pnlCategoriesHousehold_Click(object sender, EventArgs e)
        {
            lblProductType.Text = "Thiết bị gia dụng";
            productsByCategory = GetProductOfType("Thiết bị gia dụng");
            DisplayAllProducts(productsByCategory);
        }

        private void lblCategoriesFashion_Click(object sender, EventArgs e)
        {
            lblProductType.Text = "Thời trang";
            productsByCategory = GetProductOfType("Thời trang");
            DisplayAllProducts(productsByCategory);
        }

        private void pnlCategoriesFashion_Click(object sender, EventArgs e)
        {
            lblProductType.Text = "Thời trang";
            productsByCategory = GetProductOfType("Thời trang");
            DisplayAllProducts(productsByCategory);
        }

        private void lblCategoriesElectronic_Click(object sender, EventArgs e)
        {
            lblProductType.Text = "Thiết bị điện tử";
            productsByCategory = GetProductOfType("Thiết bị điện tử");
            DisplayAllProducts(productsByCategory);
        }

        private void pnlCategoriesElectronic_Click(object sender, EventArgs e)
        {
            lblProductType.Text = "Thiết bị điện tử";
            productsByCategory = GetProductOfType("Thiết bị điện tử");
            DisplayAllProducts(productsByCategory);
        }

        private void lblCategoriesFood_Click(object sender, EventArgs e)
        {
            lblProductType.Text = "Thực phẩm";
            productsByCategory = GetProductOfType("Thực phẩm");
            DisplayAllProducts(productsByCategory);
        }

        private void pnlCategoriesFood_Click(object sender, EventArgs e)
        {
            lblProductType.Text = "Thực phẩm";
            productsByCategory = GetProductOfType("Thực phẩm");
            DisplayAllProducts(productsByCategory);
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            int quantity = int.Parse(txtQuantity.Text);

            if (cart.ContainsKey(currentProduct.ID))
                cart[currentProduct.ID] += quantity;
            else
                cart.Add(currentProduct.ID, quantity);

            if (cart.Count > 0 && cart.Count < 10)
                lblCartNumItems.Text = cart.Count.ToString();
            else if (cart.Count >= 10)
                lblCartNumItems.Text = "9+";
        }

        private void picCart_Click(object sender, EventArgs e)
        {
            SwitchToCart();
            DisplayProductsInCart();
        }

        private void btnPurchase_Click(object sender, EventArgs e)
        {
            string content =
                @"
                <table style='border-collapse: collapse; width: 100%;'>
                <thead>
                    <tr style='background-color: #4da6ff;'>
                        <th style='border: 1px solid black; padding: 10px; text-align: center; vertical-align: top;'>Sản phẩm</th>
                        <th style='border: 1px solid black; padding: 10px; text-align: center; vertical-align: top;'>Đơn giá</th>
                        <th style='border: 1px solid black; padding: 10px; text-align: center; vertical-align: top;'>Số lượng</th>
                        <th style='border: 1px solid black; padding: 10px; text-align: center; vertical-align: top;'>Thành tiền</th>
                    </tr>
                </thead>
                <tbody>";
            int totalPrice = 0;
            CultureInfo culture = new CultureInfo("de-DE");

            foreach (KeyValuePair<string, int> element in cart)
            {
                string productPath = Path.Combine(productInfoPath, element.Key + ".txt");
                string[] productInfo = File.ReadAllText(productPath).Split("***");
                string productName = productInfo[0];
                int productPrice = int.Parse(productInfo[1]);
                int productQuantity = int.Parse(productInfo[2]);
                string productCategory = productInfo[3];
                string seller = productInfo[4];

                content += "<tr>";
                content += $"<td style='border: 1px solid black; padding: 10px; text-align: center; vertical-align: top;'>{productName}</td>";
                content += $"<td style='border: 1px solid black; padding: 10px; text-align: center; vertical-align: top;'>{productPrice.ToString("N0", culture)}</td>";
                content += $"<td style='border: 1px solid black; padding: 10px; text-align: center; vertical-align: top;'>{element.Value}</td>";
                content += $"<td style='border: 1px solid black; padding: 10px; text-align: center; vertical-align: top;'>{(productPrice * element.Value).ToString("N0", culture)}</td>";
                content += "</tr>";

                totalPrice += productPrice * element.Value;

                using (StreamWriter writer = new StreamWriter(productPath, false))
                {
                    writer.Write(
                                productName + "***" + productPrice.ToString() + "***" +
                                (productQuantity - element.Value).ToString() + "***" +
                                productCategory + "***" + seller
                                );
                }
            }

            content += $"<tr><td colspan='2' style='border: 1px solid black; padding: 10px; text-align: center; vertical-align: top;'>Tổng tiền:</td>";
            content += $"<td colspan='2' style='border: 1px solid black; padding: 10px; text-align: center; vertical-align: top;'>{totalPrice.ToString("N0", culture)}</td>";
            content += "</tr></tbody></table>";

            MailMessage mail = new MailMessage();
            mail.From = new System.Net.Mail.MailAddress("21522740@gm.uit.edu.vn");
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(mail.From.Address, "vddz awiq tqdy tvva");
            smtp.Host = "smtp.gmail.com";

            mail.To.Add(new MailAddress(File.ReadAllText(Path.Combine(userInfoPath, currentUsername + ".txt")).Split("***")[1]));
            mail.IsBodyHtml = true;
            mail.Subject = "MyShopping Bill";
            mail.Body = content;

            smtp.Send(mail);

            // clear the cart
            cart.Clear();
            DisplayProductsInCart();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DisplayAllProducts(SearchProducts(txtSearch.Text.Trim()));
                e.SuppressKeyPress = true;
            }
        }

        private void picSearch_Click(object sender, EventArgs e)
        {
            DisplayAllProducts(SearchProducts(txtSearch.Text.Trim()));
        }

        private void picUser_Click(object sender, EventArgs e)
        {
            SwitchToUser();
        }

        private void lblSignOut_Click(object sender, EventArgs e)
        {
            SwitchToSignIn();
        }

        private void txtNewUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string[] values = File.ReadAllText(Path.Combine(userInfoPath, currentUsername + ".txt")).Split("***");
                string password = values[0];
                string email = values[1];

                using (StreamWriter writer = new StreamWriter(Path.Combine(userInfoPath, currentUsername + ".txt")))
                    writer.WriteLine(password + "***" + email + "***" + txtNewUserName.Text.Trim());

                SwitchToUser();
                e.SuppressKeyPress = true;
            }
        }

        private void picUserNameEdit_Click(object sender, EventArgs e)
        {
            txtNewUserName.BringToFront();
            txtNewUserName.Visible = true;
            txtNewUserName.Text = lblUserName.Text;
            txtNewUserName.Focus();
            txtNewUserName.SelectAll();
        }

        private void picUserAvatarEdit_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string imagePath = openFileDialog.FileName;
                string originalPath = Path.Combine(userAvatarPath, currentUsername + ".jpg");

                if (File.Exists(originalPath))
                    File.Delete(originalPath);
                File.Copy(imagePath, originalPath);
            }

            SwitchToUser();
        }

        private void picRating1_Click(object sender, EventArgs e)
        {
            Rate(1);
        }

        private void picRating2_Click(object sender, EventArgs e)
        {
            Rate(2);
        }

        private void picRating3_Click(object sender, EventArgs e)
        {
            Rate(3);
        }

        private void picRating4_Click(object sender, EventArgs e)
        {
            Rate(4);
        }

        private void picRating5_Click(object sender, EventArgs e)
        {
            Rate(5);
        }

        private void txtComment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string comment = txtComment.Text.Trim();
                string productID = pnlProductDisplay.Tag.ToString();

                if (!string.IsNullOrWhiteSpace(comment) && currentUserRating != 0)
                    using (StreamWriter writer = new StreamWriter(Path.Combine(productCommentPath, productID + ".txt"), true))
                        writer.WriteLine(currentUsername + "***" + comment + "***" + currentUserRating);

                e.SuppressKeyPress = true;

                foreach (Product product in products)
                {
                    if (product.ID == productID)
                    {
                        DisplayProduct(product);
                        break;
                    }
                }
            }
        }
    }
}