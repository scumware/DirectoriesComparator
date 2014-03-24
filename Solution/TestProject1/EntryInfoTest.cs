using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using DirectoriesComparator.Win32.FileSystem;
using DirectoriesComparator.Win32.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DirectoryInfo = DirectoriesComparator.Win32.FileSystem.DirectoryInfo;
using FileInfo = DirectoriesComparator.Win32.FileSystem.FileInfo;
using ReparsePointInfo = DirectoriesComparator.Win32.FileSystem.ReparsePointInfo;

namespace TestProject1
{
   [TestClass()]
   public class EntryInfoTest
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
            m_BackupPrivilegeInaccessible = true;
         }
      }

      private bool m_BackupPrivilegeInaccessible;

      [TestMethod()]
      public void CreationFileSystemEntryInfoTest()
      {
         var tempFileName = Path.GetTempFileName();
         var actual = EntryInfo.GetInfo( tempFileName );
         Assert.IsTrue( actual is DirectoriesComparator.Win32.FileSystem.FileInfo );

         File.Delete( tempFileName );
         var notExistentFileName = tempFileName;

         actual = EntryInfo.GetInfo( notExistentFileName );
         Assert.AreEqual( actual, EntryInfo.NotExistent.Item );

         string dirName = Path.GetDirectoryName( tempFileName );
         actual = EntryInfo.GetInfo( dirName );
         Assert.IsTrue( actual is DirectoriesComparator.Win32.FileSystem.DirectoryInfo );
      }

      [TestMethod()]
      public void InvestigationInvalidPathTest()
      {
         bool gottenErrorInvalidName = false;
         try
         {
            EntryInfo.GetInfo(DirectoriesComparator.PathUtils.Validator.SpecialChars);
         }
         catch (ArgumentException ex)
         {
            gottenErrorInvalidName = (ex.InnerException is System.ComponentModel.Win32Exception) &&
                                     (((System.ComponentModel.Win32Exception) ex.InnerException).NativeErrorCode ==
                                      Win32Result.ERROR_INVALID_NAME);
         }
         Assert.IsTrue(gottenErrorInvalidName);
      }

      [TestMethod()]
      public void InvestigationReparsePointTest()
      {
         if (m_BackupPrivilegeInaccessible)
         {
            Assert.Inconclusive( "Для успешного прохождения этого теста процессу должны быть доступны привелегии резервного копирования (SeBackupPrivilagies)" );
         }

         EntryInfo actual = EntryInfo.GetInfo( @"C:\ProgramData\Application Data" );
         var reparsePointInfo = actual as ReparsePointInfo;

         Assert.AreNotEqual( reparsePointInfo, null );
         var dirInfo = reparsePointInfo.Target as DirectoriesComparator.Win32.FileSystem.DirectoryInfo;
         Assert.AreNotEqual(dirInfo, null);
         Console.WriteLine(dirInfo.FileName.ToLower());
         Console.WriteLine(@"C:\ProgramData".ToLower());
         Assert.IsTrue(string.Equals(dirInfo.FileName.ToLower(), @"C:\ProgramData".ToLower()));
      }

      [TestMethod()]
      public void InvestigationUncommonReparsePointTest()
      {
         if (m_BackupPrivilegeInaccessible)
         {
            Assert.Inconclusive( "Для успешного прохождения этого теста процессу должны быть доступны привелегии резервного копирования (SeBackupPrivilagies)" );
         }

         EntryInfo actual = EntryInfo.GetInfo( @"C:\temp\driveP" );
         Assert.IsTrue( actual is ReparsePointInfo );
         var reparsePointInfo = (ReparsePointInfo)actual;
         var dirInfo = reparsePointInfo.Target as DirectoriesComparator.Win32.FileSystem.DirectoryInfo;
         Assert.AreNotEqual( dirInfo, null );
         Console.WriteLine( dirInfo.FileName.ToLower() );
      }

      [TestMethod()]
      public void InvestigationDirectory()
      {
         var tempPath = Path.GetTempPath();
         var entryInfo = EntryInfo.GetInfo( tempPath );

         var dirInfo = entryInfo as DirectoriesComparator.Win32.FileSystem.DirectoryInfo;
         Assert.AreNotEqual( dirInfo, null );

         var entries = dirInfo.GetEntries();
         Assert.AreNotEqual(entries.Count, 0);
      }

      [TestMethod()]
      public  void InvestigationSystemDirectory()
      {
         EntryInfo entryInfo;
         DirectoryInfo dirInfo;
         var windowsDirectory = FileManagementImportedFunctions.GetWindowsDirectory();

         entryInfo = EntryInfo.GetInfo(windowsDirectory);
         dirInfo = entryInfo as DirectoriesComparator.Win32.FileSystem.DirectoryInfo;
         Assert.AreNotEqual(dirInfo, null);

         bool exceptionThrown = false;
         ICollection<FileInfo> allFiles;
         try
         {
            CurrentProcess.AdjustPrivileges( SecurityEntiryNames.SE_BACKUP_NAME, PrivilegeAction.Disable);
            allFiles = dirInfo.GetAllFiles(false);
            Console.WriteLine( "!!!exception was'nt thrown" );
         }
         catch (Win32Exception)
         {
            exceptionThrown = true;
         }
         Assert.IsTrue(exceptionThrown);
         Console.WriteLine("exception was thrown");

         allFiles = dirInfo.GetAllFiles(false, true);

         Assert.IsTrue(allFiles.Count > 0);
         Console.WriteLine("files count ({0}) greater than zero", allFiles.Count);
         Console.WriteLine("First filest:");
         var num = 0;
         foreach (var fileInfo in allFiles)
         {
            Console.WriteLine(fileInfo.FileName);
            if (num > 3)
            {
               break;
            }
            ++num;
         }
      }
   }
}
