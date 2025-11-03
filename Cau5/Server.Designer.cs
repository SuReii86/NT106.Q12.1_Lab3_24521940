namespace Cau5
{
    partial class Server
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
            this.btn_RESET = new System.Windows.Forms.Button();
            this.btn_EXIT = new System.Windows.Forms.Button();
            this.btn_delete = new System.Windows.Forms.Button();
            this.lw_list = new System.Windows.Forms.ListView();
            this.MonAn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IDNCC = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IDMA = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.QuyenHan = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_RESET
            // 
            this.btn_RESET.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_RESET.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btn_RESET.Font = new System.Drawing.Font("Cascadia Code SemiLight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_RESET.Location = new System.Drawing.Point(317, 478);
            this.btn_RESET.Name = "btn_RESET";
            this.btn_RESET.Size = new System.Drawing.Size(150, 50);
            this.btn_RESET.TabIndex = 14;
            this.btn_RESET.Text = "Reset";
            this.btn_RESET.UseVisualStyleBackColor = true;
            this.btn_RESET.Click += new System.EventHandler(this.btn_RESET_Click);
            // 
            // btn_EXIT
            // 
            this.btn_EXIT.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_EXIT.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btn_EXIT.Font = new System.Drawing.Font("Cascadia Code SemiLight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_EXIT.Location = new System.Drawing.Point(673, 478);
            this.btn_EXIT.Name = "btn_EXIT";
            this.btn_EXIT.Size = new System.Drawing.Size(150, 50);
            this.btn_EXIT.TabIndex = 13;
            this.btn_EXIT.Text = "Thoát";
            this.btn_EXIT.UseVisualStyleBackColor = true;
            this.btn_EXIT.Click += new System.EventHandler(this.btn_EXIT_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_delete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btn_delete.Font = new System.Drawing.Font("Cascadia Code SemiLight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_delete.Location = new System.Drawing.Point(808, 644);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(150, 50);
            this.btn_delete.TabIndex = 12;
            this.btn_delete.Text = "Xóa";
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_RAND_Click);
            // 
            // lw_list
            // 
            this.lw_list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MonAn,
            this.IDNCC,
            this.IDMA,
            this.QuyenHan});
            this.lw_list.Font = new System.Drawing.Font("Cascadia Code SemiLight", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lw_list.HideSelection = false;
            this.lw_list.Location = new System.Drawing.Point(86, 60);
            this.lw_list.Name = "lw_list";
            this.lw_list.Size = new System.Drawing.Size(1000, 400);
            this.lw_list.TabIndex = 18;
            this.lw_list.UseCompatibleStateImageBehavior = false;
            this.lw_list.View = System.Windows.Forms.View.Details;
            // 
            // MonAn
            // 
            this.MonAn.DisplayIndex = 1;
            this.MonAn.Text = "       Món Ăn";
            this.MonAn.Width = 250;
            // 
            // IDNCC
            // 
            this.IDNCC.DisplayIndex = 2;
            this.IDNCC.Text = "ID NCC";
            this.IDNCC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.IDNCC.Width = 250;
            // 
            // IDMA
            // 
            this.IDMA.DisplayIndex = 0;
            this.IDMA.Text = "ID Món Ăn";
            this.IDMA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.IDMA.Width = 250;
            // 
            // QuyenHan
            // 
            this.QuyenHan.Text = "Quyền Hạn";
            this.QuyenHan.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.QuyenHan.Width = 250;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Cascadia Code SemiLight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(424, 653);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(326, 35);
            this.textBox1.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cascadia Code SemiBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(264, 653);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 32);
            this.label1.TabIndex = 20;
            this.label1.Text = "ID Món Ăn:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Cascadia Code SemiBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(481, 594);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 32);
            this.label2.TabIndex = 21;
            this.label2.Text = "XÓA MÓN ĂN";
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(1178, 844);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lw_list);
            this.Controls.Add(this.btn_RESET);
            this.Controls.Add(this.btn_EXIT);
            this.Controls.Add(this.btn_delete);
            this.Name = "Server";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_RESET;
        private System.Windows.Forms.Button btn_EXIT;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.ListView lw_list;
        private System.Windows.Forms.ColumnHeader MonAn;
        private System.Windows.Forms.ColumnHeader IDNCC;
        private System.Windows.Forms.ColumnHeader QuyenHan;
        private System.Windows.Forms.ColumnHeader IDMA;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

