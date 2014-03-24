using DirectoriesComparator;
using DirectoriesComparator.CommandLine;
using DirectoriesComparator.CommandLine.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestProject1
{
  /// <summary>
   ///This is a test class for ParametersSplitter and is intended
   ///to contain all ParametersSplitterTest Unit Tests
   ///</summary>
   [TestClass()]
   public class ParametersSplitterTest
   {
      private const string source12 = CommandLineParser.SourcePrefix + TestConsts.sourceDir1 + " " + TestConsts.sourceDir2;
      private const string target12 = CommandLineParser.TargetPrefix + TestConsts.targetDir1 + " " + TestConsts.targetDir2;

      private const string source3 = CommandLineParser.SourcePrefix + TestConsts.sourceDir3;
      private const string target3 = CommandLineParser.TargetPrefix + TestConsts.targetDir3;

      private const string args = TestConsts.arg0Quoted + " " + TestConsts.param1 + TestConsts.param2 + " " + source12 + " " + target12 + target3 + source3;


      [TestMethod]
      public void SplitStringTest()
      {
         bool quotesOpened;
         var actualResult = ParametersSplitter.SplitString( args, out quotesOpened );

         Assert.IsFalse( quotesOpened );


         var expectedResult = new List<string>();
         expectedResult.Add( TestConsts.arg0 );
         expectedResult.Add( TestConsts.param1 );
         expectedResult.Add( TestConsts.param2 );

         expectedResult.Add( CommandLineParser.SourcePrefix );
         expectedResult.Add( TestConsts.sourceDir1 );
         expectedResult.Add( TestConsts.sourceDir2 );

         expectedResult.Add( CommandLineParser.TargetPrefix );
         expectedResult.Add( TestConsts.targetDir1 );
         expectedResult.Add( TestConsts.targetDir2 );

         expectedResult.Add( CommandLineParser.SourcePrefix );
         expectedResult.Add( TestConsts.sourceDir3 );

         expectedResult.Add( CommandLineParser.TargetPrefix );
         expectedResult.Add( TestConsts.targetDir3 );

         
         var equal = Utils.CompareIgnoringOrder(expectedResult, actualResult);
         if (!equal)
         {
            Console.WriteLine( args );

            Console.WriteLine( "================================================================" );
            for (int i = 0; i < actualResult.Count; i++)
            {
               var wordActual = actualResult[i];
               var wordExpected = expectedResult[i];
               Console.WriteLine( wordActual );
               Console.WriteLine( wordExpected );
            }
         }
         Assert.IsTrue( equal );
      }
   }
}
