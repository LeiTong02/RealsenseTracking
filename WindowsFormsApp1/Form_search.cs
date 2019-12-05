using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mycontracts
{
    public partial class Form_search : Form
    {
        public Form_search()
        {
            InitializeComponent();
        }
        void initheadtitle()
        {
            dataGridView1.Columns[0].HeaderText = "Patient ID";
            dataGridView1.Columns[1].HeaderText = "Name";
            dataGridView1.Columns[2].HeaderText = "Sex";
            dataGridView1.Columns[3].HeaderText = "Age";
            dataGridView1.Columns[4].HeaderText = "Date of Birth";
            dataGridView1.Columns[5].HeaderText = "Tel";
            dataGridView1.Columns[6].HeaderText = "Home address";
            dataGridView1.Columns[7].HeaderText = "Description";
            dataGridView1.Columns[8].HeaderText = "Data Saved";
        }

        private void cb_search_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txt_searchtxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (cb_search.Text == string.Empty)
            {
                dataGridView1.DataSource = studentinfoBLL.getallstudentinfo();
                initheadtitle();
            }
            else
            {
                if (txt_searchtxt.Text != string.Empty)
                {
                    studentinfo studentsearch = new studentinfo();
                    switch (cb_search.SelectedIndex)
                    {
                        case 0: studentsearch.studentid = Int32.Parse(txt_searchtxt.Text); break;
                        case 1: studentsearch.Name = txt_searchtxt.Text; break;
                    }
                    dataGridView1.DataSource = studentinfoBLL.getstudentinfolist(studentsearch);
                    initheadtitle();
                }
                else MessageBox.Show("Please Input details" + cb_search.Text);
            }
        }

        private void bt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
