namespace Docchemy.CodeElements;

public class ClassInfo
{
    public string ClassName { get; set; }
    public string Summary { get; set; }
    public List<string> Fields { get; set; } = new List<string>();
}