using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docchemy.Generator.DocumenterService
{
    public abstract class DocumenterService: IDocumenter
    {
        public abstract Task<Documentation> DocumantateAsync(Dictionary<string, List<string>> analyzedClasses);
    }
}
