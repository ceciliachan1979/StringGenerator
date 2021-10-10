namespace FileToString
{
    using System.Text;

    public class ClassGenerator
    {
        private string namespaceName;
        private string visibility;
        private string className;
        private string fieldName;
        private StringBuilder builder;
        private int indent;

        public ClassGenerator(string namespaceName, string visibility, string className, string fieldName)
        {
            this.namespaceName = namespaceName;
            this.visibility = visibility;
            this.className = className;
            this.fieldName = fieldName;
            this.builder = new StringBuilder();
            this.indent = 0;
        }

        public string Generate(string content)
        {
            if (!string.IsNullOrEmpty(namespaceName))
            {
                this.AppendLine($"namespace {namespaceName}");
                this.BeginBlock();
            }
            this.GenerateClass(content);
            if (!string.IsNullOrEmpty(namespaceName))
            {
                this.EndBlock();
            }
            return this.builder.ToString();
        }

        private void EndBlock()
        {
            this.indent--;
            this.AppendLine("}");
        }

        private void BeginBlock()
        {
            this.AppendLine("{");
            this.indent++;
        }

        private void GenerateClass(string content)
        {
            // Hmm, public?
            this.AppendLine($"{visibility} static partial class {className}");
            this.BeginBlock();
            this.AppendLine($"{visibility} static string {fieldName} = @\"{content.Replace("\"", "\"\"")}\";");
            this.EndBlock();
        }

        private void AppendLine(string s)
        {
            for (int i = 0; i < this.indent; i++)
            {
                this.builder.Append("    ");
            }
            this.builder.AppendLine(s);
        }
    }
}
