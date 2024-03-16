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
    public partial class UserControl7 : UserControl
    {
        public UserControl7()
        {
            InitializeComponent();
            load();
            DateTime datetimee = DateTime.Now;
            DateTime onlydate = datetimee.Date;
            dateTimePicker1.Value = datetimee;
            getStudentRecord(onlydate);
        }

        private void UserControl7_Load(object sender, EventArgs e)
        {

        }
        private void load()
        {
            var con = Configuration.getInstance().getConnection();
            string query = "Select ID AS StudentID, CONCAT(FirstName , ' ' , LastName) As Name, RegistrationNumber As [Registration Number] from Student where Status = '5' Group by id,RegistrationNumber,FirstName,LastName";
            SqlDataAdapter sqladp = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sqladp.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["StudentID"].ReadOnly = true;
            dataGridView1.Columns["Name"].ReadOnly = true;
            dataGridView1.Columns["Registration Number"].ReadOnly = true;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt = new DateTime();
            dt = dateTimePicker1.Value.Date;
            DateTime selectedDate = dt.Date;
            getStudentRecord(selectedDate);
        }
        private int GetStudentId(int rowIndex)
        {
            DataGridViewRow row = dataGridView1.Rows[rowIndex];
            int studentId = Convert.ToInt32(row.Cells["StudentID"].Value);
            return studentId;
        }
        private int InsertAttendance(DateTime attendanceDate)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("INSERT INTO ClassAttendance (AttendanceDate) VALUES (@AttendanceDate); SELECT SCOPE_IDENTITY();", con);
            cmd.Parameters.AddWithValue("@AttendanceDate", attendanceDate.Date);
            int attendanceId = Convert.ToInt32(cmd.ExecuteScalar());
            return attendanceId;
        }
        private void InsertStudentAttendance(int studentId, int attendanceId, int attendanceStatus)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("INSERT INTO StudentAttendance (StudentID, AttendanceID, AttendanceStatus) VALUES (@StudentID, @AttendanceID, @AttendanceStatus)", con);
            cmd.Parameters.AddWithValue("@StudentID", studentId);
            cmd.Parameters.AddWithValue("@AttendanceID", attendanceId);
            cmd.Parameters.AddWithValue("@AttendanceStatus", attendanceStatus);
            cmd.ExecuteNonQuery();
        }
        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            ComboBox combo = e.Control as ComboBox;
            if (combo != null)
            {
                combo.SelectedIndexChanged -= new EventHandler(attendance);
                combo.SelectedIndexChanged += attendance;
            }
        }
        private void getStudentRecord(DateTime date)
        {
            DataTable dt = new DataTable();
            bool isAttendanceMarked = IsAttendanceMarked(date);
            dataGridView1.Columns["Status"].Visible = !isAttendanceMarked;
            button1.Visible = isAttendanceMarked;

            if (isAttendanceMarked)
            {
                dt = GetAttendanceData(date);
            }
            if (dt.Rows.Count == 0)
            {
                dt = GetAllStudentsData();
            }

            dataGridView1.DataSource = dt;
        }
        private bool IsAttendanceMarked(DateTime date)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM ClassAttendance WHERE AttendanceDate = @Date", con);
            checkCmd.Parameters.AddWithValue("@Date", date.Date);
            int count = Convert.ToInt32(checkCmd.ExecuteScalar());
            return count > 0;
        }
        private DataTable GetAttendanceData(DateTime date)
        {
            var con = Configuration.getInstance().getConnection();
            string query = "SELECT DISTINCT Student.Id as StudentId,CONCAT(Student.FirstName, ' ', Student.LastName) AS Name, Student.RegistrationNumber, Lookup.Name as 'Attendance_Status'FROM Student LEFT JOIN StudentAttendance ON Student.Id = StudentAttendance.StudentId LEFT JOIN ClassAttendance ON StudentAttendance.AttendanceId = ClassAttendance.Id LEFT JOIN Lookup ON StudentAttendance.AttendanceStatus = Lookup.LookupID WHERE ClassAttendance.AttendanceDate = @Date AND Lookup.Category = 'Attendance_Status'";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Date", date.Date);
            SqlDataReader sdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sdr);
            dt = dt.DefaultView.ToTable(true);
            return dt;
        }
        private DataTable GetAllStudentsData()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Id as StudentId, CONCAT(FirstName, ' ', LastName) AS Name, RegistrationNumber FROM Student", con);
            SqlDataReader sdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sdr);
            return dt;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (.pdf)|.pdf";
            saveFileDialog.Title = "Export to PDF";
            saveFileDialog.FileName = "Attendance " + dateTimePicker1.Value.ToString("D");
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                Document document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
                document.Open();
                Paragraph heading = new Paragraph("Attendance Report - " + dateTimePicker1.Value.ToString("D"), FontFactory.GetFont("Arial", 20));
                heading.Alignment = Element.ALIGN_CENTER;
                document.Add(heading);
                document.Add(new Chunk("\n"));

                PdfPTable pdfTable = new PdfPTable(dataGridView1.ColumnCount - 1);
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.WidthPercentage = 100;
                pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                for (int i = 1; i < dataGridView1.Columns.Count; i++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(dataGridView1.Columns[i].HeaderText));
                    pdfTable.AddCell(cell);
                }

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    for (int i = 1; i < row.Cells.Count; i++)
                    {
                        object cellValue = row.Cells[i].Value;
                        string cellText = cellValue != null ? cellValue.ToString() : "";
                        pdfTable.AddCell(cellText);
                    }
                }

                document.Add(pdfTable);

                document.Close();

                MessageBox.Show("PDF file has been created!");
            }

        }
        private void attendance(object sender, EventArgs e)
        {
            DataGridViewComboBoxEditingControl combo = sender as DataGridViewComboBoxEditingControl;
            if (combo != null)
            {

                string attendanceStatus = combo.SelectedItem.ToString();
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                int studentId = GetStudentId(rowIndex);
                DateTime attendanceDate = dateTimePicker1.Value.Date;
                int attendanceStatusId = -1;
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT LookupID FROM Lookup WHERE Name = @Name AND Category = 'Attendance_Status'", con);
                cmd.Parameters.AddWithValue("@Name", attendanceStatus);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    attendanceStatusId = Convert.ToInt32(reader["LookupID"]);
                }
                reader.Close();
                int attendanceId = InsertAttendance(attendanceDate);
                InsertStudentAttendance(studentId, attendanceId, attendanceStatusId);
            }
        }


    }
}
