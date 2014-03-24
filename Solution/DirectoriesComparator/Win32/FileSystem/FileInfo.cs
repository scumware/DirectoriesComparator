using DirectoriesComparator.Win32.Interop;

namespace DirectoriesComparator.Win32.FileSystem
{
   public class FileInfo :NormalEntryInfo
   {
      public long Size
      {
         get
         {
            var result = Win32FileAttributeData.nFileSizeHigh << (sizeof (uint)*8);
            result |= Win32FileAttributeData.nFileSizeLow;
            return result;
         }
      }

      public FileInfo(string p_FileName, WIN32_FILE_ATTRIBUTE_DATA p_Win32FileAttributeData)
         : base( p_FileName, p_Win32FileAttributeData )
      {
      }
   }
}