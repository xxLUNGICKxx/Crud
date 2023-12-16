using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homework
{
    public class table
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public table(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
