using System.Runtime.InteropServices;

namespace DirectoriesComparator.Win32.Interop
{
   // ReSharper disable InconsistentNaming

   /// <summary>
   /// The REPARSE_DATA_BUFFER structure is used by Microsoft file systems,
   /// filters, and minifilter drivers, as well as the I/O manager, to store data for a reparse point. 
   /// 
   /// This structure can only be used for Microsoft reparse points.
   /// Third-party reparse point owners must use the REPARSE_GUID_DATA_BUFFER structure instead. 
   /// </summary>
   [StructLayout( LayoutKind.Sequential, CharSet = CharSet.Unicode )]
   public unsafe struct REPARSE_DATA_BUFFER
   {
      //Reparse point tag. Must be a Microsoft reparse point tag. 
      public ReparseTagType ReparseTag;

      //Size, in bytes, of the reparse data in the DataBuffer member. 
      public ushort ReparseDataLength;

      /*
       * Length, in bytes, of the unparsed portion of the file name pointed to by the FileName member
       * of the associated file object.
       * For more information about the FileName member, see FILE_OBJECT.
       * This member is only valid for create operations when the I/O fails with STATUS_REPARSE.
       * 
       * For all other purposes, such as setting or querying a reparse point for the reparse data, this member is treated as reserved.
       */
      public ushort Reserved;

      public fixed char GenericReparseBuffer[1];
      //public SymbolicLinkReparseBuffer SymbolicLinkReparseBuffer;
      //public MountPointReparseBuffer MountPointReparseBuffer; обединено с предыдущим
   }


   [StructLayout( LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Unicode )]
   public unsafe struct SymbolicLinkReparseBuffer
   {
      public PathBufferMetadata PathBufferMetadata;
      /*
       * Used to indicate if the given symbolic link is an absolute or relative symbolic link.
       * If Flags contains SYMLINK_FLAG_RELATIVE, the symbolic link contained in the
       * PathBuffer array (at offset SubstitueNameOffset) is processed as a relative symbolic link; otherwise,
       * it is processed as an absolute symbolic link.
      */
      public uint Flags;

      public fixed char PathBuffer[1];
   }

   public unsafe struct MountPointReparseBuffer
   {
      public PathBufferMetadata PathBufferMetadata;
      public fixed char PathBuffer[1];
   }

   public struct PathBufferMetadata
   {
      public ushort SubstituteNameOffset;
      public ushort SubstituteNameLength;
      public ushort PrintNameOffset;
      public ushort PrintNameLength;
   }


   // ReSharper restore InconsistentNaming
}