using System;
using System.Linq;
using DirectoriesComparator.CommandLine.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1
{
   [TestClass]
   public class ParametersParsingTest
   {
      [TestMethod]
      public void TestSimple()
      {
         var result = CommandLineParser.ParseCommandLine( TestConsts.arg0Quoted );

         Assert.AreEqual( 0, result.SourcesList.Count );
         Assert.AreEqual( 0, result.TargetsList.Count );

         Assert.IsTrue( result.Empty );
      }


      [TestMethod]
      public void TestTypicalUsage()
      {
         Console.WriteLine( TestConsts.TypicalUsageArgs );

         var result = CommandLineParser.ParseCommandLine( TestConsts.TypicalUsageArgs );

         Assert.AreEqual( 1, result.SourcesList.Count );
         Assert.AreEqual( 1, result.TargetsList.Count );

         Assert.AreEqual( result.SourcesList.First(), TestConsts.sourceDir1 );
         Assert.AreEqual( result.TargetsList.First(), TestConsts.targetDir1 );
      }


      [TestMethod]
      public void TestComplicatedUsage()
      {
         const string source12 = CommandLineParser.SourcePrefix + TestConsts.sourceDir1 + " " + TestConsts.sourceDir2;
         const string target12 = CommandLineParser.TargetPrefix + TestConsts.targetDir1 + " " + TestConsts.targetDir2;

         const string source3 = CommandLineParser.SourcePrefix + TestConsts.sourceDir3;
         const string target3 = CommandLineParser.TargetPrefix + TestConsts.targetDir3;

         const string args = TestConsts.arg0Quoted + " " + source12 + " " + target12 + target3 + source3;
         Console.WriteLine( args );

         var result = CommandLineParser.ParseCommandLine( args );

         Assert.AreEqual( 3, result.SourcesList.Count );
         Assert.AreEqual( 3, result.TargetsList.Count );

         Assert.IsTrue( result.SourcesList.Contains( TestConsts.sourceDir1 ) );
         Assert.IsTrue( result.SourcesList.Contains( TestConsts.sourceDir2 ) );
         Assert.IsTrue( result.SourcesList.Contains( TestConsts.sourceDir3 ) );

         Assert.IsTrue( result.TargetsList.Contains( TestConsts.targetDir1 ) );
         Assert.IsTrue( result.TargetsList.Contains( TestConsts.targetDir2 ) );
         Assert.IsTrue( result.TargetsList.Contains( TestConsts.targetDir3 ) );
      }

      [TestMethod]
      public void TestIncorrectUsage()
      {
         const string source12 = CommandLineParser.SourcePrefix + TestConsts.sourceDir1 + TestConsts.sourceDir2;
         const string args = TestConsts.arg0Quoted + " " + source12;
         Console.WriteLine(args);

         var result = CommandLineParser.ParseCommandLine( args );

         Assert.IsTrue( result.Incorrect );
      }
   }
}
