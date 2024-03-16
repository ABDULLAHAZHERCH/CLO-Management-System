using MidProject.Forms;
using System;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace MidProject
{
    public partial class MainForm : Form
    {
        private string label = "CLO MANAGMENT SYSTEM";
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            label1.Text = label;
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            MainForm_Load(sender, e);
            childPanel.Controls.Clear();
        }

        private void stubtn_Click(object sender, EventArgs e)
        {
            label1.Text = "Student Management";
            UserControl1 userControl1 = new UserControl1();
            userControl1.Dock = DockStyle.Fill;
            childPanel.Controls.Clear();
            childPanel.Controls.Add(userControl1);
        }

        private void cloBtn_Click(object sender, EventArgs e)
        {
            label1.Text = "CLO Management";
            UserControl2 userControl2 = new UserControl2();
            userControl2.Dock = DockStyle.Fill;
            childPanel.Controls.Clear();
            childPanel.Controls.Add(userControl2);
        }

        private void rubricBtn_Click(object sender, EventArgs e)
        {
            label1.Text = "Rubric Management";
            UserControl3 userControl3 = new UserControl3();
            userControl3.Dock = DockStyle.Fill;
            childPanel.Controls.Clear();
            childPanel.Controls.Add(userControl3);
        }

        private void assessmentBtn_Click(object sender, EventArgs e)
        {
            label1.Text = "Assessment Management";
            UserControl4 userControl4 = new UserControl4();
            userControl4.Dock = DockStyle.Fill;
            childPanel.Controls.Clear();
            childPanel.Controls.Add(userControl4);
        }

        private void rubricLevelsBtn_Click(object sender, EventArgs e)
        {
            label1.Text = "Rubric Level Management";
            UserControl5 userControl5 = new UserControl5();
            userControl5.Dock = DockStyle.Fill;
            childPanel.Controls.Clear();
            childPanel.Controls.Add(userControl5);
        }

        private void asscompbtn_Click(object sender, EventArgs e)
        {
            label1.Text = "Assessment Component Management";
            UserControl6 userControl6 = new UserControl6();
            userControl6.Dock = DockStyle.Fill;
            childPanel.Controls.Clear();
            childPanel.Controls.Add(userControl6);
        }

        private void attendancebtn_Click(object sender, EventArgs e)
        {
            label1.Text = "Attendance Management";
            UserControl7 userControl7 = new UserControl7();
            userControl7.Dock = DockStyle.Fill;
            childPanel.Controls.Clear();
            childPanel.Controls.Add(userControl7);
        }

        private void evlauationbtn_Click(object sender, EventArgs e)
        {
            label1.Text = "Evaluation Management";
            UserControl8 userControl8 = new UserControl8();
            userControl8.Dock = DockStyle.Fill;
            childPanel.Controls.Clear();
            childPanel.Controls.Add(userControl8);
        }

        private void reportBtn_Click(object sender, EventArgs e)
        {
            label1.Text = "Report Management";
            UserControl9 userControl9 = new UserControl9();
            userControl9.Dock = DockStyle.Fill;
            childPanel.Controls.Clear();
            childPanel.Controls.Add(userControl9);
        }

    }
}