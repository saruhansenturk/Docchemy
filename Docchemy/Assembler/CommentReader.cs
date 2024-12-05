using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Docchemy.Assembler
{
    public class CommentReader
    {
        public CommentReader()
        {
            var code = @"
            // This is a sample comment
            public class SampleClass 
            {
                // Method to add two numbers
                public int Add(int a, int b) 
                {
                    return a + b; // Inline comment
                }
            }";

            var tree = CSharpSyntaxTree.ParseText(code);

            var root = tree.GetRoot();

            var methodComments = root.DescendantTrivia()
                .OfType<MethodDeclarationSyntax>() // get the methods
                .SelectMany(method => method.DescendantTrivia()
                    .Where(trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
                    .Select(trivia => new
                    {
                        MethodName = method.Identifier.ValueText, // get the method name
                        Comment = trivia.ToString(),
                        LineNumber = trivia.GetLocation().GetLineSpan().StartLinePosition.Line + 1
                    }));

            foreach (var comment in methodComments)
            {
                Console.WriteLine(comment.MethodName);
                Console.WriteLine(comment.Comment);
                Console.WriteLine(comment.LineNumber);
                Console.WriteLine("-------------------");
            }

        }
    }
}
