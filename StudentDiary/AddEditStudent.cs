using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace StudentDiary
{
    public partial class AddEditStudent : Form
    {
        private int _studentId;
        private Student _student;
        private List<Group> _groups;


        private FileHelper<List<Student>> _fileHelper =
            new FileHelper<List<Student>>(Program.FilePath);
        public AddEditStudent(int id = 0)
        {
            _groups = GroupHelper.GetGroupList("Brak");

            InitializeComponent();

            _studentId = id;
            SetStudentsGroupsList();
            GetStudentData();
            tbFirstName.Select();

        }
        public void SetStudentsGroupsList()
        {
            _groups = GroupHelper.GetGroupList("Brak");
            cbxStudentGroup.DataSource = _groups;
            cbxStudentGroup.DisplayMember = "Name";
            cbxStudentGroup.ValueMember = "Id";
        }
        private void GetStudentData()
        {
            if (_studentId != 0)
            {
                Text = "Edytuj dane studenta";
                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentId);

                if (_student == null)
                    throw new Exception("Brak użytkownika o podanym Id");

                FillEditingControls();

            }

        }

        private void FillEditingControls()
        {
            tbId.Text = _student.Id.ToString();
            tbFirstName.Text = _student.FirstName;
            tbLastName.Text = _student.LastName;
            tbMath.Text = _student.Math;
            tbPhysics.Text = _student.Physics;
            tbTechnology.Text = _student.Technology;
            tbPolishLang.Text = _student.PolishLang;
            tbForeginLang.Text = _student.ForeginLang;
            rtbComments.Text = _student.Comments;
            cbxStudentGroup.SelectedItem = 
                _groups.FirstOrDefault(x=>x.Id == _student.StudentGroup);
                
            cbOptionalClasses.Checked = _student.OptionalClasses;
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            if (_studentId != 0)
                students.RemoveAll(x => x.Id == _studentId);
            else
                AssignIdToNewStudent(students);

            AddNewUserToList(students);
            _fileHelper.SerializeToFile(students);
            //await LongProcessAsync();
            Close();

        }

        /* private async Task LongProcessAsync()

         {
             await Task.Run(() =>
             {
                 Thread.Sleep(3000);

             });

         }*/
        private void AddNewUserToList(List<Student> students)
        {
            var student = new Student()
            {
                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                StudentGroup = (cbxStudentGroup.SelectedItem as Group).Id,
                Comments = rtbComments.Text,
                Math = tbMath.Text,
                Physics = tbPhysics.Text,
                PolishLang = tbPolishLang.Text,
                ForeginLang = tbForeginLang.Text,
                Technology = tbTechnology.Text,
                OptionalClasses = cbOptionalClasses.Checked
            };

            students.Add(student);

        }
        private void AssignIdToNewStudent(List<Student> students)
        {
            var studentWithHighestId = students
                    .OrderByDescending(x => x.Id).FirstOrDefault();

            _studentId = studentWithHighestId == null ? 1 : studentWithHighestId.Id + 1;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
