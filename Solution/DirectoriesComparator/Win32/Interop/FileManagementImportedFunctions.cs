using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace DirectoriesComparator.Win32.Interop
{
   // ReSharper disable InconsistentNaming
   using HANDLE = IntPtr;

   public static class FileManagementImportedFunctions
   {
      #region GetFileAttributes
      public static bool GetFileAttributesEx( string lpFileName, out WIN32_FILE_ATTRIBUTE_DATA fileData )
      {
         return GetFileAttributesEx( lpFileName, Constants.GET_FILEEX_INFO_LEVELS.GetFileExInfoStandard,
             out fileData );
      }

      [DllImport( "kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode )]
      [return: MarshalAs( UnmanagedType.Bool )]
      static extern bool GetFileAttributesEx(
         [In]string lpFileName,
         [In]Constants.GET_FILEEX_INFO_LEVELS fInfoLevelId,
         [Out]out WIN32_FILE_ATTRIBUTE_DATA fileData );

      [DllImport( "kernel32.dll" )]
      [return: MarshalAs( UnmanagedType.Bool )]
      public static extern bool GetFileAttributesEx(
         [In]string lpFileName,
         [In]Constants.GET_FILEEX_INFO_LEVELS fInfoLevelId,
         [Out]IntPtr lpFileInformation );
      #endregion

      #region GetVolumeNameForVolumeMountPoint
      public static bool GetVolumeNameForVolumeMountPoint( string lpszVolumeMountPoint, out string lpszVolumeName )
      {
         const int MaxVolumeNameLength = 100;
         var sb = new System.Text.StringBuilder( MaxVolumeNameLength );
         var result = GetVolumeNameForVolumeMountPoint( lpszVolumeMountPoint, sb, MaxVolumeNameLength );
         lpszVolumeName = sb.ToString();
         return result;
      }

      [DllImport( "kernel32.dll", SetLastError = true )]
      private static extern bool GetVolumeNameForVolumeMountPoint(
         [In]string lpszVolumeMountPoint,
         [Out] System.Text.StringBuilder lpszVolumeName,
         [In]uint cchBufferLength );
      #endregion

      #region GetWindowsDirectory
      [DllImport( "kernel32.dll", SetLastError = true, CharSet = CharSet.Auto )]
      private static extern Int32 GetWindowsDirectory( [Out]StringBuilder lpBuffer, [In] Int32 uSize );

      public static string GetWindowsDirectory()
      {
         StringBuilder sb = new StringBuilder( Constants.MAX_PATH );
         var len = GetWindowsDirectory( sb, Constants.MAX_PATH );
         if (len == 0)
         {
            var lastWin32Error = Marshal.GetLastWin32Error();
            throw new Win32Exception( lastWin32Error );
         }
         return sb.ToString( 0, len );
      }
      #endregion


      // See the PInvoke definition of DeviceIoControl for EIOControlCode
      [DllImport( "kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode )]
      public static extern bool DeviceIoControl(
         [In]   HANDLE hDevice,
         [In]   EIOControlCode dwIoControlCode,
         //[In] REPARSE_DATA_BUFFER InBuffer,
          [In]  IntPtr InBuffer,
          [In]  int nInBufferSize,
          [Out] IntPtr OutBuffer,
          [In]  int nOutBufferSize,
          [Out] out int pBytesReturned,
          [In, Out]  IntPtr lpOverlapped
      );


      // See pinvoke definition for CreateFile for the various Enums.
      [DllImport( "kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode )]
      public static extern HANDLE CreateFile(
         string lpFileName,
         EFileAccess dwDesiredAccess,
         EFileShare dwShareMode,
         IntPtr lpSecurityAttributes,
         ECreationDisposition dwCreationDisposition,
         EFileAttributes dwFlagsAndAttributes,
         IntPtr hTemplateFile );


      [DllImport( "kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
      [return: MarshalAs( UnmanagedType.Bool )]
      public static extern bool CloseHandle( HANDLE hObject );


      [DllImport( "kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode )]
      public static extern HANDLE FindFirstFile(
            [In] string lpFileName,
            [Out] out WIN32_FIND_DATA lpFindFileData);

      [DllImport( "kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode )]
      [return: MarshalAs( UnmanagedType.Bool )]
      public static extern bool FindNextFile(
            [In] HANDLE hFindFile,
            [Out] out WIN32_FIND_DATA lpFindFileData );

      [DllImport( "kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi )]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool FindClose([In, Out] HANDLE hFindFile);
   }
   // ReSharper restore InconsistentNaming
}
