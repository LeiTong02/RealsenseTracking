namespace WindowsFormsApp1
{
    partial class Form2
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
            this.example_picture = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_confim = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.example_picture)).BeginInit();
            this.SuspendLayout();
            // 
            // example_picture
            // 
            this.example_picture.Location = new System.Drawing.Point(7, 92);
            this.example_picture.Name = "example_picture";
            this.example_picture.Size = new System.Drawing.Size(287, 180);
            this.example_picture.TabIndex = 0;
            this.example_picture.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 83);
            this.label1.TabIndex = 1;
            this.label1.Text = "Next, Please follow the below example pciture to adjust body position and make su" +
    "re the human skeleton has been detected completely. ";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button_confim
            // 
            this.button_confim.Location = new System.Drawing.Point(91, 276);
            this.button_confim.Name = "button_confim";
            this.button_confim.Size = new System.Drawing.Size(126, 23);
            this.button_confim.TabIndex = 2;
            this.button_confim.Text = "OK";
            this.button_confim.UseVisualStyleBackColor = true;
            this.button_confim.Click += new System.EventHandler(this.button_confim_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 307);
            this.Controls.Add(this.button_confim);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.example_picture);
            this.Name = "Form2";
            this.Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)(this.example_picture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox example_picture;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_confim;
    }
}