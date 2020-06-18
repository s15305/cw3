using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Models
{
    public partial class Studies
    {

        public int IdStudy { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Enrollment> Enrollment { get; set; }

        public Studies()
        {

            Enrollment = new HashSet<Enrollment>();

        }
    }
}
