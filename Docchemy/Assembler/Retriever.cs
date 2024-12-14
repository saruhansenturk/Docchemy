using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Docchemy.Assembler.AssemblerService;
using Docchemy.CodeElements;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Docchemy.Assembler;

public class Retriever : RetrieverService
{
    public override string GetClassesInfo(string csFilePath)
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
            var classComments = classDeclarationSyntax.GetLeadingTrivia()
                .Select(trivia => trivia.ToString().Trim())
                .Where(comment => comment.StartsWith("///"));

            foreach (var classComment in classComments)
            {
                stringBuilder.AppendLine($"  Comment: {classComment}");
            }

            var fields = classDeclarationSyntax.DescendantNodes().OfType<FieldDeclarationSyntax>();
            foreach (var field in fields)
            {
                var variableName = field.Declaration.Variables.First().Identifier.Text;
                stringBuilder.AppendLine($"  Field: {variableName}");

                var fieldComments = field.GetLeadingTrivia()
                    .Select(trivia => trivia.ToString().Trim())
                    .Where(comment => comment.StartsWith("///"));

                foreach (var comment in fieldComments)
                {
                    stringBuilder.AppendLine($"   Comment: {comment}");
                }

                var inlineComments = field.DescendantNodes()
                    .Where(trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
                    .Select(trivia => trivia.ToString().Trim());

                foreach (var inlineComment in inlineComments)
                {
                    var lineText = field.ToString().Split('\n').FirstOrDefault()?.Trim();
                    stringBuilder.AppendLine($"    Inline Comment: {inlineComment}");
                    stringBuilder.AppendLine($"    Code: {lineText}");
                }
            }

            var methods = classDeclarationSyntax.DescendantNodes().OfType<MethodDeclarationSyntax>();
            foreach (var method in methods)
            {
                stringBuilder.AppendLine($"   Method: {method.Identifier.Text}");

                var methodComments = method.GetLeadingTrivia()
                    .Select(trivia => trivia.ToString().Trim())
                    .Where(comment => comment.StartsWith("///"));

                foreach (var comment in methodComments)
                {
                    stringBuilder.AppendLine($"   Comment: {comment}");
                }

                // Method içindeki inline yorumları ve satırdaki kodu al
                var statements = method.Body?.Statements;
                if (statements != null)
                {
                    foreach (var statement in statements)
                    {
                        var inlineComments = statement.GetLeadingTrivia()
                            .Where(trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
                            .Select(trivia => trivia.ToString().Trim());

                        foreach (var inlineComment in inlineComments)
                        {
                            var lineText = statement.ToString().Split('\n').FirstOrDefault()?.Trim();
                            stringBuilder.AppendLine($"    Inline Comment: {inlineComment}");
                            stringBuilder.AppendLine($"    Code: {lineText}");
                        }
                    }
                }
            }
        }

        return stringBuilder.ToString();
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