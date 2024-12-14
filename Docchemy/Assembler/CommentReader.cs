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
        public static async Task<string> ReadCommentAsync(string csFilePath)
        {
            var code = await File.ReadAllTextAsync(csFilePath);

            var tree = CSharpSyntaxTree.ParseText(code);
            var root = await tree.GetRootAsync();

            var retriever = new Retriever();
            var analyzerResult = retriever.GetClassesInfo(csFilePath);

            // analyzing comments and classes
            //var comments = root.DescendantTrivia()
            //    .Where(trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) ||
            //                     trivia.IsKind(SyntaxKind.MultiLineCommentTrivia))
            //    .Select(trivia => trivia.ToString()).ToList();

            //var classes = root.DescendantNodes()
            //    .OfType<ClassDeclarationSyntax>()
            //    .Select(t => t.Identifier.Text).ToList();

            //StringBuilder commentsByClassesBuilder = new StringBuilder();

            //if (comments.Count > 0 || classes.Count > 0)
            //{

            //    Console.WriteLine("Classes");
            //    foreach (var className in classes)
            //    {
            //        Console.WriteLine(className);
            //        commentsByClassesBuilder.AppendLine(className);
            //    }

            //    commentsByClassesBuilder.AppendLine("--------------------");

            //    Console.WriteLine("Comments");
            //    foreach (var comment in comments)
            //    {
            //        Console.WriteLine(comment);
            //        commentsByClassesBuilder.AppendLine(comment);
            //    }

            //    commentsByClassesBuilder.AppendLine("--------------------");
            //}
            
            //Console.WriteLine(new string('-', 40));

            //return commentsByClassesBuilder.ToString();
            return analyzerResult;
        }
    }
}
