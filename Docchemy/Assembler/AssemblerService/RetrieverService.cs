using System.Xml.Linq;
using Docchemy.CodeElements;

namespace Docchemy.Assembler.AssemblerService;

public abstract class RetrieverService
{
    public abstract string GetClassesInfo(string csFilePath);
}


