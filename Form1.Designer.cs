namespace Tiny_Language
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
            input = new TextBox();
            Splite = new Button();
            table = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)table).BeginInit();
            SuspendLayout();
            // 
            // input
            // 
            input.Location = new Point(61, 55);
            input.Multiline = true;
            input.Name = "input";
            input.Size = new Size(565, 818);
            input.TabIndex = 0;
            // 
            // Splite
            // 
            Splite.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            Splite.Location = new Point(61, 971);
            Splite.Name = "Splite";
            Splite.Size = new Size(565, 58);
            Splite.TabIndex = 1;
            Splite.Text = "Split";
            Splite.UseVisualStyleBackColor = true;
            Splite.Click += Splite_Click;
            // 
            // table
            // 
            table.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            table.Location = new Point(722, 55);
            table.Name = "table";
            table.RowHeadersWidth = 82;
            table.Size = new Size(465, 974);
            table.TabIndex = 2;
            table.CellContentClick += dataGridView1_CellContentClick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1248, 1085);
            Controls.Add(table);
            Controls.Add(Splite);
            Controls.Add(input);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)table).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox input;
        private Button Splite;
        private DataGridView table;
    }
}
