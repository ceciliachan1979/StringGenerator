namespace FileToString.UnitTests
{
    using Xunit;

    public class ClassGeneratorTests
    {
        [Fact]
        public void TestGeneratePublicSimpleString()
        {
            ClassGenerator classGenerator = new ClassGenerator("TestNamespace", "public", "TestStrings", "TestField");
            string result = classGenerator.Generate("The test works!");
            Assert.Equal(UnitTestSourceTexts.Case1Test, result);
        }
    }
}