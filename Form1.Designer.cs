namespace Ark_Survival_Evolved_RCON_Player_Finder
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tab_page_config = new TabPage();
            richTextBox1 = new RichTextBox();
            textBox_set_name = new TextBox();
            label_set_name = new Label();
            button_connect = new Button();
            tabControl1 = new TabControl();
            tab_page_config.SuspendLayout();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // tab_page_config
            // 
            tab_page_config.Controls.Add(richTextBox1);
            tab_page_config.Controls.Add(textBox_set_name);
            tab_page_config.Controls.Add(label_set_name);
            tab_page_config.Controls.Add(button_connect);
            tab_page_config.Location = new Point(4, 24);
            tab_page_config.Margin = new Padding(3, 2, 3, 2);
            tab_page_config.Name = "tab_page_config";
            tab_page_config.Padding = new Padding(3, 2, 3, 2);
            tab_page_config.Size = new Size(1340, 635);
            tab_page_config.TabIndex = 0;
            tab_page_config.Text = "Config";
            tab_page_config.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(255, 4);
            richTextBox1.Margin = new Padding(3, 2, 3, 2);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(1081, 632);
            richTextBox1.TabIndex = 6;
            richTextBox1.Text = "";
            // 
            // textBox_set_name
            // 
            textBox_set_name.Location = new Point(124, 37);
            textBox_set_name.Margin = new Padding(3, 2, 3, 2);
            textBox_set_name.Name = "textBox_set_name";
            textBox_set_name.Size = new Size(110, 23);
            textBox_set_name.TabIndex = 4;
            // 
            // label_set_name
            // 
            label_set_name.AutoSize = true;
            label_set_name.Location = new Point(9, 39);
            label_set_name.Name = "label_set_name";
            label_set_name.Size = new Size(100, 15);
            label_set_name.TabIndex = 5;
            label_set_name.Text = "Your Player Name";
            // 
            // button_connect
            // 
            button_connect.Location = new Point(124, 70);
            button_connect.Margin = new Padding(3, 2, 3, 2);
            button_connect.Name = "button_connect";
            button_connect.Size = new Size(82, 22);
            button_connect.TabIndex = 0;
            button_connect.Text = "Connect";
            button_connect.UseVisualStyleBackColor = true;
            button_connect.Click += Button_connect_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tab_page_config);
            tabControl1.Location = new Point(10, 9);
            tabControl1.Margin = new Padding(3, 2, 3, 2);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1348, 663);
            tabControl1.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1008, 681);
            Controls.Add(tabControl1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "Ark Player Finder";
            tab_page_config.ResumeLayout(false);
            tab_page_config.PerformLayout();
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabPage tab_page_config;
        private TextBox textBox_set_name;
        private Label label_set_name;
        private Button button_connect;
        private TabControl tabControl1;
        private RichTextBox richTextBox1;
    }
}