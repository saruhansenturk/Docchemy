using System.Reflection;
using System.Text;
using Docchemy.Analysis.ProjectAnalyzer.CommentAnalyzer.CommentAnalyzerService;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Docchemy.Analysis.ProjectAnalyzer.CommentAnalyzer;

public class Analyzer : AnalyzerService
{
    public override string GetHierarchicalClassInfo(string csFilePath)
    {
        var code = File.ReadAllText(csFilePath);
        var tree = CSharpSyntaxTree.ParseText(code);
        var root = tree.GetCompilationUnitRoot();

        var stringBuilder = new StringBuilder();

        foreach (var classDeclaration in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
        {
            AppendClassInfo(classDeclaration, stringBuilder);
        }

        return stringBuilder.ToString();
    }

    private void AppendClassInfo(ClassDeclarationSyntax classDeclaration, StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine($"Class: {classDeclaration.Identifier.Text}");
        AppendComments(classDeclaration, stringBuilder, "  Comment:");

        foreach (var field in classDeclaration.DescendantNodes().OfType<FieldDeclarationSyntax>())
        {
            AppendFieldInfo(field, stringBuilder);
        }

        foreach (var method in classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>())
        {
            AppendMethodInfo(method, stringBuilder);
        }
    }

    private void AppendFieldInfo(FieldDeclarationSyntax field, StringBuilder stringBuilder)
    {
        var variableName = field.Declaration.Variables.First().Identifier.Text;
        stringBuilder.AppendLine($"  Field: {variableName}");
        AppendComments(field, stringBuilder, "    Comment:");
    }

    private void AppendMethodInfo(MethodDeclarationSyntax method, StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine($"  Method: {method.Identifier.Text}");
        AppendComments(method, stringBuilder, "    Comment:");

        var statements = method.Body?.DescendantNodes().OfType<StatementSyntax>();
        if (statements != null)
        {
            foreach (var statement in statements)
            {
                AppendInlineComments(statement, stringBuilder);
            }
        }
    }

    private void AppendComments(SyntaxNode node, StringBuilder stringBuilder, string prefix)
    {
        var comments = node.GetLeadingTrivia()
            .Where(trivia => trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia))
            .Select(trivia => trivia.ToString().Trim());

        foreach (var comment in comments)
        {
            stringBuilder.AppendLine($"{prefix} {comment}");
        }
    }

    private void AppendInlineComments(StatementSyntax statement, StringBuilder stringBuilder)
    {
        var comments = statement.GetLeadingTrivia()
            .Concat(statement.GetTrailingTrivia())
            .Where(trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) ||
                             trivia.IsKind(SyntaxKind.MultiLineCommentTrivia))
            .Select(trivia => trivia.ToString().Trim());

        var fullStatementText = statement.ToFullString().Trim();

        foreach (var comment in comments)
        {
            stringBuilder.AppendLine($"    Inline Comment: {comment}");
            stringBuilder.AppendLine($"    Code: {fullStatementText}");
        }
    }
}