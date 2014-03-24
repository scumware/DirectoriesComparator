namespace DirectoriesComparator.Win32.Interop
{
   // ReSharper disable InconsistentNaming
   public enum ReparseTagType :uint
   {
      IO_REPARSE_TAG_MOUNT_POINT = (0xA0000003),
      IO_REPARSE_TAG_HSM = (0xC0000004),
      IO_REPARSE_TAG_SIS = (0x80000007),
      IO_REPARSE_TAG_DFS = (0x8000000A),
      IO_REPARSE_TAG_SYMLINK = (0xA000000C),
      IO_REPARSE_TAG_DFSR = (0x80000012),
   }
   // ReSharper restore InconsistentNaming
}