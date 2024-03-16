using CRUD_Operations;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace MidProject.Forms
{
    public partial class UserControl9 : UserControl
    {
        private const string student = "Select Student";
        private const string assessment = "Select Assessment";
        public UserControl9()
        {
            InitializeComponent();
        }

        private void UserControl9_Load(object sender, EventArgs e)
        {
            loadBoxes();
            comboBox1.Text = student;
            comboBox2.Text = student;
            comboBox3.Text = assessment;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = CloWiseDataTable();
            CreatePdfFromDataTable(button1.Text, dt);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt = AssessmentWiseDataTable();
            CreatePdfFromDataTable(button2.Text, dt);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show(Messages.NOT_EMPTY);
                return;
            }
            DataTable dt = StudentReport();
            CreatePdfFromDataTable("Student No." + comboBox1.SelectedValue.ToString() + " Report", dt);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == -1 || comboBox3.SelectedIndex == -1)
            {
                MessageBox.Show(Messages.NOT_EMPTY);
                return;
            }
            DataTable dt = StudentAssessmentReport();
            CreatePdfFromDataTable("Student No." + comboBox2.SelectedValue.ToString() + comboBox3.SelectedText.ToString() + " Assessment Report", dt);
        }

        private void CreatePdfFromDataTable(string title, DataTable dataTable)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (.pdf)|.pdf";
            saveFileDialog.Title = "Export to PDF";
            saveFileDialog.FileName = title;
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                Document document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
                document.Open();
                Paragraph heading = new Paragraph(title, FontFactory.GetFont("Arial", 20));
                heading.Alignment = Element.ALIGN_CENTER;
                document.Add(heading);
                document.Add(new Chunk("\n"));

                PdfPTable pdfTable = new PdfPTable(dataTable.Columns.Count);
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.WidthPercentage = 100;
                pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                foreach (DataColumn column in dataTable.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName));
                    pdfTable.AddCell(cell);
                }

                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        string cellText = item != null ? item.ToString() : "";
                        pdfTable.AddCell(cellText);
                    }
                }

                document.Add(pdfTable);

                document.Close();

                MessageBox.Show("PDF file has been created!");
            }
        }
        DataTable CloWiseDataTable()
        {
            var con = Configuration.getInstance().getConnection();
            DataTable CLO = new DataTable();
            CLO.Columns.Add("Student Name");
            CLO.Columns.Add("CLO Name");
            CLO.Columns.Add("Marks Obtained");
            string query = "select count(*) from StudentResult";
            SqlCommand sqlcmd = new SqlCommand(query, con);
            sqlcmd.ExecuteScalar();
            string q = "SELECT DISTINCT CONCAT(Student.FirstName, ' ', Student.LastName) AS StudentName,   Clo.Name,   SUM((MeasurementLevel * AssessmentComponent.TotalMarks) / 4) as ObtainedMarks FROM    StudentResult INNER JOIN     RubricLevel ON StudentResult.RubricMeasurementId = RubricLevel.Id INNER JOIN   Rubric ON Rubric.Id = RubricLevel.RubricId INNER JOIN  Clo ON Clo.Id = Rubric.CloId INNER JOIN    AssessmentComponent ON AssessmentComponent.RubricId = Rubric.Id INNER JOIN     Student ON StudentResult.StudentId = Student.Id GROUP BY   Clo.Name, StudentId,Student.FirstName,Student.LastName";
            SqlCommand sqlcmd1 = new SqlCommand(q, con);
            SqlDataReader dr = sqlcmd1.ExecuteReader();
            while (dr.Read())
            {
                CLO.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
            }

            return CLO;
        }
        DataTable AssessmentWiseDataTable()
        {
            var con = Configuration.getInstance().getConnection();
            DataTable Ass = new DataTable();
            Ass.Columns.Add("Student name");
            Ass.Columns.Add("Assessment Title");
            Ass.Columns.Add("Marks Obtained");
            string query = "SELECT DISTINCT CONCAT(Student.FirstName, ' ', Student.LastName) AS StudentName,Assessment.Title,SUM(MeasurementLevel * AssessmentComponent.TotalMarks) / SUM(Assessment.TotalMarks) as ObtainedMarks  FROM StudentResult  INNER JOIN RubricLevel ON StudentResult.RubricMeasurementId = RubricLevel.Id  INNER JOIN Rubric ON Rubric.Id = RubricLevel.RubricId  INNER JOIN Clo ON Clo.Id = Rubric.CloId  INNER JOIN AssessmentComponent ON AssessmentComponent.RubricId = Rubric.Id  INNER JOIN Assessment ON Assessment.Id = AssessmentComponent.AssessmentId  INNER JOIN Student ON StudentResult.StudentId = Student.Id GROUP BY Assessment.Title,Student.FirstName,Student.LastName ";
            SqlCommand sqlcmd = new SqlCommand(query, con);
            SqlDataReader dr = sqlcmd.ExecuteReader();
            while (dr.Read())
            {
                Ass.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
            }

            return Ass;
        }
        DataTable StudentAssessmentReport()
        {
            var con = Configuration.getInstance().getConnection();
            string query = "SELECT AC.Name AS Component, (SELECT Details FROM Rubric WHERE Id = AC.RubricId) AS Rubric, AC.TotalMarks, AC.TotalMarks * CAST((SELECT MeasurementLevel FROM RubricLevel WHERE Id = SR.RubricMeasurementId AND RubricId = AC.RubricId) AS float)/ 4 AS ObtainedMarks FROM StudentResult SR JOIN AssessmentComponent AS AC ON SR.AssessmentComponentId = AC.Id AND AC.AssessmentId =  @AssessmentID WHERE StudentId = @StudentID";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@StudentID", comboBox2.SelectedValue);
            cmd.Parameters.AddWithValue("@AssessmentID", comboBox3.SelectedValue);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        DataTable StudentReport()
        {
            var con = Configuration.getInstance().getConnection();
            string query = "SELECT (SELECT Title FROM Assessment WHERE Id = AssessmentId) AS Assessment, SUM(ObtainedMarks) AS ObtainedMarks, (SELECT TotalMarks FROM Assessment WHERE Id = AssessmentId) AS TotalMarks, (SELECT TotalWeightage FROM Assessment WHERE Id = AssessmentId) As Weightage FROM (SELECT StudentId, AssessmentId, AC.TotalMarks * CAST((SELECT MeasurementLevel FROM RubricLevel WHERE Id = RubricMeasurementId) AS float) / 4 AS ObtainedMarks FROM StudentResult SR JOIN AssessmentComponent AC ON SR.AssessmentComponentId = AC.Id WHERE StudentId = @StudentID) p1 GROUP BY StudentId, AssessmentId";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@StudentID", comboBox1.SelectedValue);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private void loadBoxes()
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

            SqlDataAdapter da1 = new SqlDataAdapter(query, con);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);

            comboBox2.DataSource = dt1;
            comboBox2.DisplayMember = "RegistrationNumber";
            comboBox2.ValueMember = "Id";
            comboBox2.SelectedIndex = -1;

            string query2 = "SELECT * FROM Assessment Where Title NOT LIKE '%#Del%'";
            SqlDataAdapter da2 = new SqlDataAdapter(query2, con);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            comboBox3.DataSource = dt2;
            comboBox3.DisplayMember = "Title";
            comboBox3.ValueMember = "Id";
            comboBox3.SelectedIndex = -1;
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
                comboBox2.Text = student;
            }
        }

        private void comboBox2_Enter(object sender, EventArgs e)
        {
            if (comboBox2.Text == student)
            {
                comboBox2.Text = "";
            }
        }

        private void comboBox3_Leave(object sender, EventArgs e)
        {
            if (comboBox3.Text == "")
            {
                comboBox3.Text = assessment;
            }
        }

        private void comboBox3_Enter(object sender, EventArgs e)
        {
            if (comboBox3.Text == assessment)
            {
                comboBox3.Text = "";
            }
        }
    }
}


