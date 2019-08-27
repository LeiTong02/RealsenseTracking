using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public string pic_path;
        public Form2(string pic_path)
        {
            InitializeComponent();
            this.pic_path = pic_path;
            Bitmap original = (Bitmap)Image.FromFile(pic_path);
            Bitmap resized = new Bitmap(original, new Size(example_picture.Width, example_picture.Height));
            example_picture.Image = resized;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button_confim_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
