using System;
using System.IO;
using System.Runtime.InteropServices;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;
// ReSharper disable InconsistentNaming

namespace DirectoriesComparator.Win32.Interop
{
   [StructLayout( LayoutKind.Sequential )]
   public struct WIN32_FILE_ATTRIBUTE_DATA
   {
      public FileAttributes dwFileAttributes;
      public FILETIME ftCreationTime;
      public FILETIME ftLastAccessTime;
      public FILETIME ftLastWriteTime;
      public uint nFileSizeHigh;
      public uint nFileSizeLow;
   }

   // The CharSet must match the CharSet of the corresponding PInvoke signature
   [StructLayout( LayoutKind.Sequential, CharSet = CharSet.Unicode )]
   public struct WIN32_FIND_DATA
   {
      /*
      public uint dwFileAttributes;
      public FILETIME ftCreationTime;
      public FILETIME ftLastAccessTime;
      public FILETIME ftLastWriteTime;
      public uint nFileSizeHigh;
      public uint nFileSizeLow;
      */
      public WIN32_FILE_ATTRIBUTE_DATA FILE_ATTRIBUTE_DATA;
      public uint dwReserved0;
      public uint dwReserved1;

      [MarshalAs( UnmanagedType.ByValTStr, SizeConst = 260 )]
      public string cFileName;

      [MarshalAs( UnmanagedType.ByValTStr, SizeConst = 14 )]
      public string cAlternateFileName;
   }


   [StructLayout( LayoutKind.Sequential )]
   public struct LUID
   {
      public uint LowPart;
      public int HighPart;
   }

   [StructLayout( LayoutKind.Sequential )]
   public struct LUID_AND_ATTRIBUTES
   {
      public LUID Luid;
      public UInt32 Attributes;
   }

   public struct TOKEN_PRIVILEGES
   {
      public UInt32 PrivilegeCount;
      [MarshalAs( UnmanagedType.ByValArray, SizeConst = Constants.ANYSIZE_ARRAY )]
      public LUID_AND_ATTRIBUTES[] Privileges;
   }
}
// ReSharper restore InconsistentNaming
