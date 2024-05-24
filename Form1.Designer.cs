namespace WinFormsApp1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            Var_Num = new NumericUpDown();
            Cons_Num = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            button1 = new Button();
            Z_X = new Label();
            MAXorMIN = new ComboBox();
            ZeroEquationPanel = new FlowLayoutPanel();
            EquationsPanel = new FlowLayoutPanel();
            button2 = new Button();
            button3 = new Button();
            label3 = new Label();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)Var_Num).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Cons_Num).BeginInit();
            SuspendLayout();
            // 
            // Var_Num
            // 
            Var_Num.BackColor = Color.Snow;
            Var_Num.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Var_Num.Location = new Point(217, 12);
            Var_Num.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            Var_Num.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            Var_Num.Name = "Var_Num";
            Var_Num.Size = new Size(70, 29);
            Var_Num.TabIndex = 0;
            Var_Num.TextAlign = HorizontalAlignment.Center;
            Var_Num.Value = new decimal(new int[] { 2, 0, 0, 0 });
            Var_Num.ValueChanged += Var_Num_ValueChanged;
            // 
            // Cons_Num
            // 
            Cons_Num.BackColor = Color.Snow;
            Cons_Num.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Cons_Num.Location = new Point(526, 12);
            Cons_Num.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            Cons_Num.Name = "Cons_Num";
            Cons_Num.Size = new Size(70, 29);
            Cons_Num.TabIndex = 1;
            Cons_Num.TextAlign = HorizontalAlignment.Center;
            Cons_Num.Value = new decimal(new int[] { 2, 0, 0, 0 });
            Cons_Num.ValueChanged += Cons_Num_ValueChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(36, 17);
            label1.Name = "label1";
            label1.Size = new Size(171, 21);
            label1.TabIndex = 2;
            label1.Text = "Number of Variables:";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label2.ForeColor = Color.Black;
            label2.Location = new Point(336, 15);
            label2.Name = "label2";
            label2.Size = new Size(181, 21);
            label2.TabIndex = 3;
            label2.Text = "Number of Constrains:";
            label2.Click += label2_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.DodgerBlue;
            button1.FlatStyle = FlatStyle.Popup;
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            button1.ForeColor = Color.White;
            button1.ImageAlign = ContentAlignment.MiddleRight;
            button1.Location = new Point(622, 12);
            button1.Name = "button1";
            button1.Size = new Size(120, 29);
            button1.TabIndex = 4;
            button1.Text = "GENERATE";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // Z_X
            // 
            Z_X.AutoSize = true;
            Z_X.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            Z_X.Location = new Point(83, 80);
            Z_X.Name = "Z_X";
            Z_X.Size = new Size(57, 21);
            Z_X.TabIndex = 5;
            Z_X.Text = "Z(X) =";
            Z_X.Visible = false;
            Z_X.Click += label3_Click;
            // 
            // MAXorMIN
            // 
            MAXorMIN.BackColor = Color.Snow;
            MAXorMIN.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            MAXorMIN.FormattingEnabled = true;
            MAXorMIN.Items.AddRange(new object[] { "MAX", "MIN" });
            MAXorMIN.Location = new Point(12, 77);
            MAXorMIN.Name = "MAXorMIN";
            MAXorMIN.Size = new Size(65, 29);
            MAXorMIN.TabIndex = 6;
            MAXorMIN.Text = "MAX";
            MAXorMIN.Visible = false;
            // 
            // ZeroEquationPanel
            // 
            ZeroEquationPanel.AutoScroll = true;
            ZeroEquationPanel.AutoScrollMinSize = new Size(10000, 0);
            ZeroEquationPanel.Location = new Point(146, 77);
            ZeroEquationPanel.Name = "ZeroEquationPanel";
            ZeroEquationPanel.Size = new Size(596, 52);
            ZeroEquationPanel.TabIndex = 7;
            ZeroEquationPanel.Visible = false;
            ZeroEquationPanel.Paint += ZeroEquationPanel_Paint;
            // 
            // EquationsPanel
            // 
            EquationsPanel.Location = new Point(11, 161);
            EquationsPanel.Name = "EquationsPanel";
            EquationsPanel.Size = new Size(731, 225);
            EquationsPanel.TabIndex = 8;
            // 
            // button2
            // 
            button2.BackColor = Color.Green;
            button2.FlatAppearance.BorderColor = Color.FromArgb(0, 120, 74);
            button2.FlatAppearance.BorderSize = 2;
            button2.FlatStyle = FlatStyle.Popup;
            button2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            button2.ForeColor = Color.White;
            button2.ImageAlign = ContentAlignment.MiddleRight;
            button2.Location = new Point(250, 392);
            button2.Name = "button2";
            button2.Size = new Size(120, 50);
            button2.TabIndex = 9;
            button2.Text = "SOLVE";
            button2.UseVisualStyleBackColor = false;
            button2.Visible = false;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.Red;
            button3.FlatStyle = FlatStyle.Popup;
            button3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            button3.ForeColor = Color.White;
            button3.Location = new Point(376, 392);
            button3.Name = "button3";
            button3.Size = new Size(120, 50);
            button3.TabIndex = 10;
            button3.Text = "CLEAR";
            button3.UseVisualStyleBackColor = false;
            button3.Visible = false;
            button3.Click += button3_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label3.ForeColor = Color.FromArgb(192, 0, 0);
            label3.Location = new Point(8, 48);
            label3.Name = "label3";
            label3.Size = new Size(224, 21);
            label3.TabIndex = 11;
            label3.Text = "Linear Programming Model:";
            label3.Visible = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label4.ForeColor = Color.Black;
            label4.Location = new Point(12, 137);
            label4.Name = "label4";
            label4.Size = new Size(210, 21);
            label4.TabIndex = 12;
            label4.Text = "Subject to the Constraints:";
            label4.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(749, 451);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(EquationsPanel);
            Controls.Add(ZeroEquationPanel);
            Controls.Add(MAXorMIN);
            Controls.Add(Z_X);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(Cons_Num);
            Controls.Add(Var_Num);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            Text = "Simplex Calculator";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)Var_Num).EndInit();
            ((System.ComponentModel.ISupportInitialize)Cons_Num).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NumericUpDown Var_Num;
        private NumericUpDown Cons_Num;
        private Label label1;
        private Label label2;
        private Button button1;
        private Label Z_X;
        private ComboBox MAXorMIN;
        private FlowLayoutPanel ZeroEquationPanel;
        private FlowLayoutPanel EquationsPanel;
        private Button button2;
        private Button button3;
        private Label label3;
        private Label label4;
    }
}