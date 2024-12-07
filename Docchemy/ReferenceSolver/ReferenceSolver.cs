using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Docchemy.Constants;
using Attribute = Docchemy.Constants.Attribute;

namespace Docchemy.ReferenceSolver
{
    public class ReferenceSolver
    {
        public static async Task AnalyzeProject(string projectFilePath, HashSet<string> analyzedProjects)
        {
            if (analyzedProjects.Contains(projectFilePath))
                return;

            analyzedProjects.Add(projectFilePath);

            Console.WriteLine($"Analyzing {projectFilePath}");

            var projectDir = Path.GetDirectoryName(projectFilePath);
            var projectXml = XDocument.Load(projectFilePath);

            var projectReferences = projectXml.Descendants(nameof(Dependency.ProjectReference))
                .Select(pr => Path.GetFullPath(Path.Combine(projectDir, pr.Attribute(nameof(Attribute.Include)).Value)));


            foreach (var reference in projectReferences)
            {
                await AnalyzeProject(reference, analyzedProjects);
            }

        }
    }
}
