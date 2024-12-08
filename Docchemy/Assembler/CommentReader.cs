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
        public static async Task ReadCommentAsync(string csFilePath)
        {
            var code = await File.ReadAllTextAsync(csFilePath);

            var tree = CSharpSyntaxTree.ParseText(code);
            var root = await tree.GetRootAsync();

            // analyzing comments and classes
            var comments = root.DescendantTrivia()
                .Where(trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) ||
                                 trivia.IsKind(SyntaxKind.MultiLineCommentTrivia))
                .Select(trivia => trivia.ToString());

            var classes = root.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .Select(t => t.Identifier.Text);

            Console.WriteLine("Comments");
            foreach (var comment in comments)
            {
                Console.WriteLine(comment);
            }

            Console.WriteLine("Classes");
            foreach (var className in classes)
            {
                Console.WriteLine(className);
            }

            Console.WriteLine(new string('-', 40));


        }
    }
}
