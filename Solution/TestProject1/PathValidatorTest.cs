using System.Text;
using DirectoriesComparator.PathUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject1
{
    [TestClass()]
    public class PathValidatorTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for ValidatePath
        ///</summary>
        [TestMethod]
        public void ValidatePathTest()
        {
            using (var file = new System.IO.StreamReader(@"C:\temp\TestData.txt", Encoding.GetEncoding("cp866")))
            {
                string dir;
                while ((dir = file.ReadLine()) != null)
                {
                    var result = Validator.ValidatePath(dir);
                    Assert.IsTrue(result, "error validating path: " + dir);
                }
            }
        }
    }
}