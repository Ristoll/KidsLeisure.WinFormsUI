namespace KidsLeisure.UI
{
    partial class AuthorizationWin
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuthorizationWin));
            label3 = new Label();
            label2 = new Label();
            textBox3 = new TextBox();
            textBox2 = new TextBox();
            button1 = new Button();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(293, 327);
            label3.Name = "label3";
            label3.Size = new Size(77, 20);
            label3.TabIndex = 21;
            label3.Text = "Phone No.";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(293, 294);
            label2.Name = "label2";
            label2.Size = new Size(78, 20);
            label2.TabIndex = 20;
            label2.Text = "NickName";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(389, 320);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(125, 27);
            textBox3.TabIndex = 17;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(389, 287);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(125, 27);
            textBox2.TabIndex = 16;
            // 
            // button1
            // 
            button1.Location = new Point(319, 402);
            button1.Name = "button1";
            button1.Size = new Size(175, 36);
            button1.TabIndex = 14;
            button1.Text = "Authorize";
            button1.UseVisualStyleBackColor = true;
            button1.Click += this.button1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(286, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(239, 204);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 13;
            pictureBox1.TabStop = false;
            // 
            // AuthorizationWin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            Name = "AuthorizationWin";
            Text = "OrderConfirmation";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label3;
        private Label label2;
        private TextBox textBox3;
        private TextBox textBox2;
        private Button button1;
        private PictureBox pictureBox1;
    }
}