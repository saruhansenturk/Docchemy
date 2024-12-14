using System.Xml.Linq;
using Docchemy.CodeElements;

namespace Docchemy.Assembler.AssemblerService;

public abstract class RetrieverService
{
    public abstract List<ClassInfo> GetClassesInfo(string csFilePath);

    public abstract List<MethodInfoWithSummary> GetMethodsInfo(Type type, XDocument xmlDoc);

    public abstract List<InlineComment> GetInlineComments(string code);
}


