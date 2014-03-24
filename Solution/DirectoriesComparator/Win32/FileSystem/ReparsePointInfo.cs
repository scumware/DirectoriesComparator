using System;
using DirectoriesComparator.Win32.Interop;

namespace DirectoriesComparator.Win32.FileSystem
{
   public class ReparsePointInfo :NormalEntryInfo
   {
      public static ReparsePointInfo Create(string p_FileName, WIN32_FILE_ATTRIBUTE_DATA p_Win32FileAttributeData)
      {
         var reparsePointInfo = Interop.ReparsePointInfo.GetReparsePointInfo( p_FileName );
         var result = new ReparsePointInfo(p_FileName, p_Win32FileAttributeData);

         result.Target = EntryInfo.GetInfo( reparsePointInfo.PrintName );
         return result;
      }

      public EntryInfo Target { get; private set; }

      protected ReparsePointInfo( string p_FileName, WIN32_FILE_ATTRIBUTE_DATA p_Win32FileAttributeData )
         : base( p_FileName, p_Win32FileAttributeData )
      {
      }
   }
}