using System;
using System.IO;
using DirectoriesComparator.Win32.Interop;

namespace DirectoriesComparator.Win32.FileSystem
{
   public abstract class NormalEntryInfo :EntryInfo
   {
      private readonly string m_FileName;
      private readonly WIN32_FILE_ATTRIBUTE_DATA m_Win32FileAttributeData;


      public string FileName
      {
         get { return m_FileName; }
      }

      public FileAttributes Attributes
      {
         get { return m_Win32FileAttributeData.dwFileAttributes; }
      }

      public DateTime CreationTime
      {
         get
         {
            return m_Win32FileAttributeData.ftCreationTime.ToDateTime();
         } 
      }

      public DateTime LastAccessTime
      {
         get
         {
            return m_Win32FileAttributeData.ftLastAccessTime.ToDateTime();
         }
      }

      public DateTime LastWriteTime
      {
         get
         {
            return m_Win32FileAttributeData.ftLastWriteTime.ToDateTime();
         }
      }

      public WIN32_FILE_ATTRIBUTE_DATA Win32FileAttributeData
      {
         get { return m_Win32FileAttributeData; }
      }

      public NormalEntryInfo( string p_FileName, WIN32_FILE_ATTRIBUTE_DATA p_Win32FileAttributeData )
      {
         m_FileName = p_FileName;
         m_Win32FileAttributeData = p_Win32FileAttributeData;
      }
   }
}
