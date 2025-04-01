namespace My_Shopping.User_Controls
{
    partial class ProductDisplay
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param Username="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductDisplay));
            picProductImage = new PictureBox();
            lblProductName = new Label();
            lblPrice = new Label();
            picStar = new PictureBox();
            lblRating = new Label();
            ((System.ComponentModel.ISupportInitialize)picProductImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picStar).BeginInit();
            SuspendLayout();
            // 
            // picProductImage
            // 
            picProductImage.Image = (Image)resources.GetObject("picProductImage.Image");
            picProductImage.Location = new Point(0, 0);
            picProductImage.Name = "picProductImage";
            picProductImage.Size = new Size(290, 198);
            picProductImage.SizeMode = PictureBoxSizeMode.StretchImage;
            picProductImage.TabIndex = 0;
            picProductImage.TabStop = false;
            // 
            // lblProductName
            // 
            lblProductName.AutoSize = true;
            lblProductName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblProductName.Location = new Point(0, 201);
            lblProductName.MaximumSize = new Size(290, 70);
            lblProductName.Name = "lblProductName";
            lblProductName.Size = new Size(196, 32);
            lblProductName.TabIndex = 1;
            lblProductName.Text = "<product name>";
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPrice.ForeColor = Color.Red;
            lblPrice.Location = new Point(0, 315);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(91, 30);
            lblPrice.TabIndex = 1;
            lblPrice.Text = "<price>";
            // 
            // picStar
            // 
            picStar.Image = (Image)resources.GetObject("picStar.Image");
            picStar.Location = new Point(5, 279);
            picStar.Name = "picStar";
            picStar.Size = new Size(30, 30);
            picStar.SizeMode = PictureBoxSizeMode.StretchImage;
            picStar.TabIndex = 2;
            picStar.TabStop = false;
            // 
            // lblRating
            // 
            lblRating.AutoSize = true;
            lblRating.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblRating.ForeColor = Color.FromArgb(252, 196, 25);
            lblRating.Location = new Point(41, 279);
            lblRating.Name = "lblRating";
            lblRating.Size = new Size(99, 30);
            lblRating.TabIndex = 1;
            lblRating.Text = "<rating>";
            // 
            // ProductDisplay
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(picStar);
            Controls.Add(lblRating);
            Controls.Add(lblPrice);
            Controls.Add(lblProductName);
            Controls.Add(picProductImage);
            Name = "ProductDisplay";
            Size = new Size(290, 350);
            Load += ProductDisplay_Load;
            ((System.ComponentModel.ISupportInitialize)picProductImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)picStar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picProductImage;
        private Label lblProductName;
        private Label lblPrice;
        private PictureBox picStar;
        private Label lblRating;
    }
}
