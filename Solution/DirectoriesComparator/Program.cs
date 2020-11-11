using System;
using System.Linq;
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

         Console.WriteLine("@echo on");
   


            int groupNumber = 1;
         var firstPass = true;
         var equalGroups = (new FileOperations()).CompareDirectories(parameters);
         foreach (var equalFilesGroup in equalGroups)
         {
             const string commentString = "rem ";
             if (firstPass)
             {
                 Console.Write(commentString + "Groups of equal files:" + Environment.NewLine);
                 firstPass = false;
             }
             else
             {
                 Console.WriteLine();
             }

             Console.WriteLine(commentString + "---- Group {0}", groupNumber);

             if (parameters.TargetListSpecified)
             {
                 Console.WriteLine(commentString + "Source equal files:");
                 var firstFile = equalFilesGroup.SourceFiles.First();
                 Console.WriteLine(commentString + " ( " + SizeToFormatedString(firstFile.Size, false) + " )");

                 foreach (var sourceFile in equalFilesGroup.SourceFiles)
                     Console.WriteLine(sourceFile.FullName);

                 Console.WriteLine(commentString + "Target equal files:");
                 foreach (var targetFile in equalFilesGroup.TargetFiles)
                     Console.WriteLine(commentString + +'\"'+targetFile.FullName + '\"');
             }
             else
             {
                 Console.WriteLine(commentString + "Equal files:");
                 var firstFile = equalFilesGroup.SourceFiles.First();
                 Console.WriteLine("rem" + "  ( " + SizeToFormatedString(firstFile.Size, false) + " )");

                 foreach (var sourceFile in equalFilesGroup.SourceFiles)
                     Console.WriteLine(commentString+"del /Q " + '\"'+ sourceFile.FullName + '\"');
             }

             ++groupNumber;
         }

         Console.ReadLine();
      }

        public static string SizeToFormatedString(long bytes, bool noFractionalDigits)
        {
            if (bytes == 0)
                return 0.ToString();

            const string kilo = "KB";
            const string mega = "MB";
            const string giga = "GB";
            const string tera = "TB";
            const string peta = "PB";
            const string exa = "EB";
            const string zetta = "ZB";
            const string yotta = "YB";

            var resultValue = noFractionalDigits ? "{0:.} {1}" : "{0:.##} {1}";
            var dimensionValue = "bytes";
            var convertedValue = 0.0;


            // bytes
            if (bytes < Math.Pow(2, 10))
            {
                convertedValue = bytes;
            } // kilo
            else if (bytes < Math.Pow(2, 20))
            {
                convertedValue = bytes / Math.Pow(2, 10);
                dimensionValue = kilo;
            } // mega
            else if (bytes < Math.Pow(2, 30))
            {
                convertedValue = bytes / Math.Pow(2, 20);
                dimensionValue = mega;
            } // giga
            else if (bytes < Math.Pow(2, 40))
            {
                convertedValue = bytes / Math.Pow(2, 30);
                dimensionValue = giga;
            } // tera
            else if (bytes < Math.Pow(2, 50))
            {
                convertedValue = bytes / Math.Pow(2, 40);
                dimensionValue = tera;
            } // peta
            else if (bytes < Math.Pow(2, 60))
            {
                convertedValue = bytes / Math.Pow(2, 50);
                dimensionValue = peta;
            } // exa
            else if (bytes < Math.Pow(2, 70))
            {
                convertedValue = bytes / Math.Pow(2, 60);
                dimensionValue = exa;
            } // zetta
            else if (bytes < Math.Pow(2, 80))
            {
                convertedValue = bytes / Math.Pow(2, 70);
                dimensionValue = zetta;
            } // yotta
            else if (bytes < Math.Pow(2, 90))
            {
                convertedValue = bytes / Math.Pow(2, 80);
                dimensionValue = yotta;
            }

            return string.Format(resultValue, convertedValue, dimensionValue);
        }

    }
}
