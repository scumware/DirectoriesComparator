using System;
using DirectoriesComparator.CommandLine.Parser;
using DirectoriesComparator.Win32.Interop;

namespace DirectoriesComparator
{
   class Program
   {
      static void Main()
      {
         Console.WriteLine( Environment.CommandLine );

         var parameters = CommandLineParser.ParseCommandLine(Environment.CommandLine);
         parameters.Validate();

         if (parameters.Incorrect)
         {
            Console.WriteLine("Incorrect usage");
            if (!string.IsNullOrEmpty(parameters.AdditionalMessage))
            {
               Console.WriteLine( parameters.AdditionalMessage );
            }
            Console.WriteLine("use /? switch for help");
            return;
         }

         if (parameters.Empty)
         {
            Console.WriteLine( "parameters.Empty\n\ruse /? switch for help" );
            return;
         }

         if (parameters.InvestigateJunctions)
         {
            CurrentProcess.AdjustPrivileges( SecurityEntiryNames.SE_BACKUP_NAME, PrivilegeAction.Enable );
         }

         int groupNumber = 1;
         var firstPass = true;
         var equalGroups = (new FileOperations()).CompareDirectories(parameters);
         foreach (var equalFilesGroup in equalGroups)
         {
            if (firstPass)
            {
               Console.WriteLine( "Groups of equal files:" );
               firstPass = false;
            }
            else
            {
               Console.WriteLine();
            }

            Console.WriteLine( " ---- Group {0}", groupNumber );

            Console.WriteLine("Source equal files:");
            foreach (var sourceFile in equalFilesGroup.SourceFiles)
               Console.WriteLine(sourceFile.FullName);

            Console.WriteLine( "Target equal files:" );
            foreach (var targetFile in equalFilesGroup.TargetFiles)
               Console.WriteLine(targetFile.FullName);

            ++groupNumber;
         }
         Console.ReadLine();
      }
   }
}
