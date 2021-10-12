namespace FileToString
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Text;
    using System.IO;
    using System.Text;
    using System.Threading;

    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            string defaultNamespaceName = null;
            string defaultClassName = "StringLiterals";
            string defaultVisibility = "public";

            foreach (AdditionalText additionalText in context.AdditionalFiles)
            {
                string filename = Path.GetFileName(additionalText.Path);
                if (string.Equals(filename, "FileToString.config"))
                {
                    SourceText text = additionalText.GetText(context.CancellationToken);
                    if (text != null)
                    {
                        string[] lines = text.ToString().Split(new char[] { '\r','\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                        foreach (var line in lines)
                        {
                            if (line.StartsWith("namespace="))
                            {
                                defaultNamespaceName = line.Substring("namespace=".Length);
                            }
                            if (line.StartsWith("className="))
                            {
                                defaultClassName = line.Substring("className=".Length);
                            }
                            if (line.StartsWith("visibility="))
                            {
                                defaultVisibility = line.Substring("visibility=".Length);
                            }
                        }
                    }
                }
            }
            foreach (AdditionalText additionalText in context.AdditionalFiles)
            {
                string filename = Path.GetFileName(additionalText.Path);
                if (Path.GetExtension(filename) == ".string")
                {
                    string namespaceName = null;
                    string visibility = null;
                    string className = null;
                    string fieldName = null;

                    AnalyzerConfigOptions options = context.AnalyzerConfigOptions.GetOptions(additionalText);

                    options.TryGetValue("build_metadata.additionalfiles.Namespace", out namespaceName);
                    options.TryGetValue("build_metadata.additionalfiles.Visibility", out visibility);
                    options.TryGetValue("build_metadata.additionalfiles.ClassName", out className);
                    options.TryGetValue("build_metadata.additionalfiles.FieldName", out fieldName);

                    if (string.IsNullOrEmpty(namespaceName))
                    {
                        namespaceName = defaultNamespaceName;
                    }

                    if (string.IsNullOrEmpty(visibility))
                    {
                        visibility = defaultVisibility;
                    }

                    if (string.IsNullOrEmpty(className))
                    {
                        className = defaultClassName;
                    }

                    if (string.IsNullOrEmpty(fieldName))
                    {
                        fieldName = Path.GetFileNameWithoutExtension(filename);
                    }

                    SourceText text = additionalText.GetText(context.CancellationToken);

                    if (text != null)
                    {
                        string source = new ClassGenerator(namespaceName, visibility, className, fieldName).Generate(text.ToString());
                        context.AddSource(Path.GetFileNameWithoutExtension(filename) + ".cs", source);
                    }
                }
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}