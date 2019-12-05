using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace mycontracts
{

    
    public partial class Form_add : Form
    {

        private static string _basePath = "C://Users//hpsin//patient.xml";
        private string saved_path = "";
        public Form_add()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void btn_add_Click(object sender, EventArgs e)
        {

            studentinfo studentinfo = new studentinfo();
            try
            {

                int Input_ID = Int32.Parse(txt_studengid.Text);
                XElement xml = XElement.Load(_basePath);
                var studentvar = xml.Descendants("student");
                if (Input_ID != 0)
                {
                    studentvar = xml.Descendants("student").Where(a => a.Attribute("studentid").Value
                        == Input_ID.ToString());
                }
                if (studentvar.ToList().Count > 0)
                {
                    throw new System.ArgumentException("Id is unique", "original");
                }
                else
                {
                    studentinfo.studentid = Int32.Parse(txt_studengid.Text);
                }




                studentinfo.Name = txt_name.Text;
                if (rb_man.Checked)
                    studentinfo.sex = "Man";
                else if (rb_woman.Checked)
                    studentinfo.sex = "Woman";

                studentinfo.Age = Int32.Parse(txt_age.Text);
                studentinfo.Birthdate = DateTime.Parse(dt_birthdate.Text);
                studentinfo.Phone = txt_phone.Text;
                studentinfo.Email = txt_email.Text;
                studentinfo.Homeaddress = txt_homeaddress.Text;
                saved_path = "C://Users//hpsin//Desktop//Data//"+ studentinfo.studentid+"_"+studentinfo.Name;
                //if (txt_profession.Text == "")
                //{

                //    throw new Exception();
                //}
                //else {
                    studentinfo.Profession = saved_path;
                //}


                System.IO.Directory.CreateDirectory(saved_path);



                if (studentinfoBLL.Addstudentinfo(studentinfo))
                { MessageBox.Show("Add successfully, data will be saved in: "+saved_path); }



            }

            catch (System.ArgumentException ae) {
                MessageBox.Show("Patient ID has been registered!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            catch (FormatException format) {
                MessageBox.Show("Patient ID or age should be a number!");
            }
            catch (Exception exception)
            {

                MessageBox.Show("Please complete all items!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }
        






        private void bt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_path_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            

            //saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Png Image|*.png";
            saveFileDialog1.Title = "Save Folder Path";
            
            
           // saveFileDialog1.ShowDialog();
            String filename = "";
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                // If the file name is not an empty string open it for saving.  
                if (saveFileDialog1.FileName != "")
            
                filename = saveFileDialog1.FileName;
                
 
            
            saved_path = filename;
            txt_profession.Text = saved_path;
        }
    }
}
