namespace FileToString.UnitTests
{
    using Xunit;

    public class ClassGenerator
    {
        [Fact]
        public void TestGeneratePublicSimpleString()
        {
            FileToString.ClassGenerator sourceGenerator = new FileToString.ClassGenerator("TestNamespace", "public", "TestStrings", "TestField");
            string result = sourceGenerator.Generate("The test works!");
            Assert.Equal(@"namespace TestNamespace
{
    public static partial class TestStrings
    {
        public static string TestField = @""The test works!"";
    }
}
", result);
        }
    }
}