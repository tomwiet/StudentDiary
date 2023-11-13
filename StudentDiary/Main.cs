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
            SetStudentsGroupsList();
            RefreshDiary();
            SetColumnsHeader();
            SetColumnsOrder();
            SetColumnsReadOnly();
           
        }
        public void SetColumnsReadOnly() 
        { 
            
            foreach(DataGridViewColumn column in dgvDiary.Columns)
                column.ReadOnly = true;

        }
        public void SetStudentsGroupsList()
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

            dgvDiary.Columns[nameof(Student.Id)].HeaderText = "Numer";
            dgvDiary.Columns[nameof(Student.FirstName)].HeaderText = "Imię";
            dgvDiary.Columns[nameof(Student.LastName)].HeaderText = "Nazwisko";
            dgvDiary.Columns[nameof(Student.StudentSection)].HeaderText = "Wydział";
            dgvDiary.Columns[nameof(Student.Comments)].HeaderText = "Uwagi";
            dgvDiary.Columns[nameof(Student.Math)].HeaderText = "Matematyka";
            dgvDiary.Columns[nameof(Student.Physics)].HeaderText = "Fizyka";
            dgvDiary.Columns[nameof(Student.Technology)].HeaderText = "Technologia";
            dgvDiary.Columns[nameof(Student.PolishLang)].HeaderText = "J. polski";
            dgvDiary.Columns[nameof(Student.ForeginLang)].HeaderText = "J. obcy";
            dgvDiary.Columns[nameof(Student.OptionalClasses)].HeaderText = "Zaj dod.";

        }
        public void SetColumnsOrder()
        {
            dgvDiary.Columns[nameof(Student.Id)].DisplayIndex = 0;
            dgvDiary.Columns[nameof(Student.FirstName)].DisplayIndex = 1;
            dgvDiary.Columns[nameof(Student.LastName)].DisplayIndex = 2;
            dgvDiary.Columns[nameof(Student.StudentSection)].DisplayIndex = 3;
            dgvDiary.Columns[nameof(Student.Comments)].DisplayIndex = 4;
            dgvDiary.Columns[nameof(Student.Math)].DisplayIndex = 5;
            dgvDiary.Columns[nameof(Student.Physics)].DisplayIndex = 6;
            dgvDiary.Columns[nameof(Student.Technology)].DisplayIndex = 7;
            dgvDiary.Columns[nameof(Student.PolishLang)].DisplayIndex = 8;
            dgvDiary.Columns[nameof(Student.ForeginLang)].DisplayIndex = 9;
            dgvDiary.Columns[nameof(Student.OptionalClasses)].DisplayIndex = 10;

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
