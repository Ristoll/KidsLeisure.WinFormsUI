namespace KidsLeisure.UI
{
    partial class KidsLeisureWin
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
            components = new System.ComponentModel.Container();
            menuStrip1 = new MenuStrip();
            mainToolStripMenuItem = new ToolStripMenuItem();
            zonesToolStripMenuItem = new ToolStripMenuItem();
            атракціониToolStripMenuItem = new ToolStripMenuItem();
            аніматориToolStripMenuItem = new ToolStripMenuItem();
            програмиToolStripMenuItem = new ToolStripMenuItem();
            деньНародженняToolStripMenuItem = new ToolStripMenuItem();
            кошикToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1 = new ContextMenuStrip(components);
            listBox1 = new ListBox();
            panel1 = new Panel();
            label4 = new Label();
            label3 = new Label();
            panel2 = new Panel();
            label2 = new Label();
            label1 = new Label();
            button3 = new Button();
            button2 = new Button();
            button1 = new Button();
            listBox2 = new ListBox();
            menuStrip1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { mainToolStripMenuItem, zonesToolStripMenuItem, атракціониToolStripMenuItem, аніматориToolStripMenuItem, програмиToolStripMenuItem, кошикToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            menuStrip1.ItemClicked += menuStrip1_ItemClicked;
            // 
            // mainToolStripMenuItem
            // 
            mainToolStripMenuItem.Name = "mainToolStripMenuItem";
            mainToolStripMenuItem.Size = new Size(81, 24);
            mainToolStripMenuItem.Text = "Головна";
            // 
            // zonesToolStripMenuItem
            // 
            zonesToolStripMenuItem.Name = "zonesToolStripMenuItem";
            zonesToolStripMenuItem.Size = new Size(58, 24);
            zonesToolStripMenuItem.Text = "Зони";
            // 
            // атракціониToolStripMenuItem
            // 
            атракціониToolStripMenuItem.Name = "атракціониToolStripMenuItem";
            атракціониToolStripMenuItem.Size = new Size(103, 24);
            атракціониToolStripMenuItem.Text = "Атракціони";
            // 
            // аніматориToolStripMenuItem
            // 
            аніматориToolStripMenuItem.Name = "аніматориToolStripMenuItem";
            аніматориToolStripMenuItem.Size = new Size(98, 24);
            аніматориToolStripMenuItem.Text = "Аніматори";
            // 
            // програмиToolStripMenuItem
            // 
            програмиToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { деньНародженняToolStripMenuItem });
            програмиToolStripMenuItem.Name = "програмиToolStripMenuItem";
            програмиToolStripMenuItem.Size = new Size(95, 24);
            програмиToolStripMenuItem.Text = "Програми";
            // 
            // деньНародженняToolStripMenuItem
            // 
            деньНародженняToolStripMenuItem.Name = "деньНародженняToolStripMenuItem";
            деньНародженняToolStripMenuItem.Size = new Size(221, 26);
            деньНародженняToolStripMenuItem.Text = "День Народження";
            деньНародженняToolStripMenuItem.Click += деньНародженняToolStripMenuItem_Click;
            // 
            // кошикToolStripMenuItem
            // 
            кошикToolStripMenuItem.Name = "кошикToolStripMenuItem";
            кошикToolStripMenuItem.Size = new Size(69, 24);
            кошикToolStripMenuItem.Text = "Кошик";
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(50, 52);
            listBox1.Margin = new Padding(3, 4, 3, 4);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(212, 344);
            listBox1.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Bottom;
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Location = new Point(0, 518);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 49);
            panel1.TabIndex = 3;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(611, 14);
            label4.Name = "label4";
            label4.Size = new Size(177, 20);
            label4.TabIndex = 1;
            label4.Text = "Контакти: +00000000000";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 14);
            label3.Name = "label3";
            label3.Size = new Size(225, 20);
            label3.TabIndex = 0;
            label3.Text = "Країна, Місто, вул. Вулиця,  №0";
            // 
            // panel2
            // 
            panel2.Controls.Add(label2);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(button3);
            panel2.Controls.Add(button2);
            panel2.Controls.Add(button1);
            panel2.Controls.Add(listBox2);
            panel2.Controls.Add(listBox1);
            panel2.Location = new Point(12, 55);
            panel2.Margin = new Padding(3, 4, 3, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(776, 455);
            panel2.TabIndex = 4;
            panel2.Paint += panel2_Paint;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(587, 29);
            label2.Name = "label2";
            label2.Size = new Size(66, 20);
            label2.TabIndex = 8;
            label2.Text = "Вибрані";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(116, 28);
            label1.Name = "label1";
            label1.Size = new Size(70, 20);
            label1.TabIndex = 7;
            label1.Text = "Доступні";
            // 
            // button3
            // 
            button3.Location = new Point(346, 348);
            button3.Margin = new Padding(3, 4, 3, 4);
            button3.Name = "button3";
            button3.Size = new Size(88, 50);
            button3.TabIndex = 6;
            button3.Text = "Додати в кошик";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.Location = new Point(398, 141);
            button2.Margin = new Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new Size(87, 29);
            button2.TabIndex = 5;
            button2.Text = "Видалити";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(289, 141);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(75, 29);
            button1.TabIndex = 4;
            button1.Text = "Додати";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // listBox2
            // 
            listBox2.FormattingEnabled = true;
            listBox2.Location = new Point(512, 52);
            listBox2.Margin = new Padding(3, 4, 3, 4);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(212, 344);
            listBox2.TabIndex = 3;
            // 
            // KidsLeisureWin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 562);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 4, 3, 4);
            Name = "KidsLeisureWin";
            Text = "KidsLeasure";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zonesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem атракціониToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem аніматориToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem кошикToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripMenuItem програмиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem деньНародженняToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}

