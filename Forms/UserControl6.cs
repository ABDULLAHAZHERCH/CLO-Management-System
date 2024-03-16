using CRUD_Operations;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MidProject.Forms
{
    public partial class UserControl6 : UserControl
    {
        private int AssessmentComponentID = 0;
        private const string searchbox = "Search";
        private const string namebox = "Enter Name";
        private const string marksbox = "Enter Total Marks";
        private const string rubricbox = "Select Rubric";
        private const string assessmentbox = "Select Assessment";
        public UserControl6()
        {
            InitializeComponent();
        }

        private void UserControl6_Load(object sender, EventArgs e)
        {
            load();
            combo1Load();
            combo2Load();
            searchBox.Text = searchbox;
            textBox1.Text = namebox;
            textBox2.Text = marksbox;
            comboBox1.Text = rubricbox;
            comboBox2.Text = assessmentbox;
        }

        private void load()
        {
            var con = Configuration.getInstance().getConnection();
            string query = "Select * from AssessmentComponent Where Name Not Like '%#Del%' ";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void combo1Load()
        {
            var con = Configuration.getInstance().getConnection();
            string query = "Select * from Rubric Where Details Not Like '%#Del%' ";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader myreader;
            myreader = cmd.ExecuteReader();
            while (myreader.Read())
            {
                string sName = myreader.GetString(1);
                comboBox1.Items.Add(sName);
            }
            myreader.Close();
        }

        private void combo2Load()
        {
            var con = Configuration.getInstance().getConnection();
            string query = "Select * from Assessment Where Title Not Like '%#Del%' ";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader myreader;
            myreader = cmd.ExecuteReader();
            while (myreader.Read())
            {
                comboBox2.Items.Add(myreader["Title"].ToString());
            }
            myreader.Close();
        }

        private void searchBox_Leave(object sender, EventArgs e)
        {
            if (searchBox.Text == "")
            {
                searchBox.Text = searchbox;
            }
        }

        private void searchBox_Enter(object sender, EventArgs e)
        {
            if (searchBox.Text == searchbox)
            {
                searchBox.Text = "";
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == marksbox)
            {
                textBox2.Text = "";
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = marksbox;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = namebox;
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == namebox)
            {
                textBox1.Text = "";
            }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                comboBox1.Text = rubricbox;
            }
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            if (comboBox1.Text == rubricbox)
            {
                comboBox1.Text = "";
            }
        }

        private void comboBox2_Enter(object sender, EventArgs e)
        {
            if (comboBox2.Text == assessmentbox)
            {
                comboBox2.Text = "";
            }
        }

        private void comboBox2_Leave(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                comboBox2.Text = assessmentbox;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                AssessmentComponentID = Convert.ToInt32(row.Cells[0].Value);
                textBox1.Text = row.Cells[1].Value.ToString();
                textBox2.Text = row.Cells[3].Value.ToString();
                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == namebox || textBox2.Text == marksbox || comboBox1.Text == rubricbox || comboBox2.Text == assessmentbox)
            {
                MessageBox.Show(Messages.NOT_EMPTY);
                return;
            }
            var myDateTime = DateTime.Now;
            var sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            var con = Configuration.getInstance().getConnection();
            string query = "Insert into AssessmentComponent (Name,TotalMarks,RubricId,DateCreated,DateUpdated,AssessmentId) values ('" + textBox1.Text + "','" + textBox2.Text + "',(Select Id from Rubric Where Details = '" + comboBox1.Text + "'),'" + sqlFormattedDate + "','" + sqlFormattedDate + "',(Select Id from Assessment Where Title = '" + comboBox2.Text + "'))";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            load();
            MessageBox.Show(Messages.SUCCESS_INSERT);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == namebox || textBox2.Text == marksbox || comboBox1.Text == rubricbox || comboBox2.Text == assessmentbox)
            {
                MessageBox.Show(Messages.NOT_EMPTY);
                return;
            }
            var myDateTime = DateTime.Now;
            var sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            var con = Configuration.getInstance().getConnection();
            string query = "Update AssessmentComponent Set Name = '" + textBox1.Text + "', TotalMarks = '" + textBox2.Text + "', RubricId = (Select Id from Rubric Where Details = '" + comboBox1.Text + "'), DateUpdated = '" + sqlFormattedDate + "', AssessmentId = (Select Id from Assessment Where Title = '" + comboBox2.Text + "') Where Id = '" + AssessmentComponentID + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            load();
            MessageBox.Show(Messages.SUCCESS_UPDATE);
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            string query = "Update AssessmentComponent Set Name = '" + textBox1.Text + "#Del', TotalMarks = '" + textBox2.Text + "', RubricId = (Select Id from Rubric Where Details = '" + comboBox1.Text + "'), AssessmentId = (Select Id from Assessment Where Title = '" + comboBox2.Text + "') Where Id = '" + AssessmentComponentID + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            load();
            MessageBox.Show(Messages.SUCCESS_DELETE);
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            if (searchBox.Text == searchbox)
            {
                return;
            }
            var con = Configuration.getInstance().getConnection();
            string query = "Select * from AssessmentComponent Where Name Like '%" + searchBox.Text + "%' ";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
