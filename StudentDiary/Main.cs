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
        private string _filePath = Path.Combine(Environment.CurrentDirectory,"students.txt");
        public Main()
        {
            InitializeComponent();
            var students = DeserializeFromFile().OrderBy(x => x.Id).ToList();
            dgvDiary.DataSource = students;
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

        

        public void SerializeToFile(List<Student> students) 
        {
            var serializer = new XmlSerializer(typeof(List<Student>));

            using (var streamWriter = new StreamWriter(_filePath)) 
            {
                serializer.Serialize(streamWriter, students);
                
                streamWriter.Close();

            }
        }

        public List<Student> DeserializeFromFile() 
        {
            if(!File.Exists(_filePath)) 
            {
                return new List<Student>();
            }
            var serializer = new XmlSerializer(typeof(List<Student>));

            using (var streamReader = new StreamReader(_filePath))
            {
                var students = (List<Student>)serializer.Deserialize(streamReader);
                streamReader.Close();
                return students;
            }
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
                var students = DeserializeFromFile();
                students.RemoveAll(x =>
                x.Id == Convert.ToInt32(selectedStudent.Cells[0].Value));
                SerializeToFile(students);
                dgvDiary.DataSource = students;

            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var students = DeserializeFromFile().OrderBy(x => x.Id).ToList();
            dgvDiary.DataSource = students;
        }

        
    }
}
