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

        private FileHelper<List<Student>> _fileHelper =
            new FileHelper<List<Student>>(Program.FilePath);
        public AddEditStudent(int id = 0)
        {
            InitializeComponent();

            _studentId = id;
            cbxStudentGroup.Items.AddRange(School.studentsSections.ToArray());
            GetStudentData();
            tbFirstName.Select();

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

                FillEditingControls(_student.StudentSection);



            }

        }

        private void FillEditingControls(string studentGroupName)
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
            cbxStudentGroup.SelectedIndex =
                School.studentsSections.IndexOf(studentGroupName);
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
                StudentSection = cbxStudentGroup.SelectedItem.ToString(),
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
