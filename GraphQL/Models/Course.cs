using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQL.Models
{   
    public class Course
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public Instructor Instructor { get; set; }
        public Guid Instructorid { get; set; }     
        public List<Student>? Students { get; set; }= new List<Student>();

    }
}
