using System;
using System.Collections.Generic;
using System.Linq;
using DirectoriesComparator.Win32.FileSystem;

namespace DirectoriesComparator
{
   public class FileOperations
   {
      private Parameters m_Parameters;

      public ICollection<EqualFilesGroup> CompareDirectories( Parameters p_Parameters )
      {
         m_Parameters = p_Parameters;

         var sourceFiles = GetFiles(p_Parameters.SourcesList);
         var targetFiles = GetFiles( p_Parameters.TargetsList );

         return GetIntersection(sourceFiles, targetFiles);
      }

      private ICollection<EqualFilesGroup> GetIntersection(ICollection<File> p_SourceFiles,
                                                           ICollection<File> p_TargetFiles)
      {
         var result = new List<EqualFilesGroup>();

         var sourceFilesGroups = GetEqualFilesGroup(p_SourceFiles);
         var targetFilesGroups = GetEqualFilesGroup( p_TargetFiles );


         foreach (var sourceFilesGroup in sourceFilesGroups)
         {
            var sourceFile = sourceFilesGroup.First();

            for (var targetFilesGroupNode = targetFilesGroups.First; targetFilesGroupNode != null; targetFilesGroupNode = targetFilesGroupNode.Next)
            {
               var targetFilesGroup = targetFilesGroupNode.Value;
               var targetFile = targetFilesGroup.First();

               if (targetFile.Size == sourceFile.Size)
               {
                  if (File.ContentEqualityComparer.Equals(targetFile, sourceFile))
                  {
                     result.Add(new EqualFilesGroup(sourceFilesGroup, targetFilesGroup));
                     targetFilesGroups.Remove(targetFilesGroupNode);
                  }

                  break;
               }
            }
         }

         return result;
      }

      private LinkedList<IList<File>> GetEqualFilesGroup( ICollection<File> p_SourceFiles )
      {
         var result = new LinkedList<IList<File>>();

         var sortedSorceFiles = new SortedArray<File>( p_SourceFiles, File.CompareBySize );
         var equalSizeFilesGroups = sortedSorceFiles.GetEqualItemsGroups();

         foreach (var equalSizeFilesGroup in equalSizeFilesGroups)
         {
            var filesGroup = new LinkedList<File>( equalSizeFilesGroup );

            while (null != filesGroup.First)
            {
               var file = filesGroup.First.Value;
               filesGroup.RemoveFirst();

               var equalFilesGroup = new List<File>();
               equalFilesGroup.Add(file);
               result.AddLast( equalFilesGroup );

               for (var node = filesGroup.First; node != null; node = node.Next)
               {
                  bool equalityResult = false;
                  try
                  {
                     equalityResult = File.ContentEqualityComparer.Equals(node.Value, file);
                  }
                  catch (System.UnauthorizedAccessException ex)
                  {
                     if (m_Parameters.ShowWarnings)
                     {
                        Console.WriteLine(ex.Message);
                     }

                     if (!m_Parameters.IgnoreInaccessible)
                     {
                        throw;
                     }
                  }
                  catch (System.IO.IOException ex)
                  {
                     if (m_Parameters.ShowWarnings)
                     {
                        Console.WriteLine( ex.Message );
                     }

                     if (!m_Parameters.IgnoreInaccessible)
                     {
                        throw;
                     }
                  }


                  if (equalityResult)
                  {
                     equalFilesGroup.Add(node.Value);
                     filesGroup.Remove(node);
                  }
               }
            }
         }
         return result;
      }

      private ICollection<File> GetFiles(IEnumerable<string> p_DirList)
      {
         ICollection<File> fileList = new List<File>();
         foreach (var dirName in p_DirList)
         {
            var sourceI = GetFiles(dirName);
            fileList = MergeFilesByName(fileList, sourceI);
         }
         return fileList;
      }

      private static ICollection<File> MergeFilesByName( IEnumerable<File> p_Sources1, IEnumerable<File> p_Sources2 )
      {
         var hashSet = new HashSet<File>( File.FullNameComparer );

         hashSet.UnionWith(p_Sources1);
         hashSet.UnionWith( p_Sources2 );

         return hashSet;
      }

      public IEnumerable<File> GetFiles(string p_Source)
      {
         var entiryInfo = EntryInfo.GetInfo(p_Source);

         var result = new List<File>();

         if ((entiryInfo is EntryInfo.Inaccessible) || (entiryInfo is EntryInfo.NotExistent))
         {
            return result;
         }

         var fileInfo = entiryInfo as FileInfo;
         if (fileInfo != null)
         {
            var file = new File( fileInfo.FileName, fileInfo.Size );
            result.Add( file );
            return result;
         }

         var directoryInfo = entiryInfo as DirectoryInfo;
         if (directoryInfo != null)
         {
            return GetFiles( directoryInfo );
         }


         if (m_Parameters.InvestigateJunctions)
         {
            throw new NotImplementedException();
         }
         return result;
      }


      private IEnumerable<File> GetFiles( DirectoryInfo p_DirectoryInfo )
      {
         var result = new List<File>();

         var fileInfos = p_DirectoryInfo.GetAllFiles(!m_Parameters.InvestigateJunctions, m_Parameters.IgnoreInaccessible);
         foreach (var fileInfo in fileInfos)
         {
            var file = new File(fileInfo.FileName, fileInfo.Size);
            result.Add(file);
         }
         return result;
      }
   }
}
