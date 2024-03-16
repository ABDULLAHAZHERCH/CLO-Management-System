using CRUD_Operations;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MidProject.Forms
{
    public partial class UserControl2 : UserControl
    {
        private int CloID = 0;
        private const string searchBoxText = "Search";
        private const string cnameBoxText = "Enter Clo Name";
        public UserControl2()
        {
            InitializeComponent();
        }

        private void UserControl2_Load(object sender, EventArgs e)
        {
            load();
            textBox1.Text = cnameBoxText;
            searchBox.Text = searchBoxText;
        }
        private void clearBoxes()
        {
            searchBox.Text = "";
            textBox1.Text = "";
        }
        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == cnameBoxText)
            {
                textBox1.Text = "";
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = cnameBoxText;
            }
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

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            if (searchBox.Text == searchBoxText)
            {
                return;
            }
            var con = Configuration.getInstance().getConnection();
            string query = "Select * from Clo Where Name Like '%" + searchBox.Text + "%' And Name Not Like '%#Del%' ";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void load()
        {
            var con = Configuration.getInstance().getConnection();
            string query = "Select * from Clo Where Name Not Like '%#Del%' ";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == cnameBoxText)
            {
                MessageBox.Show(Messages.NOT_EMPTY);
                return;
            }
            var con = Configuration.getInstance().getConnection();
            var myDateTime = DateTime.Now;
            var sqlFormattedDate = myDateTime.Date.ToString("yyyy-MM-dd HH:mm:ss");
            string query = "Insert Into Clo(Name, DateCreated, DateUpdated) " + "Values('" + textBox1.Text + "', '" + sqlFormattedDate + "', '" + sqlFormattedDate + "' )";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            load();
            MessageBox.Show(Messages.SUCCESS_INSERT);
            clearBoxes();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == cnameBoxText)
            {
                MessageBox.Show(Messages.NOT_EMPTY);
                return;
            }
            var con = Configuration.getInstance().getConnection();
            var myDateTime = DateTime.Now;
            var sqlFormattedDate = myDateTime.Date.ToString("yyyy-MM-dd HH:mm:ss");
            string query = "Update Clo Set Name = '" + textBox1.Text + "', DateUpdated = '" + sqlFormattedDate + "' Where Id = " + CloID;
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
            string query = "Update Clo Set Name = Name + '#Del' Where Id = " + CloID;
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            MessageBox.Show(Messages.SUCCESS_DELETE);
            clearBoxes();
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            load();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                CloID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }

    }
}