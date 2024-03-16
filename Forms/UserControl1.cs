using CRUD_Operations;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MidProject.Forms
{
    public partial class UserControl1 : UserControl
    {
        private int StdID = 0;
        private const string searchBoxText = "Search";
        private const string fnameBoxText = "Enter First Name";
        private const string lnameBoxText = "Enter Last Name";
        private const string contactBoxText = "Enter Contact";
        private const string emailBoxText = "Enter Email";
        private const string regnoBoxText = "Enter Registration Number";
        public UserControl1()
        {
            InitializeComponent();

        }
        private void UserControl1_Load(object sender, EventArgs e)
        {
            load();
            myBoxes();
        }
        private void myBoxes()
        {
            searchBox.Text = searchBoxText;
            textBox2.Text = fnameBoxText;
            textBox3.Text = lnameBoxText;
            textBox4.Text = contactBoxText;
            textBox5.Text = emailBoxText;
            textBox6.Text = regnoBoxText;
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
        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == fnameBoxText)
            {
                textBox2.Text = "";
            }
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = fnameBoxText;
            }
        }
        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (textBox3.Text == lnameBoxText)
            {
                textBox3.Text = "";
            }
        }
        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                textBox3.Text = lnameBoxText;
            }
        }
        private void textBox4_Enter(object sender, EventArgs e)
        {
            if (textBox4.Text == contactBoxText)
            {
                textBox4.Text = "";
            }
        }
        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
            {
                textBox4.Text = contactBoxText;
            }
        }
        private void textBox5_Enter(object sender, EventArgs e)
        {
            if (textBox5.Text == emailBoxText)
            {
                textBox5.Text = "";
            }
        }
        private void textBox5_Leave(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
            {
                textBox5.Text = emailBoxText;
            }
        }
        private void textBox6_Enter(object sender, EventArgs e)
        {
            if (textBox6.Text == regnoBoxText)
            {
                textBox6.Text = "";
            }
        }
        private void textBox6_Leave(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
            {
                textBox6.Text = regnoBoxText;
            }
        }
        private void clearBoxes()
        {
            searchBox.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
        }
        private void load()
        {
            var con = Configuration.getInstance().getConnection();
            string query = "Select * from Student Where Status = 5";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox2.Text == fnameBoxText || textBox3.Text == lnameBoxText || textBox4.Text == contactBoxText || textBox5.Text == emailBoxText || textBox6.Text == regnoBoxText)
            {
                MessageBox.Show(Messages.NOT_EMPTY);
                return;
            }
            var con = Configuration.getInstance().getConnection();
            string query = "Insert into Student values ('" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "','" + 5 + "')";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            load();
            MessageBox.Show(Messages.SUCCESS_INSERT);
            clearBoxes();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox2.Text == fnameBoxText || textBox3.Text == lnameBoxText || textBox4.Text == contactBoxText || textBox5.Text == emailBoxText || textBox6.Text == regnoBoxText)
            {
                MessageBox.Show(Messages.NOT_EMPTY);
                return;
            }
            var con = Configuration.getInstance().getConnection();
            string query = "Update Student set FirstName = '" + textBox2.Text + "', LastName = '" + textBox3.Text + "', Contact = '" + textBox4.Text + "', Email = '" + textBox5.Text + "', RegistrationNumber = '" + textBox6.Text + "' where Id = '" + StdID + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            load();
            MessageBox.Show(Messages.SUCCESS_UPDATE);
            clearBoxes();
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            string query = "Update Student set Status = 6 where Id = '" + StdID + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            load();
            MessageBox.Show(Messages.SUCCESS_DELETE);
            clearBoxes();
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
            string query = "Select * from Student where Status = 5 and (FirstName like '%" + searchBox.Text + "%' or LastName like '%" + searchBox.Text + "%' or Contact like '%" + searchBox.Text + "%' or Email like '%" + searchBox.Text + "%' or RegistrationNumber like '%" + searchBox.Text + "%')";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
                StdID = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                textBox6.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            }
        }
    }
}
