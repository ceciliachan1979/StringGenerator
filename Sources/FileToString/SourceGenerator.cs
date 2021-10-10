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

                    if (string.IsNullOrEmpty(visibility))
                    {
                        visibility = "public";
                    }

                    if (string.IsNullOrEmpty(className))
                    {
                        className = "StringLiterals";
                    }

                    if (string.IsNullOrEmpty(fieldName))
                    {
                        fieldName = Path.GetFileNameWithoutExtension(filename);
                    }

                    SourceText text = additionalText.GetText(CancellationToken.None);

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