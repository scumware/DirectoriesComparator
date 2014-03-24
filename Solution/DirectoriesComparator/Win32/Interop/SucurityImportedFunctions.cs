using System;
using System.Runtime.InteropServices;

namespace DirectoriesComparator.Win32.Interop
{
   public static class SucurityImportedFunctions
   {
      [DllImport( "advapi32.dll", SetLastError = true )]
      [return: MarshalAs( UnmanagedType.Bool )]
      public static extern bool AdjustTokenPrivileges(
            IntPtr TokenHandle,
            [MarshalAs( UnmanagedType.Bool )]
                  bool DisableAllPrivileges,
            ref TOKEN_PRIVILEGES NewState,
            Int32 BufferLengthInBytes,
            ref TOKEN_PRIVILEGES PreviousState,
            out UInt32 ReturnLengthInBytes );

      [DllImport( "advapi32.dll", SetLastError = true )]
      [return: MarshalAs( UnmanagedType.Bool )]
      public static extern bool AdjustTokenPrivileges(
            IntPtr TokenHandle,
            [MarshalAs( UnmanagedType.Bool )]
                  bool DisableAllPrivileges,
            ref TOKEN_PRIVILEGES NewState,
            Int32 Zero,
            IntPtr Null1,
            IntPtr Null2 );

      [DllImport( "advapi32.dll", SetLastError = true )]
      [return: MarshalAs( UnmanagedType.Bool )]
      public static extern bool OpenProcessToken( IntPtr ProcessHandle,
                                                  UInt32 DesiredAccess, out IntPtr TokenHandle );

      [DllImport( "advapi32.dll", SetLastError = true, CharSet = CharSet.Auto )]
      [return: MarshalAs( UnmanagedType.Bool )]
      public static extern bool LookupPrivilegeValue( string lpSystemName, string lpName, out LUID lpLuid );
   }
}