using System.Transactions;

namespace Docchemy.Generator.DocumenterService;

public interface IDocumenter
{
    Task<Documentation> DocumantateAsync(Dictionary<string, List<string>> analyzedClasses);

}