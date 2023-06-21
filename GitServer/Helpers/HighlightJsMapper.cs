using System.Collections.Generic;

namespace GitServer.Helpers;

public class ExtensionHljsClassItem
{
    public string Extension;
    public string HljsClass;
    public string Description;
}

public interface IHighlightJsMapper
{
    ExtensionHljsClassItem Map(string fileExtension);
}

public class HighlightJsMapper : IHighlightJsMapper
{
    private Dictionary<string, ExtensionHljsClassItem> _dictionary = new();

    public HighlightJsMapper()
    {
        AddItem(".sh", "bash", "Bash shell script");
        AddItem(".cs", "csharp", "CSharp source code file");
        AddItem(".cpp", "cpp", "C++ source code file");
        AddItem(".h", "cpp", "C, C++, and Objective-C header file");
        AddItem(".c", "cpp", "C and C++ source code file");
        AddItem(".css", "css", "Cascading Style Sheet file");
        AddItem(".scss", "scss", "Scss Cascading Style Sheet file");
        AddItem(".sass", "scss", "Syntactically Awesome StyleSheets file");
        AddItem(".coffee", "coffee", "CoffeeScript JavaScript file");
        AddItem(".diff", "diff", "Patch file");
        AddItem(".patch", "diff", "Patch file");
        AddItem(".html", "html", "Hypertext Markup Language file");
        AddItem(".htm", "htm", "Hypertext Markup Language file");
        AddItem(".xml", "xml", "Extensible Markup Language file");
        AddItem(".haml", "haml", "HTML abstraction markup language file");
        AddItem(".ini", "ini", "Configuration file");
        AddItem(".json", "json", "JavaScript Object Notation file");
        AddItem(".java", "java", "Java Source Code file");
        AddItem(".jsp", "jsp", "Java Server Page");
        AddItem(".js", "javascript", "JavaScript file");
        AddItem(".makefile", "makefile", "Makefile");
        AddItem(".md", "markdown", "Markdown Documentation file");
        AddItem(".conf", "nginxconf", "Unix Configuration file");
        AddItem(".m", "objectivec", "Objective-C source code file");
        AddItem(".php", "php", "PHP Source Code file");
        AddItem(".pl", "perl", "Perl script");
        AddItem(".py", "python", "Python script");
        AddItem(".rb", "ruby", "Ruby Source Code file");
        AddItem(".sql", "sql", "Structured Query Language data file");
        AddItem(".pas", "pas", "Delphi Unit Source file");
        AddItem(".dfm", "dfm", "Delphi Form file");
        AddItem(".dpr", "dpr", "Delphi Project");
        AddItem(".kt", "kotlin", "Kotlin Source Code file");
    }

    private void AddItem(string fileExtension, string hljsClass, string description)
    {
        _dictionary.Add(fileExtension, new ExtensionHljsClassItem
        {
            Extension = fileExtension,
            HljsClass = hljsClass,
            Description = description
        });
    }

    public ExtensionHljsClassItem Map(string fileExtension)
    {
        ExtensionHljsClassItem extClassItem;
        _dictionary.TryGetValue(fileExtension, out extClassItem);
        return extClassItem;
    }
}
