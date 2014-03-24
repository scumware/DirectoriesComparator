using System.ComponentModel;
using DirectoriesComparator.Win32.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestProject1
{
   [TestClass()]
   public class FileManagementFunctionsTest
   {
      [TestInitialize()]
      public void InitializeTest()
      {
         try
         {
            CurrentProcess.AdjustPrivileges( SecurityEntiryNames.SE_BACKUP_NAME, PrivilegeAction.Enable );
         }
         catch (Win32Exception)
         {
           Assert.Inconclusive( "Для успешного прохождения этого теста процессу должны быть доступны привелегии резервного копирования (SeBackupPrivilagies)" );
         }
      }

      [TestMethod()]
      public void GetJunctionInfoTest()
      {
         ReparsePointInfo actual;
         actual = ReparsePointInfo.GetReparsePointInfo( @"C:\Users\All Users" );
         Console.WriteLine( actual.PrintName );
         Console.WriteLine( actual.SubstituteName );

         actual = ReparsePointInfo.GetReparsePointInfo( @"C:\Users\Default User" );
         Console.WriteLine( actual.PrintName );
         Console.WriteLine( actual.SubstituteName );

         actual = ReparsePointInfo.GetReparsePointInfo( @"C:\temp\driveP" );
         Console.WriteLine( actual.PrintName );
         Console.WriteLine( actual.SubstituteName );
         
         Assert.Inconclusive( "Verify the correctness of this test method." );
      }
   }
}
