using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THRU
{
    public class Object
    {
        public string Id { get; }

        public ObjectType Type { get; set; }
        
        public Object(string id)
        {
            Id = id;
        }
    }
}
