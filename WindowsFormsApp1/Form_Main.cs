using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace mycontracts
{
    public partial class Form_Main : Form
    {
        public Form_Main()
        {
            InitializeComponent();
            initcontracts(); 
        }
        void initcontracts()
            {
                if (System.IO.File.Exists("C://Users//hpsin//patient.xml"))
                {
                    dataGridView1 .DataSource =studentinfoBLL .getallstudentinfo ();
                }
                else 
                {
                    studentinfoBLL .CreateStudentXml ();
                    dataGridView1 .DataSource =studentinfoBLL .getallstudentinfo ();
                }
            dataGridView1 .Columns [0].HeaderText="Patient ID";
            dataGridView1 .Columns [1].HeaderText="Name";
            dataGridView1 .Columns [2].HeaderText="Sex";
            dataGridView1 .Columns [3].HeaderText="Age";
            dataGridView1 .Columns [4].HeaderText="Date of Birth";
            dataGridView1 .Columns [5].HeaderText="Tel";
            dataGridView1 .Columns [6].HeaderText="Home address";
            dataGridView1 .Columns [7].HeaderText="Description";
            dataGridView1 .Columns [8].HeaderText="Data Saved";   
                }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void toolstrip_add_Click(object sender, EventArgs e)
        {
            Form_add formadd = new Form_add();
            formadd.ShowDialog();

            

            initcontracts();
        }

        private void toolStrip_search_Click(object sender, EventArgs e)
        {
            Form_search formsearch = new Form_search();
            formsearch.ShowDialog();
        }

        private void toolStrip_delete_Click(object sender, EventArgs e)
        {
         if (dataGridView1.SelectedRows.Count == 1)
         {
             if (MessageBox.Show("Do you want to delete this patient information？", "Yes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
             MessageBoxDefaultButton.Button2) == DialogResult.Yes)
             {
                 int selectrow = Int32.Parse(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0]
                     .Value.ToString());
                 if (studentinfoBLL.Deletestudentinfo(selectrow))
                     MessageBox.Show("Delete successfully！");
                 else
                     MessageBox.Show("Delete failed, please make sure selected one row！");
                 initcontracts();
             }             
          }
          else
                 MessageBox.Show("Please select one row then click delete!");
        }

        private void toolStrip_edit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                int selectrow = Int32.Parse(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex]
                    .Cells[0].Value.ToString());
                Form_edit formedit = new Form_edit();
                formedit.studentid_edit = selectrow;
                formedit.ShowDialog();
                initcontracts();
            }
            else
                MessageBox.Show("Please select one row then click edit！");
        }

        

        private void toolStripMenuItem_views_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count == 1)
            {
                int selectrow = Int32.Parse(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex]
                    .Cells[0].Value.ToString());
                studentinfo studentinfo = studentinfoBLL.getstudentinfo(selectrow);
                //this.Hide();
                Form1 form1 = new Form1();
                form1.passed_save_dir = studentinfo.Profession;
                //form1.Closed += (s, args) => this.Close();
                form1.Show();
                
                
                
                
                
               
            }
            else
                MessageBox.Show("Please select one row！");

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count == 1)
            {
                int selectrow = Int32.Parse(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex]
                    .Cells[0].Value.ToString());
                studentinfo studentinfo = studentinfoBLL.getstudentinfo(selectrow);
                Lissajous_Form form1 = new Lissajous_Form();
                form1.passed_save_dir = studentinfo.Profession;
                //form1.Closed += (s, args) => this.Close();
                form1.Show();

                // Process.Start(studentinfo.Profession);
            }
            else
                MessageBox.Show("Please select one row！");

        }

        private void toolStripButton_openDir_Click(object sender, EventArgs e)
        {
            try
            {


                if (dataGridView1.SelectedRows.Count == 1)
                {
                    int selectrow = Int32.Parse(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex]
                        .Cells[0].Value.ToString());
                    studentinfo studentinfo = studentinfoBLL.getstudentinfo(selectrow);


                    Process.Start(studentinfo.Profession);
                }
                else
                    MessageBox.Show("Please select one row！");
            }
            catch (Exception ex) {
                MessageBox.Show("Target Folder has been deleted");
            }
            
        }
    }
}
