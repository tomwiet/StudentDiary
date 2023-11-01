﻿using System;
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
    public partial class AddEditStudent : Form
    {
        private string _filePath = Path.Combine(Environment.CurrentDirectory, "students.txt");
        private int _studentId;
        public AddEditStudent(int id=0)
        {
            InitializeComponent();
            _studentId = id;
            
            if (id != 0) 
            {
                var students = DeserializeFromFile();
                var student = students.FirstOrDefault(x => x.Id == id);

                if (student == null)
                    throw new Exception("Brak użytkownika o podanym Id");

                tbId.Text = student.Id.ToString();
                tbFirstName.Text = student.FirstName;
                tbLastName.Text = student.LastName;
                tbMath.Text = student.Math;
                tbPhysics.Text = student.Physics;
                tbTechnology.Text = student.Technology;
                tbPolishLang.Text = student.PolishLang;
                tbForeginLang.Text = student.ForeginLang;
                rtbComments.Text = student.Comments;
                
            }
            
            tbFirstName.Select();
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
            if (!File.Exists(_filePath))
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
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = DeserializeFromFile();
            if (_studentId != 0)
            {
                students.RemoveAll(x => x.Id == _studentId);
            }
            else
            {
                var studentWithHighestId = students
                    .OrderByDescending(x => x.Id).FirstOrDefault();
                /*    
                 var studentId = 0;

                 if(studentWithHighestId == null)
                 {
                    studentId = 1;
                 }
                 else
                 {
                    studentId = studentWithHighestId.Id + 1;
                 }
                */
                _studentId = studentWithHighestId == null ? 1 : studentWithHighestId.Id + 1;
            }

            var student = new Student()
            {
                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Comments = rtbComments.Text,
                Math = tbMath.Text,
                Physics = tbPhysics.Text,
                PolishLang = tbPolishLang.Text,
                ForeginLang = tbForeginLang.Text,
                Technology = tbTechnology.Text
            };
            
            students.Add(student);
            SerializeToFile(students);
            Close();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
