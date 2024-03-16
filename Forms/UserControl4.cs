using CRUD_Operations;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MidProject.Forms
{
    public partial class UserControl4 : UserControl
    {
        private int AssessmentID = 0;
        private const string searchBoxText = "Search";
        private const string title = "Enter Title";
        private const string marks = "Enter Total Marks";
        private const string weightage = "Enter Weightage";
        public UserControl4()
        {
            InitializeComponent();
        }

        private void UserControl4_Load(object sender, EventArgs e)
        {
            load();
            textBox1.Text = title;
            textBox2.Text = marks;
            textBox3.Text = weightage;
            searchBox.Text = searchBoxText;
        }
        private void load()
        {
            var con = Configuration.getInstance().getConnection();
            string query = "Select * from Assessment Where Title Not Like '%#Del%' ";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void searchBox_Enter(object sender, EventArgs e)
        {
            if (searchBox.Text == searchBoxText)
            {
                searchBox.Text = "";
            }
        }

        private void searchBox_Leave(object sender, EventArgs e)
        {
            if (searchBox.Text == "")
            {
                searchBox.Text = searchBoxText;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = title;
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == title)
            {
                textBox1.Text = "";
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = marks;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == marks)
            {
                textBox2.Text = "";
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                textBox3.Text = weightage;
            }
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (textBox3.Text == weightage)
            {
                textBox3.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == title || textBox2.Text == marks || textBox3.Text == weightage)
            {
                MessageBox.Show(Messages.NOT_EMPTY);
                return;
            }
            var con = Configuration.getInstance().getConnection();
            var myDateTime = DateTime.Now;
            var sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string query = "Insert Into Assessment(Title,DateCreated,TotalMarks,TotalWeightage) " + "Values('" + textBox1.Text + "', '" + sqlFormattedDate + "', " + textBox2.Text + ", " + textBox3.Text + ")";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            load();
            MessageBox.Show(Messages.SUCCESS_INSERT);
            textBox1.Text = title;
            textBox2.Text = marks;
            textBox3.Text = weightage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == title || textBox2.Text == marks || textBox3.Text == weightage)
            {
                MessageBox.Show(Messages.NOT_EMPTY);
                return;
            }
            var con = Configuration.getInstance().getConnection();
            string query = "Update Assessment Set Title = '" + textBox1.Text + "', TotalMarks = " + textBox2.Text + ", TotalWeightage = " + textBox3.Text + " Where Id = " + AssessmentID;
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            load();
            MessageBox.Show(Messages.SUCCESS_UPDATE);
            textBox1.Text = title;
            textBox2.Text = marks;
            textBox3.Text = weightage;
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            string query = "Update Assessment Set Title = Title + '#Del' Where Id = " + AssessmentID;
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            load();
            MessageBox.Show(Messages.SUCCESS_DELETE);
            textBox1.Text = title;
            textBox2.Text = marks;
            textBox3.Text = weightage;
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            if (searchBox.Text == searchBoxText)
            {
                return;
            }
            var con = Configuration.getInstance().getConnection();
            string query = "Select * from Assessment Where Title Like '%" + searchBox.Text + "%' And Title Not Like '%#Del%' ";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex>=0)
            {
                AssessmentID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }
    }
}
