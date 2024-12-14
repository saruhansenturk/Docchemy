using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Docchemy.Assembler.AssemblerService;
using Docchemy.CodeElements;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Docchemy.Assembler;

public class Retriever : RetrieverService
{
    public override List<ClassInfo> GetClassesInfo(string csFilePath)
    {
        var code = File.ReadAllText(csFilePath);
        var tree = CSharpSyntaxTree.ParseText(code);
        var root = tree.GetCompilationUnitRoot();

        var stringBuilder = new StringBuilder();

        var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

        foreach (var classDeclarationSyntax in classes)
        {
            stringBuilder.AppendLine($"Classes: {classDeclarationSyntax.Identifier.Text}");

            // get the summaries
            var classComments = classDeclarationSyntax.GetLeadingTrivia().Select(trivia => trivia.ToString().Trim())
                .Where(comment => comment.StartsWith("///"));

            foreach (var classComment in classComments)
            {
                stringBuilder.AppendLine($"  Comment: {classComment}");
            }
        }

    }

    public override List<MethodInfoWithSummary> GetMethodsInfo(Type type, XDocument xmlDoc)
    {
        throw new NotImplementedException();
    }

    public override List<InlineComment> GetInlineComments(string code)
    {
        throw new NotImplementedException();
    }

    private string _GetXmlSummary(XDocument xmlDoc, MemberInfo member)
    {
        var memberName = member is Type type ?
            $"T:{type.Name}" :
            $"M:{member.DeclaringType?.FullName}.{member.Name}";

        var summaryElement = xmlDoc.Descendants("member").FirstOrDefault(m => m.Attribute("name")?.Value == memberName);


        return summaryElement?.Element("summary")?.Value.Trim() ?? string.Empty;
    }
}