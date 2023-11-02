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
            RefreshDiary();
            SetColumnsHeader();
        }
        public void RefreshDiary()
        {
            var students = _fileHelper.DeserializeFromFile().OrderBy(x => x.Id).ToList();
            dgvDiary.DataSource = students;

        }
        
        public void SetColumnsHeader()
        {
            
            dgvDiary.Columns[0].HeaderText = "Numer";
            dgvDiary.Columns[1].HeaderText = "Imię";
            dgvDiary.Columns[2].HeaderText = "Nazwisko";
            dgvDiary.Columns[3].HeaderText = "Uwagi";
            dgvDiary.Columns[4].HeaderText = "Matematyka";
            dgvDiary.Columns[5].HeaderText = "Fizyka";
            dgvDiary.Columns[6].HeaderText = "Technologia";
            dgvDiary.Columns[7].HeaderText = "J. polski";
            dgvDiary.Columns[8].HeaderText = "J. obcy";

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditStudent = new AddEditStudent();
            addEditStudent.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Musisz zanaczyć ktrego studenta chcesz edytować");
                return;
            }

            var addEditStudent = new AddEditStudent(
                Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));
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
                ($"{selectedStudent.Cells[1].Value.ToString()}" +
                $"{selectedStudent.Cells[2].Value.ToString()}").Trim(),
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
            students.RemoveAll(x =>x.Id == id );
            _fileHelper.SerializeToFile(students);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        
    }
}
