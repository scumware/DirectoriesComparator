using System;
using System.Runtime;
using System.Runtime.InteropServices.ComTypes;

namespace DirectoriesComparator.Win32
{
   public static class Utils
   {
      [TargetedPatchingOptOut( "Performance critical to inline across NGen image boundaries" )]
      public static System.Int64 ConcatenateItegers( System.Int32 p_HiPart, System.Int32 p_LowPart )
      {
         return ConcatenateItegers( (System.UInt32)p_HiPart, (System.UInt32)p_LowPart );
      }

      [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
      public static System.Int64 ConcatenateItegers(System.UInt32 p_HiPart, System.UInt32 p_LowPart)
      {
         unchecked
         {
            System.UInt64 result = (System.UInt64)p_HiPart << (sizeof( System.Int32 ) * 8);
            result |= (System.UInt64)p_LowPart;
            return (long)result;
         }
      }

      [TargetedPatchingOptOut( "Performance critical to inline across NGen image boundaries" )]
      public static System.DateTime ToDateTime( this FILETIME p_Filetime )
      {
            var fileTime = ConcatenateItegers( p_Filetime.dwHighDateTime, p_Filetime.dwLowDateTime );
            return System.DateTime.FromFileTime( fileTime );
      }

      [TargetedPatchingOptOut( "Performance critical to inline across NGen image boundaries" )]
      public static bool CompareByteArrays( byte[] p_LeftArray, byte[] p_RightArray, int p_ComparingCount = -1 )
      {
         /*
         if (p_LeftArray == null)
            throw new ArgumentNullException("p_LeftArray");
         if (p_RightArray == null)
            throw new ArgumentNullException("p_RightArray");
         if ((p_ComparingCount > p_LeftArray.Length) || (p_ComparingCount > p_RightArray.Length))
            throw new ArgumentOutOfRangeException("p_ComparingCount");
         */

         var length = (p_ComparingCount == -1) ? p_LeftArray.Length : p_ComparingCount;

         if (p_LeftArray.Length != p_RightArray.Length)
         {
            return false;
         }

         for (int i = 0; i < length; i++)
         {
            if (p_LeftArray[i] != p_RightArray[i])
               return false;
         }

         return true;
      }
   }
}
