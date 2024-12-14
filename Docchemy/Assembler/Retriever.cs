using System.Reflection;
using System.Text;
using Docchemy.Assembler.AssemblerService;
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
            var classComments = classDeclarationSyntax
                .GetLeadingTrivia()
                .Select(trivia => trivia.ToString().Trim())
                .Where(comment => comment.Contains("summary") && comment.Contains("///"));

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
                    .Where(comment => comment.Contains("summary") && comment.Contains("///"));

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
                    .Where(comment => comment.Contains("summary") && comment.Contains("///"));

                foreach (var comment in methodComments)
                {
                    stringBuilder.AppendLine($"   Comment: {comment}");
                }

                var statements = method.Body?.DescendantNodes().OfType<StatementSyntax>();
                if (statements != null)
                {
                    foreach (var statement in statements)
                    {
                        var inlineComments = statement.GetLeadingTrivia()
                            .Concat(statement.GetTrailingTrivia())
                            .Where(trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) ||
                                             trivia.IsKind(SyntaxKind.MultiLineCommentTrivia) ||
                                             trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia))
                            .Select(trivia => trivia.ToString().Trim());

                        var fullStatementText = statement.ToFullString().Trim();

                        foreach (var inlineComment in inlineComments)
                        {
                            stringBuilder.AppendLine($"    Inline Comment: {inlineComment}");
                            stringBuilder.AppendLine($"    Code: {fullStatementText}");
                        }
                    }
                }
            }
        }

        return stringBuilder.ToString();
    }
}