namespace My_Shopping
{
    partial class ProductDisplayWarehouse
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductDisplayWarehouse));
            picImage = new PictureBox();
            lblName = new Label();
            lblPrice = new Label();
            lblQuantity = new Label();
            ((System.ComponentModel.ISupportInitialize)picImage).BeginInit();
            SuspendLayout();
            // 
            // picImage
            // 
            picImage.Image = (Image)resources.GetObject("picImage.Image");
            picImage.Location = new Point(10, 10);
            picImage.Name = "picImage";
            picImage.Size = new Size(100, 100);
            picImage.SizeMode = PictureBoxSizeMode.StretchImage;
            picImage.TabIndex = 0;
            picImage.TabStop = false;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Font = new Font("Segoe UI", 12F);
            lblName.Location = new Point(130, 44);
            lblName.MaximumSize = new Size(610, 35);
            lblName.Name = "lblName";
            lblName.Size = new Size(106, 32);
            lblName.TabIndex = 1;
            lblName.Text = "<name>";
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.Font = new Font("Segoe UI", 12F);
            lblPrice.ForeColor = Color.Red;
            lblPrice.Location = new Point(800, 44);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(119, 32);
            lblPrice.TabIndex = 1;
            lblPrice.Text = "đ <price>";
            // 
            // lblQuantity
            // 
            lblQuantity.AutoSize = true;
            lblQuantity.Font = new Font("Segoe UI", 12F);
            lblQuantity.Location = new Point(1090, 44);
            lblQuantity.Name = "lblQuantity";
            lblQuantity.Size = new Size(134, 32);
            lblQuantity.TabIndex = 1;
            lblQuantity.Text = "<quantity>";
            // 
            // ProductDisplayWarehouse
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(lblQuantity);
            Controls.Add(lblPrice);
            Controls.Add(lblName);
            Controls.Add(picImage);
            Name = "ProductDisplayWarehouse";
            Size = new Size(1278, 118);
            Load += ProductDisplayWarehouse_Load;
            ((System.ComponentModel.ISupportInitialize)picImage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picImage;
        private Label lblName;
        private Label lblPrice;
        private Label lblQuantity;
    }
}
