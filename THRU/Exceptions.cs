using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THRU
{
    public class SubjectNotFound : Exception
    {
        public SubjectNotFound(Subject subj) : base($"Subject not found: {subj.Id}")
        {
        }
    }

    public class ObjectNotFound : Exception
    {
        public ObjectNotFound(Object obj) : base($"Object not found: {obj.Id}")
        {
        }
    }
}
