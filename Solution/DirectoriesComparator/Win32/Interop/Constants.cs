using System;

namespace DirectoriesComparator.Win32.Interop
// ReSharper disable InconsistentNaming
{
   public static class Constants
   {
      public const int INVALID_HANDLE_VALUE = -1;


      public enum GET_FILEEX_INFO_LEVELS
      {
         GetFileExInfoStandard,
         GetFileExMaxInfoLevel
      }


      public const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
      public const int TOKEN_QUERY = 0x00000008;

      public const Int32 ANYSIZE_ARRAY = 1;
      public const Int32 MAX_PATH = 260;
   }

}
// ReSharper restore InconsistentNaming
