namespace WindowsFormsApp1
{
    partial class Lissajous_Form
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
            this.clear_line_bt = new System.Windows.Forms.Button();
            this.right_radio = new System.Windows.Forms.RadioButton();
            this.left_radio = new System.Windows.Forms.RadioButton();
            this.button_save = new System.Windows.Forms.Button();
            this.calib_end = new System.Windows.Forms.Button();
            this.calib_start = new System.Windows.Forms.Button();
            this.panel_setting = new System.Windows.Forms.Panel();
            this.label_panel_name = new System.Windows.Forms.Label();
            this.panel_process = new System.Windows.Forms.Panel();
            this.back_button = new System.Windows.Forms.Button();
            this.button_setting_save = new System.Windows.Forms.Button();
            this.query_radio_button = new System.Windows.Forms.Label();
            this.front_or_side_box = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel_setting.SuspendLayout();
            this.panel_process.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.pictureBox1.Location = new System.Drawing.Point(0, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(848, 480);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // clear_line_bt
            // 
            this.clear_line_bt.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.clear_line_bt.Location = new System.Drawing.Point(12, 234);
            this.clear_line_bt.Name = "clear_line_bt";
            this.clear_line_bt.Size = new System.Drawing.Size(177, 36);
            this.clear_line_bt.TabIndex = 2;
            this.clear_line_bt.Text = "Clear Up";
            this.clear_line_bt.UseVisualStyleBackColor = true;
            this.clear_line_bt.Click += new System.EventHandler(this.clear_line_bt_Click);
            // 
            // right_radio
            // 
            this.right_radio.AutoSize = true;
            this.right_radio.Location = new System.Drawing.Point(105, 203);
            this.right_radio.Name = "right_radio";
            this.right_radio.Size = new System.Drawing.Size(50, 17);
            this.right_radio.TabIndex = 6;
            this.right_radio.TabStop = true;
            this.right_radio.Text = "Right";
            this.right_radio.UseVisualStyleBackColor = true;
            this.right_radio.CheckedChanged += new System.EventHandler(this.right_radio_CheckedChanged);
            // 
            // left_radio
            // 
            this.left_radio.AutoSize = true;
            this.left_radio.Location = new System.Drawing.Point(36, 203);
            this.left_radio.Name = "left_radio";
            this.left_radio.Size = new System.Drawing.Size(43, 17);
            this.left_radio.TabIndex = 5;
            this.left_radio.TabStop = true;
            this.left_radio.Text = "Left";
            this.left_radio.UseVisualStyleBackColor = true;
            this.left_radio.CheckedChanged += new System.EventHandler(this.left_radio_CheckedChanged);
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(12, 184);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(177, 38);
            this.button_save.TabIndex = 4;
            this.button_save.Text = "Save Results";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_process_Click);
            // 
            // calib_end
            // 
            this.calib_end.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.calib_end.Location = new System.Drawing.Point(12, 133);
            this.calib_end.Name = "calib_end";
            this.calib_end.Size = new System.Drawing.Size(177, 34);
            this.calib_end.TabIndex = 1;
            this.calib_end.Text = "Stop Recording";
            this.calib_end.UseVisualStyleBackColor = true;
            this.calib_end.Visible = false;
            this.calib_end.Click += new System.EventHandler(this.calib_end_Click);
            // 
            // calib_start
            // 
            this.calib_start.Location = new System.Drawing.Point(12, 133);
            this.calib_start.Name = "calib_start";
            this.calib_start.Size = new System.Drawing.Size(177, 33);
            this.calib_start.TabIndex = 10;
            this.calib_start.Text = "Start Recording";
            this.calib_start.UseVisualStyleBackColor = true;
            this.calib_start.Click += new System.EventHandler(this.calib_start_Click_1);
            // 
            // panel_setting
            // 
            this.panel_setting.Controls.Add(this.label_panel_name);
            this.panel_setting.Controls.Add(this.panel_process);
            this.panel_setting.Controls.Add(this.button_setting_save);
            this.panel_setting.Controls.Add(this.query_radio_button);
            this.panel_setting.Controls.Add(this.right_radio);
            this.panel_setting.Controls.Add(this.left_radio);
            this.panel_setting.Controls.Add(this.front_or_side_box);
            this.panel_setting.Location = new System.Drawing.Point(851, 3);
            this.panel_setting.Name = "panel_setting";
            this.panel_setting.Size = new System.Drawing.Size(204, 480);
            this.panel_setting.TabIndex = 11;
            // 
            // label_panel_name
            // 
            this.label_panel_name.AutoSize = true;
            this.label_panel_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_panel_name.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label_panel_name.Location = new System.Drawing.Point(42, 14);
            this.label_panel_name.Name = "label_panel_name";
            this.label_panel_name.Size = new System.Drawing.Size(112, 20);
            this.label_panel_name.TabIndex = 13;
            this.label_panel_name.Text = "Settings Panel";
            // 
            // panel_process
            // 
            this.panel_process.Controls.Add(this.back_button);
            this.panel_process.Controls.Add(this.button_save);
            this.panel_process.Controls.Add(this.calib_start);
            this.panel_process.Controls.Add(this.clear_line_bt);
            this.panel_process.Controls.Add(this.calib_end);
            this.panel_process.Location = new System.Drawing.Point(0, 0);
            this.panel_process.Name = "panel_process";
            this.panel_process.Size = new System.Drawing.Size(204, 480);
            this.panel_process.TabIndex = 12;
            // 
            // back_button
            // 
            this.back_button.Location = new System.Drawing.Point(12, 284);
            this.back_button.Name = "back_button";
            this.back_button.Size = new System.Drawing.Size(177, 35);
            this.back_button.TabIndex = 11;
            this.back_button.Text = "Back to Settings";
            this.back_button.UseVisualStyleBackColor = true;
            this.back_button.Click += new System.EventHandler(this.back_button_Click);
            // 
            // button_setting_save
            // 
            this.button_setting_save.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_setting_save.Location = new System.Drawing.Point(16, 252);
            this.button_setting_save.Name = "button_setting_save";
            this.button_setting_save.Size = new System.Drawing.Size(167, 35);
            this.button_setting_save.TabIndex = 11;
            this.button_setting_save.Text = "Save Settings";
            this.button_setting_save.UseVisualStyleBackColor = true;
            this.button_setting_save.Click += new System.EventHandler(this.button_setting_save_Click);
            // 
            // query_radio_button
            // 
            this.query_radio_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.query_radio_button.Location = new System.Drawing.Point(3, 158);
            this.query_radio_button.Name = "query_radio_button";
            this.query_radio_button.Size = new System.Drawing.Size(195, 42);
            this.query_radio_button.TabIndex = 10;
            this.query_radio_button.Text = "Please select meansurement body side.";
            // 
            // front_or_side_box
            // 
            this.front_or_side_box.FormattingEnabled = true;
            this.front_or_side_box.Items.AddRange(new object[] {
            "Coronal Plane",
            "Sagittal Plane",
            "Arm-axial Plane"});
            this.front_or_side_box.Location = new System.Drawing.Point(6, 106);
            this.front_or_side_box.Name = "front_or_side_box";
            this.front_or_side_box.Size = new System.Drawing.Size(192, 21);
            this.front_or_side_box.TabIndex = 9;
            this.front_or_side_box.Visible = false;
            // 
            // Lissajous_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 486);
            this.Controls.Add(this.panel_setting);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Name = "Lissajous_Form";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Lissajous Form";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel_setting.ResumeLayout(false);
            this.panel_setting.PerformLayout();
            this.panel_process.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button clear_line_bt;
        private System.Windows.Forms.RadioButton right_radio;
        private System.Windows.Forms.RadioButton left_radio;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button calib_end;
        private System.Windows.Forms.Button calib_start;
        private System.Windows.Forms.Panel panel_setting;
        private System.Windows.Forms.Label query_radio_button;
        private System.Windows.Forms.Button button_setting_save;
        private System.Windows.Forms.Panel panel_process;
        private System.Windows.Forms.Button back_button;
        private System.Windows.Forms.Label label_panel_name;
        private System.Windows.Forms.ComboBox front_or_side_box;
    }
}

