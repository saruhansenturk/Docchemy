using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Docchemy.Generator
{
    public class Documentation
    {
        public Documentation(string document)
        {
            Document = document;
        }


        public bool IsSuccess { get; set; }
        public string Document { get; set; }
    }
}
