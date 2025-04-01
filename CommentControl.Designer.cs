namespace My_Shopping
{
    partial class CommentControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommentControl));
            picAvatar = new PictureBox();
            lblUsername = new Label();
            picStar1 = new PictureBox();
            picStar2 = new PictureBox();
            picStar3 = new PictureBox();
            picStar4 = new PictureBox();
            picStar5 = new PictureBox();
            lblComment = new Label();
            ((System.ComponentModel.ISupportInitialize)picAvatar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picStar1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picStar2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picStar3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picStar4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picStar5).BeginInit();
            SuspendLayout();
            // 
            // picAvatar
            // 
            picAvatar.Image = (Image)resources.GetObject("picAvatar.Image");
            picAvatar.Location = new Point(23, 15);
            picAvatar.Name = "picAvatar";
            picAvatar.Size = new Size(75, 75);
            picAvatar.SizeMode = PictureBoxSizeMode.StretchImage;
            picAvatar.TabIndex = 0;
            picAvatar.TabStop = false;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI", 11F);
            lblUsername.Location = new Point(104, 15);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(138, 30);
            lblUsername.TabIndex = 1;
            lblUsername.Text = "<username>";
            // 
            // picStar1
            // 
            picStar1.Image = (Image)resources.GetObject("picStar1.Image");
            picStar1.Location = new Point(115, 60);
            picStar1.Name = "picStar1";
            picStar1.Size = new Size(30, 30);
            picStar1.SizeMode = PictureBoxSizeMode.StretchImage;
            picStar1.TabIndex = 2;
            picStar1.TabStop = false;
            // 
            // picStar2
            // 
            picStar2.Image = (Image)resources.GetObject("picStar2.Image");
            picStar2.Location = new Point(150, 60);
            picStar2.Name = "picStar2";
            picStar2.Size = new Size(30, 30);
            picStar2.SizeMode = PictureBoxSizeMode.StretchImage;
            picStar2.TabIndex = 2;
            picStar2.TabStop = false;
            // 
            // picStar3
            // 
            picStar3.Image = (Image)resources.GetObject("picStar3.Image");
            picStar3.Location = new Point(185, 60);
            picStar3.Name = "picStar3";
            picStar3.Size = new Size(30, 30);
            picStar3.SizeMode = PictureBoxSizeMode.StretchImage;
            picStar3.TabIndex = 2;
            picStar3.TabStop = false;
            // 
            // picStar4
            // 
            picStar4.Image = (Image)resources.GetObject("picStar4.Image");
            picStar4.Location = new Point(220, 60);
            picStar4.Name = "picStar4";
            picStar4.Size = new Size(30, 30);
            picStar4.SizeMode = PictureBoxSizeMode.StretchImage;
            picStar4.TabIndex = 2;
            picStar4.TabStop = false;
            // 
            // picStar5
            // 
            picStar5.Image = (Image)resources.GetObject("picStar5.Image");
            picStar5.Location = new Point(255, 60);
            picStar5.Name = "picStar5";
            picStar5.Size = new Size(30, 30);
            picStar5.SizeMode = PictureBoxSizeMode.StretchImage;
            picStar5.TabIndex = 2;
            picStar5.TabStop = false;
            // 
            // lblComment
            // 
            lblComment.AutoSize = true;
            lblComment.Font = new Font("Segoe UI", 12F);
            lblComment.Location = new Point(23, 115);
            lblComment.MaximumSize = new Size(1500, 0);
            lblComment.Name = "lblComment";
            lblComment.Size = new Size(148, 32);
            lblComment.TabIndex = 3;
            lblComment.Text = "<comment>";
            // 
            // CommentControl
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(lblComment);
            Controls.Add(picStar5);
            Controls.Add(picStar4);
            Controls.Add(picStar3);
            Controls.Add(picStar2);
            Controls.Add(picStar1);
            Controls.Add(lblUsername);
            Controls.Add(picAvatar);
            Name = "CommentControl";
            Size = new Size(1525, 162);
            Load += CommentControl_Load;
            ((System.ComponentModel.ISupportInitialize)picAvatar).EndInit();
            ((System.ComponentModel.ISupportInitialize)picStar1).EndInit();
            ((System.ComponentModel.ISupportInitialize)picStar2).EndInit();
            ((System.ComponentModel.ISupportInitialize)picStar3).EndInit();
            ((System.ComponentModel.ISupportInitialize)picStar4).EndInit();
            ((System.ComponentModel.ISupportInitialize)picStar5).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picAvatar;
        private Label lblUsername;
        private PictureBox picStar1;
        private PictureBox picStar2;
        private PictureBox picStar3;
        private PictureBox picStar4;
        private PictureBox picStar5;
        private Label lblComment;
    }
}
