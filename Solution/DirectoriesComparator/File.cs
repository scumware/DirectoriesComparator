using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using DirectoriesComparator.Win32;

namespace DirectoriesComparator
{
   public class File
   {
      private byte[] m_Hash;
      private static IEqualityComparer<File> s_fullEquality;
      private static IEqualityComparer<File> s_contentEquality;
      public string FullName { get; set; }
      public long Size { get; set; }
      public byte[] Hash
      {
         get
         {
            if (m_Hash == null)
            {
               m_Hash = CalculateHash();
            }
            return m_Hash;
         }
      }

      public static IEqualityComparer<File> FullNameComparer
      {
         get { return s_fullEquality ?? (s_fullEquality = new EqualityComparer(ComparingType.FullName)); }
      }

      public static IEqualityComparer<File> ContentEqualityComparer
      {
         get { return s_contentEquality ?? (s_contentEquality = new EqualityComparer( ComparingType.Content )); }
      }

      private byte[] CalculateHash()
      {
         using (var fs = new FileStream(FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
         {
            return HashAlgorithm.ComputeHash(fs);
         }
      }

      private static HashAlgorithm s_hashAlgorithm;

      protected static HashAlgorithm HashAlgorithm
      {
         get { return s_hashAlgorithm ?? (s_hashAlgorithm = MD5.Create()); }
      }

      public File( string p_Name, long p_Size )
      {
         FullName = p_Name;
         Size = p_Size;
      }

      [Flags]
      private enum ComparingType
      {
         Size = 1,
         WritingDate = 2,
         Name = 4,
         Content = 8,
         FullName = 16
      }

      private class EqualityComparer :IEqualityComparer<File>
      {
         private readonly ComparingType m_ComparingType;

         public EqualityComparer( ComparingType p_ComparingType )
         {
            m_ComparingType = p_ComparingType;
         }


         public bool Equals(File p_Left, File p_Right)
         {
            var leftLowerName = p_Left.FullName.ToLower();
            var rightLowerName = p_Right.FullName.ToLower();

            var result = leftLowerName == rightLowerName;

            if (result)
               return true;

            if ((m_ComparingType & ComparingType.FullName) != 0)
               return result;

            if ((m_ComparingType & ComparingType.Name) != 0)
            {
               var leftName = Path.GetFileName(leftLowerName);
               var rightName = Path.GetFileName( rightLowerName );
               
               result = leftName == rightName;
               return result;
            }

            if ((m_ComparingType & ComparingType.Content) != 0)
            {
               if (p_Left.Size != p_Right.Size)
                  return false;

               if (!Utils.CompareByteArrays(p_Left.Hash, p_Right.Hash))
                  return false;

               const int BufferSize = 4 * 1024;
               var leftBytes = new byte[BufferSize];
               var rightBytes = new byte[BufferSize];
               using (var fsLeft = new FileStream(p_Left.FullName, FileMode.Open, FileAccess.Read))
               using (var fsRight = new FileStream(p_Right.FullName, FileMode.Open, FileAccess.Read))
               {
                  int readCount;
                  while (0 != (readCount = fsLeft.Read(leftBytes, 0, BufferSize)))
                  {
                     fsRight.Read(rightBytes, 0, BufferSize);

                     var compareResult = Utils.CompareByteArrays(leftBytes, rightBytes, readCount);
                     if (!compareResult)
                        return false;
                  }
                  return true;
               }
            }

            throw new NotImplementedException();
         }

         public int GetHashCode( File p_File )
         {
            if ((m_ComparingType & ComparingType.FullName) != 0)
               return p_File.FullName.ToLower().GetHashCode();

            throw new NotImplementedException();
         }
      }

      public static int CompareBySize(File p_File1, File p_File)
      {
         var difference = p_File1.Size - p_File.Size;
         if (difference > 0) return 1;
         return difference < 0 ? -1 : 0;
      }
   }
}