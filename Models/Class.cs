using System.Collections.Generic;

namespace SchoolManagementSystem.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Student> Students { get; set; }

        public ICollection<Subject> Subjects { get; set; }
    }
}
