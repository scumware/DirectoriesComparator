using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace DirectoriesComparator.Win32.Interop
{
   public sealed class SymbolicLinkInfo :ReparsePointInfo
   {
      public uint Flags;
   }

   public sealed class MountPointInfo :ReparsePointInfo { }


   public abstract class ReparsePointInfo
   {
      public string SubstituteName;
      public string PrintName;


      private unsafe void RetriveStrings(PathBufferMetadata p_PathBufferMetadata, char* p_PathBuffer)
      {
         var startIndex = p_PathBufferMetadata.PrintNameOffset / sizeof( char );
         var length = p_PathBufferMetadata.PrintNameLength / sizeof( char );
         PrintName = new String( p_PathBuffer, startIndex, length );

         startIndex = p_PathBufferMetadata.SubstituteNameOffset / sizeof( char );
         length = p_PathBufferMetadata.SubstituteNameLength / sizeof( char );
         SubstituteName = new String( p_PathBuffer, startIndex, length );
      }


      public static ReparsePointInfo GetReparsePointInfo( string p_FileName )
      {
         IntPtr hFile = (IntPtr)Constants.INVALID_HANDLE_VALUE;
         try
         {
            hFile = OpenReparsePoint(p_FileName);

            unsafe
            {
               void* bufer = stackalloc byte[MAXIMUM_REPARSE_DATA_BUFFER_SIZE];
               REPARSE_DATA_BUFFER* reparseDataBuffer = ReadReparsePointInfo(hFile, bufer);

               return TreatBuffer(reparseDataBuffer);
            }
         }
         finally
         {
            if (hFile != (IntPtr)Constants.INVALID_HANDLE_VALUE)
               FileManagementImportedFunctions.CloseHandle( hFile );
         }
      }


      private static unsafe ReparsePointInfo TreatBuffer(REPARSE_DATA_BUFFER* p_ReparseDataBuffer)
      {
         var genericReparseBuffer = p_ReparseDataBuffer->GenericReparseBuffer;
         switch (p_ReparseDataBuffer->ReparseTag)
         {
            case ReparseTagType.IO_REPARSE_TAG_SYMLINK:
               {
                  var symbolicLinkReparseBuffer = (SymbolicLinkReparseBuffer*) genericReparseBuffer;

                  ReparsePointInfo result = new SymbolicLinkInfo();
                  result.RetriveStrings(symbolicLinkReparseBuffer->PathBufferMetadata,
                                        symbolicLinkReparseBuffer->PathBuffer);

                  ((SymbolicLinkInfo) result).Flags = symbolicLinkReparseBuffer->Flags;
                  return result;
               }

            case ReparseTagType.IO_REPARSE_TAG_MOUNT_POINT:
               {
                  var mountPointReparseBuffer = (MountPointReparseBuffer*) genericReparseBuffer;

                  ReparsePointInfo result = new MountPointInfo();
                  result.RetriveStrings(mountPointReparseBuffer->PathBufferMetadata,
                                        mountPointReparseBuffer->PathBuffer);
                  return result;
               }

            default:
               throw new NotImplementedException(p_ReparseDataBuffer->ReparseTag.ToString() + " это парсить не умеем");
         }
      }


      private static unsafe REPARSE_DATA_BUFFER* ReadReparsePointInfo(IntPtr p_FileHandle, void* p_Bufer)
      {
         int pBytesReturned;
         bool success = FileManagementImportedFunctions.DeviceIoControl(
                                                                p_FileHandle,
                                                                EIOControlCode.FsctlGetReparsePoint,
                                                                IntPtr.Zero,
                                                                0,
                                                                (IntPtr) p_Bufer,
                                                                MAXIMUM_REPARSE_DATA_BUFFER_SIZE,
                                                                out pBytesReturned,
                                                                IntPtr.Zero);

         if (!success)
         {
            var win32Error = Marshal.GetLastWin32Error();
            if (win32Error != Win32Result.ERROR_SUCCESS)
            {
               throw new Win32Exception();
            }
         }

         var reparseDataBuffer = (REPARSE_DATA_BUFFER*) p_Bufer;
         return reparseDataBuffer;
      }


      private static IntPtr OpenReparsePoint(string p_FileName)
      {
         IntPtr hFile = FileManagementImportedFunctions.CreateFile(p_FileName,
                                                           EFileAccess.FILE_READ_EA,
                                                           EFileShare.Read | EFileShare.Write | EFileShare.Delete,
                                                           IntPtr.Zero,
                                                           ECreationDisposition.OpenExisting,
                                                           EFileAttributes.BackupSemantics | EFileAttributes.OpenReparsePoint,
                                                           IntPtr.Zero);
         if (hFile == (IntPtr) Constants.INVALID_HANDLE_VALUE)
         {
            throw new Win32Exception();
         }
         return hFile;
      }

// ReSharper disable InconsistentNaming  win32 constant
      private const int MAXIMUM_REPARSE_DATA_BUFFER_SIZE = 16 * 1024;
// ReSharper restore InconsistentNaming
   }
}