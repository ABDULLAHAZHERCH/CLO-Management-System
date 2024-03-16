using CRUD_Operations;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MidProject.Forms
{
    public partial class UserControl8 : UserControl
    {
        private const string student = "Select Student";
        private const string rubric = "Select Rubric";
        private const string component = "Select Component";
        public UserControl8()
        {
            InitializeComponent();
        }

        private void UserControl8_Load(object sender, EventArgs e)
        {
            load();
            loadStudentData();
            loadAssessmentComponents();
            loadRubrics();
            comboBox1.Text = student;
            comboBox2.Text = component;
            comboBox3.Text = rubric;
        }

        private void load()
        {
            var con = Configuration.getInstance().getConnection();
            string query = "Select * from StudentResult";
            SqlDataAdapter sql = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sql.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void comboBox1_Enter(object sender, EventArgs e)
        {
            if (comboBox1.Text == student)
            {
                comboBox1.Text = "";
            }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                comboBox1.Text = student;
            }
        }

        private void comboBox2_Leave(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                comboBox2.Text = component;
            }
        }

        private void comboBox2_Enter(object sender, EventArgs e)
        {
            if (comboBox2.Text == component)
            {
                comboBox2.Text = "";
            }
        }

        private void comboBox3_Enter(object sender, EventArgs e)
        {
            if (comboBox3.Text == rubric)
            {
                comboBox3.Text = "";
            }
        }

        private void comboBox3_Leave(object sender, EventArgs e)
        {
            if (comboBox3.Text == "")
            {
                comboBox3.Text = rubric;
            }
        }

        private void loadStudentData()
        {
            var con = Configuration.getInstance().getConnection();
            string query = "SELECT * FROM Student Where Status = 5";
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "RegistrationNumber";
            comboBox1.ValueMember = "Id";
            comboBox1.SelectedIndex = -1;

        }
        private void loadRubrics()
        {
            var con = Configuration.getInstance().getConnection();
            string query = "SELECT * FROM RubricLevel Where Details NOT LIKE '%#DEL%'";
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox3.DataSource = dt;
            comboBox3.DisplayMember = "Details";
            comboBox3.ValueMember = "Id";
            comboBox3.SelectedIndex = -1;
        }
        private void loadAssessmentComponents()
        {
            var con = Configuration.getInstance().getConnection();
            string query = "SELECT * FROM AssessmentComponent Where Name NOT LIKE '%#DEL%'";
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "Id";
            comboBox2.SelectedIndex = -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addStudentResult((int)comboBox1.SelectedValue, (int)comboBox2.SelectedValue, (int)comboBox3.SelectedValue, DateTime.Now);
            load();
        }


        public void addStudentResult(int studentID, int componentID, int rubricMeasurementID, DateTime date)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("INSERT INTO StudentResult VALUES (@StudentID, @ComponentID, @RubricID, @Date)", con);
                cmd.Parameters.AddWithValue("@StudentID", studentID);
                cmd.Parameters.AddWithValue("@ComponentID", componentID);
                cmd.Parameters.AddWithValue("@RubricID", rubricMeasurementID);
                cmd.Parameters.AddWithValue("@Date", date);

                cmd.ExecuteNonQuery();
            }
            catch (Exception) { MessageBox.Show(Messages.ADDED,"Evaluation Exists"); };
        }
    }
}
