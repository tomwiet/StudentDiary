using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace StudentDiary
{
    public partial class Main : Form
    {

        private FileHelper<List<Student>> _fileHelper =
            new FileHelper<List<Student>>(Program.FilePath);
        public Main()
        {

            InitializeComponent();
            setStudentsGroupsList();
            RefreshDiary();
            SetColumnsHeader();
            SetColumnsOrder();
            dgvDiary.Columns["OptionalClasses"].ReadOnly = true;


        }
        public void setStudentsGroupsList()
        {
            cbxIdOfStudentClass.Items.Add("Wszyscy");
            cbxIdOfStudentClass.Items.AddRange(School.studentsSections.ToArray());
            cbxIdOfStudentClass.SelectedIndex = 0;
        }
        public void RefreshDiary()
        {

            var students = _fileHelper.DeserializeFromFile().OrderBy(x => x.Id).ToList();


            if (!cbxIdOfStudentClass.SelectedItem.Equals("Wszyscy"))
            {
                students.RemoveAll(
                    x => x.StudentSection != cbxIdOfStudentClass.SelectedItem.ToString());
            }

            dgvDiary.DataSource = students;

        }

        public void SetColumnsHeader()
        {

            dgvDiary.Columns["id"].HeaderText = "Numer";
            dgvDiary.Columns["FirstName"].HeaderText = "Imię";
            dgvDiary.Columns["LastName"].HeaderText = "Nazwisko";
            dgvDiary.Columns["StudentSection"].HeaderText = "Wydział";
            dgvDiary.Columns["Comments"].HeaderText = "Uwagi";
            dgvDiary.Columns["Math"].HeaderText = "Matematyka";
            dgvDiary.Columns["Physics"].HeaderText = "Fizyka";
            dgvDiary.Columns["Technology"].HeaderText = "Technologia";
            dgvDiary.Columns["PolishLang"].HeaderText = "J. polski";
            dgvDiary.Columns["ForeginLang"].HeaderText = "J. obcy";
            dgvDiary.Columns["OptionalClasses"].HeaderText = "Zaj dod.";

        }
        public void SetColumnsOrder()
        {
            dgvDiary.Columns["id"].DisplayIndex = 0;
            dgvDiary.Columns["FirstName"].DisplayIndex = 1;
            dgvDiary.Columns["LastName"].DisplayIndex = 2;
            dgvDiary.Columns["StudentSection"].DisplayIndex = 3;
            dgvDiary.Columns["Comments"].DisplayIndex = 4;
            dgvDiary.Columns["Math"].DisplayIndex = 5;
            dgvDiary.Columns["Physics"].DisplayIndex = 6;
            dgvDiary.Columns["Technology"].DisplayIndex = 7;
            dgvDiary.Columns["PolishLang"].DisplayIndex = 8;
            dgvDiary.Columns["ForeginLang"].DisplayIndex = 9;
            dgvDiary.Columns["OptionalClasses"].DisplayIndex = 10;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditStudent = new AddEditStudent();
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();
        }

        private void AddEditStudent_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshDiary();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Musisz zanaczyć ktrego studenta chcesz edytować");
                return;
            }

            var addEditStudent = new AddEditStudent(
                Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Musisz zanaczyć ktrego studenta chcesz usunąć");
                return;
            }
            var selectedStudent = dgvDiary.SelectedRows[0];

            var confirmDelete = MessageBox.Show($"Czy na pewno chcesz usunąć studenta " +
                ($"{selectedStudent.Cells[1].Value}" +
                $"{selectedStudent.Cells[2].Value}").Trim(),
                "Usuwanie studenta",
                MessageBoxButtons.OKCancel);
            if (confirmDelete == DialogResult.OK)
            {
                DeleteStudent(Convert.ToInt32(selectedStudent.Cells[0].Value));
                RefreshDiary();

            }

        }
        private void DeleteStudent(int id)
        {
            var students = _fileHelper.DeserializeFromFile();
            students.RemoveAll(x => x.Id == id);
            _fileHelper.SerializeToFile(students);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void cbxIdOfStudentGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshDiary();
        }
    }
}
