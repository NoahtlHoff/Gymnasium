using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gymnasium.Models
{
    public class Personal
    {
        public int PersonalID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string PersonalNumber { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
