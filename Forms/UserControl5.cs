using System;
using System.Windows.Forms;
using CRUD_Operations;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Specialized;

namespace MidProject.Forms
{
    public partial class UserControl5 : UserControl
    {
        private int RubricLevelID = 0;
        private const string searchbox = "Search";
        private const string Details = "Enter Details";
        private const string rubricbox = "Select Rubric";
        private const string MeasurementLevel = "Select Level";
        public UserControl5()
        {
            InitializeComponent();
        }

        private void UserControl5_Load(object sender, EventArgs e)
        {
            load();
            combo1Load();
            searchBox.Text = searchbox;
            textBox1.Text = Details;
            comboBox1.Text = rubricbox;
            comboBox2.Text = MeasurementLevel;
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

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = Details;
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == Details)
            {
                textBox1.Text = "";
            }
        }

        private void load()
        {
            var con = Configuration.getInstance().getConnection();
            string query = "Select * from RubricLevel Where Details Not Like '%#Del%' ";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                RubricLevelID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }
        private void combo1Load()
        {
            var con = Configuration.getInstance().getConnection();
            string query = "Select * from Rubric Where Details NOT Like '%#Del%'";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader["Details"].ToString());
            }
            reader.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "" || textBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show(Messages.NOT_EMPTY);
                return;
            }
            var con = Configuration.getInstance().getConnection();
            string query = "Insert into RubricLevel (RubricId,Details,MeasurementLevel) values ((Select Id from Rubric Where Details = '" + comboBox1.Text + "'),'" + textBox1.Text + "','" + comboBox2.Text + "')";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            load();
            MessageBox.Show(Messages.SUCCESS_INSERT);
            comboBox1.Text = "";
            textBox1.Text = "";
            comboBox2.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "" || textBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show(Messages.NOT_EMPTY);
                return;
            }
            var con = Configuration.getInstance().getConnection();
            string query = "Update RubricLevel Set RubricId = (Select Id from Rubric Where Details = '" + comboBox1.Text + "'), Details = '" + textBox1.Text + "', MeasurementLevel = '" + comboBox2.Text + "' Where Id = " + RubricLevelID;
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            load();
            MessageBox.Show(Messages.SUCCESS_UPDATE);
            comboBox1.Text = "";
            textBox1.Text = "";
            comboBox2.Text = "";
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            string query = "Update RubricLevel Set Details = Details + '#Del' Where Id = " + RubricLevelID;
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            load();
            MessageBox.Show(Messages.SUCCESS_DELETE);
            comboBox1.Text = "";
            textBox1.Text = "";
            comboBox2.Text = "";
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            if (comboBox1.Text == rubricbox)
            {
                comboBox1.Text = "";
            }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                comboBox1.Text = rubricbox;
            }
        }

        private void comboBox2_Leave(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                comboBox2.Text = MeasurementLevel;
            }
        }

        private void comboBox2_Enter(object sender, EventArgs e)
        {
            if (comboBox2.Text == MeasurementLevel)
            {
                comboBox2.Text = "";
            }
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            if (searchBox.Text == searchbox)
            {
                return;
            }
            var con = Configuration.getInstance().getConnection();
            string query = "Select * from RubricLevel Where Details Like '%" + searchBox.Text + "%' And Details Not Like '%#Del%'";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
