using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDiary
{
    public class GroupHelper
    {
        public static List<Group> GetGroupList(string defaultGroup)
        {
            return new List<Group>() 
            { 
                new Group() {Id = 0,Name = defaultGroup},
                new Group() {Id = 1,Name = "Mechanika"},
                new Group() {Id = 2,Name = "Budownictwo"},
                new Group() {Id = 3,Name = "Chemia"}
            };
        }
    }
}
