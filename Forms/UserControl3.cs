using CRUD_Operations;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MidProject.Forms
{
    public partial class UserControl3 : UserControl
    {
        private int RubricID = 0;
        private const string searchBoxText = "Search";
        private const string cnameBoxText = "Enter Rubric Details";
        private const string cloBoxText = "Select CLO";
        public UserControl3()
        {
            InitializeComponent();
        }

        private void UserControl3_Load(object sender, EventArgs e)
        {
            load();
            fillCombo();
            textBox1.Text = cnameBoxText;
            searchBox.Text = searchBoxText;
            comboBox1.Text = cloBoxText;

        }
        private void load()
        {
            var con = Configuration.getInstance().getConnection();
            string query = "Select * from Rubric Where Details Not Like '%#Del%' ";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            if (searchBox.Text == searchBoxText)
            {
                return;
            }
            var con = Configuration.getInstance().getConnection();
            string query = "Select * from Rubric Where Details Like '%" + searchBox.Text + "%' And Details Not Like '%#Del%' ";
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
                textBox1.Text = cnameBoxText;
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == cnameBoxText)
            {
                textBox1.Text = "";
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                RubricID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }

        private void fillCombo()
        {
            string query = "Select * from Clo Where Name Not Like '%#Del%'";
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                comboBox1.Items.Add(dr["Name"].ToString());
            }
            dr.Close();

        }

        private void getID()
        {
            string query = "Select Count(Id) from Rubric";
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand(query, con);
            RubricID = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == cnameBoxText)
            {
                MessageBox.Show(Messages.NOT_EMPTY);
                return;
            }
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select CLO");
                return;
            }
            var con = Configuration.getInstance().getConnection();
            getID();
            string query = "Insert Into Rubric(Id, Details, CloId) " + "Values(" + RubricID + ", '" + textBox1.Text + "', (Select Id from Clo Where Name = '" + comboBox1.SelectedItem.ToString() + "') )";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            load();
            textBox1.Text = cnameBoxText;
            comboBox1.SelectedIndex = -1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == cnameBoxText)
            {
                MessageBox.Show("Please Enter Rubric Details");
                return;
            }
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select CLO");
                return;
            }
            var con = Configuration.getInstance().getConnection();
            string query = "Update Rubric Set Details = '" + textBox1.Text + "', CloId = (Select Id from Clo Where Name = '" + comboBox1.SelectedItem.ToString() + "') Where Id = " + RubricID;
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            load();
            textBox1.Text = cnameBoxText;
            comboBox1.SelectedIndex = -1;
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            string query = "Update Rubric Set Details = Details + '#Del' Where Id = " + RubricID;
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            load();
            textBox1.Text = cnameBoxText;
            comboBox1.SelectedIndex = -1;
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            if (comboBox1.Text == cloBoxText)
            {
                comboBox1.SelectedIndex = -1;
            }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                comboBox1.Text = cloBoxText;
            }
        }
    }
}
