using System;
using System.IO;
using System.Runtime.InteropServices;
using DirectoriesComparator.Win32.Interop;

namespace DirectoriesComparator.Win32.FileSystem
{
   public abstract class EntryInfo
   {
      public static EntryInfo GetInfo( string p_FileName )
      {
         WIN32_FILE_ATTRIBUTE_DATA attributeData;
         var result = FileManagementImportedFunctions.GetFileAttributesEx( p_FileName, out attributeData );
         if (!result)
         {
            var win32Error = Marshal.GetLastWin32Error();

            switch (win32Error)
            {
               case Win32Result.ERROR_INVALID_NAME:
                  var win32Exception = new System.ComponentModel.Win32Exception( win32Error );
                  throw new ArgumentException(win32Exception.Message, win32Exception);

               case Win32Result.ERROR_PATH_NOT_FOUND:
               case Win32Result.ERROR_FILE_NOT_FOUND:
                  return NotExistent.Item;

               case Win32Result.ERROR_ACCESS_DENIED:
                  return Inaccessible.Item;

               default:
                  throw new System.ComponentModel.Win32Exception( win32Error );
            }
         }
         return GetInfo(p_FileName, attributeData);
      }

      protected static EntryInfo GetInfo( string p_FileName, WIN32_FILE_ATTRIBUTE_DATA p_AttributeData )
      {
         var fileAttributes = p_AttributeData.dwFileAttributes;

         if ((fileAttributes & FileAttributes.Device) != 0)
         {
            throw new NotSupportedException();
         }

         if ((fileAttributes & FileAttributes.ReparsePoint) != 0)
         {
            return ReparsePointInfo.Create( p_FileName, p_AttributeData );
         }

         if ((fileAttributes & FileAttributes.Directory) != 0)
         {
            return new DirectoryInfo( p_FileName, p_AttributeData );
         }

         return new FileInfo( p_FileName, p_AttributeData );
      }

      public class NotExistent :EntryInfo
      {
         private static NotExistent s_item;

         public static NotExistent Item
         {
            get { return s_item ?? (s_item = new NotExistent()); }
         }

         private NotExistent() { }
      }

      public class Inaccessible :EntryInfo
      {
         private static Inaccessible s_item;

         public static Inaccessible Item
         {
            get { return s_item ?? (s_item = new Inaccessible()); }
         }

         private Inaccessible() { }
      }
   }
}
