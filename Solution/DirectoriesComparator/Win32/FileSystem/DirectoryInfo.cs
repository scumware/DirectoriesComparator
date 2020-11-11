using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using DirectoriesComparator.Win32.Interop;

namespace DirectoriesComparator.Win32.FileSystem
{
   public class DirectoryInfo :NormalEntryInfo
   {
#pragma warning disable 108,114
      public bool Inaccessible { get; protected set; }
#pragma warning restore 108,114

      public DirectoryInfo( string p_FileName, WIN32_FILE_ATTRIBUTE_DATA p_Win32FileAttributeData )
         : base( p_FileName, p_Win32FileAttributeData )
      {
      }

      public ICollection<NormalEntryInfo> GetEntries()
      {
         var findHandle = (IntPtr)Constants.INVALID_HANDLE_VALUE;
         var result = new List<NormalEntryInfo>();
         try
         {
            WIN32_FIND_DATA findData;

            var path = this.FileName + "\\*";

            findHandle = FileManagementImportedFunctions.FindFirstFile( path, out findData );
            int win32Error = Marshal.GetLastWin32Error();

            var retVal = findHandle != (IntPtr)Constants.INVALID_HANDLE_VALUE;
            var num = 0;
            while (retVal)
            {
               if (num >= 2)
               {
                  var entry = ParseFinded(findData);
                  result.Add(entry);
               }
               else
               {
                  ++num;
                  if ((findData.cFileName != ".") && (findData.cFileName != ".."))
                  {
                     var entry = ParseFinded(findData);
                     result.Add(entry);
                  }
               }

               retVal = FileManagementImportedFunctions.FindNextFile(findHandle, out findData);
               win32Error = Marshal.GetLastWin32Error();
            }

            switch (win32Error)
            {
                case Win32Result.ERROR_NO_MORE_FILES:
                case Win32Result.ERROR_FILE_NOT_FOUND:
                    break;

                default:
                    if (win32Error == Win32Result.ERROR_ACCESS_DENIED)
                        Inaccessible = true;

                    if (win32Error == Win32Result.ERROR_PATH_NOT_FOUND)
                        if (path.Length >= Constants.MAX_PATH)
                            Inaccessible = true;


                    throw new Win32Exception(win32Error);
            }
         }
         finally
         {
            if (findHandle != (IntPtr)Constants.INVALID_HANDLE_VALUE)
            {
               var retVal = FileManagementImportedFunctions.FindClose( findHandle );
               if (!retVal)
               {
                  var win32Error = Marshal.GetLastWin32Error();
                  throw new Win32Exception( win32Error );
               }
            }
         }

         return result;
      }

      private NormalEntryInfo ParseFinded( WIN32_FIND_DATA p_FindData )
      {
         var fullName = this.FileName + "\\" + p_FindData.cFileName;

         var result = EntryInfo.GetInfo( fullName, p_FindData.FILE_ATTRIBUTE_DATA );
         return (NormalEntryInfo)result;
      }

      public ICollection<FileInfo> GetAllFiles( bool p_IgnorReparsePoints, bool p_IgnoreInaccessible)
      {
         ICollection<FileInfo> result;
         if (p_IgnorReparsePoints)
         {
            result = GetAllFiles(p_IgnoreInaccessible);
         }
         else
         {
            var walker = new ReparsePointWalker();
            walker.IgnoreInaccessible = p_IgnoreInaccessible;

            walker.Walk( this);
            result = walker.FindedFiles;
         }
         return result;
      }

      public ICollection<FileInfo> GetAllFiles(bool p_IgnoreInaccessible)
      {
         var result = new List<FileInfo>();

         ICollection<NormalEntryInfo> entries;
         try
         {
            entries = GetEntries();
         }
         catch (Win32Exception)
         {
            if (p_IgnoreInaccessible && Inaccessible)
            {
               return result;
            }
            throw;
         }

         foreach (var entry in entries)
         {
            if (entry is FileInfo)
            {
               ((List<FileInfo>) result).Add((FileInfo) entry);
            }
            else if (entry is DirectoryInfo)
            {
               var fileInfos = ((DirectoryInfo) entry).GetAllFiles(p_IgnoreInaccessible);
               ((List<FileInfo>) result).AddRange(fileInfos);
            }
         }
         return result;
      }

      internal class ReparsePointWalker
      {
         private readonly ICollection<FileInfo> m_FindedFiles = new HashSet<FileInfo>();
         private readonly ICollection<DirectoryInfo> m_VisitedDirectory = new HashSet<DirectoryInfo>();
         public bool IgnoreInaccessible;

         public ICollection<FileInfo> FindedFiles
         {
            get { return m_FindedFiles; }
         }

         public IEnumerable<DirectoryInfo> VisitedDirectory
         {
            get { return m_VisitedDirectory; }
         }

         public void Walk(DirectoryInfo p_DirectoryInfo)
         {
            m_VisitedDirectory.Add(p_DirectoryInfo);

            ICollection<NormalEntryInfo> entries;
            try
            {
               entries = p_DirectoryInfo.GetEntries();
            }
            catch (Win32Exception)
            {
               if (IgnoreInaccessible && p_DirectoryInfo.Inaccessible)
               {
                  return;
               }
               throw;
            }

            foreach (var entry in entries)
            {
               if (entry is FileInfo)
               {
                  FileFinded((FileInfo) entry);
               }
               else if (entry is DirectoryInfo)
               {
                  DirectoryFinded( (DirectoryInfo)entry);
               }
               else if (entry is ReparsePointInfo)
               {
                  ReparsePointFinded((ReparsePointInfo) entry);
               }
            }
         }

         private void DirectoryFinded(DirectoryInfo p_DirectoryInfo)
         {
            if (m_VisitedDirectory.Contains(p_DirectoryInfo))
            {
               return;
            }
            Walk(p_DirectoryInfo);
         }

         private void FileFinded(FileInfo p_FileInfo)
         {
            if (m_FindedFiles.Contains(p_FileInfo))
            {
               return;
            }
            m_FindedFiles.Add(p_FileInfo);
         }

         private void ReparsePointFinded(ReparsePointInfo p_ReparsePointInfo)
         {
            if (p_ReparsePointInfo.Target is FileInfo)
               FileFinded((FileInfo) p_ReparsePointInfo.Target);
            else if (p_ReparsePointInfo.Target is DirectoryInfo)
               DirectoryFinded((DirectoryInfo) p_ReparsePointInfo.Target);
         }
      }
   }
}