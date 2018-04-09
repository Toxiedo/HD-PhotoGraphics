namespace HD_PhotoGraphics
{
    partial class Gauss
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.segma = new System.Windows.Forms.Label();
            this.segma_text_box = new System.Windows.Forms.TextBox();
            this.Gaussian_Filter = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(505, 432);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // segma
            // 
            this.segma.AutoSize = true;
            this.segma.Location = new System.Drawing.Point(520, 57);
            this.segma.Name = "segma";
            this.segma.Size = new System.Drawing.Size(38, 13);
            this.segma.TabIndex = 20;
            this.segma.Text = "segma";
            // 
            // segma_text_box
            // 
            this.segma_text_box.Location = new System.Drawing.Point(611, 54);
            this.segma_text_box.Name = "segma_text_box";
            this.segma_text_box.Size = new System.Drawing.Size(100, 20);
            this.segma_text_box.TabIndex = 19;
            // 
            // Gaussian_Filter
            // 
            this.Gaussian_Filter.Location = new System.Drawing.Point(523, 12);
            this.Gaussian_Filter.Name = "Gaussian_Filter";
            this.Gaussian_Filter.Size = new System.Drawing.Size(227, 26);
            this.Gaussian_Filter.TabIndex = 18;
            this.Gaussian_Filter.Text = "Gaussian Filter";
            this.Gaussian_Filter.UseVisualStyleBackColor = true;
            this.Gaussian_Filter.Click += new System.EventHandler(this.Gaussian_Filter_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(756, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(505, 432);
            this.pictureBox2.TabIndex = 21;
            this.pictureBox2.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(589, 421);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 22;
            this.button1.Text = "Back";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Gauss
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1271, 457);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.segma);
            this.Controls.Add(this.segma_text_box);
            this.Controls.Add(this.Gaussian_Filter);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Gauss";
            this.Text = "Gauss";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label segma;
        private System.Windows.Forms.TextBox segma_text_box;
        private System.Windows.Forms.Button Gaussian_Filter;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button1;
    }
}