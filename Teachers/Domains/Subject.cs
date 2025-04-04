using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teachers.Domains
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RecordId { get; set; }
        public Subject()
        {
            
        }
        public Subject(int id, string name, int recordId)
        {
            Id = id;
            Name = name;
            RecordId = recordId;
        }
    }
}
