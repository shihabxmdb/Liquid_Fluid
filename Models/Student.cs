using System.Net;

namespace Liquid_Fluid.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public Contact Contact { get; set; }
        public Address Address { get; set; }

        public List<Course> Courses { get; set; }
    }
}
