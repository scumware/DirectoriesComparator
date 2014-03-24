using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace DirectoriesComparator.Win32.Interop
{
   public enum PrivilegeAction
   {
      Enable,
      Disable
   }

   public static class CurrentProcess
   {
      public static void AdjustPrivileges( string p_PrivilegeName, PrivilegeAction p_PrivilegeAction = PrivilegeAction.Enable)
      {
         var token = IntPtr.Zero;
         var tokenPrivileges = new TOKEN_PRIVILEGES { Privileges = new LUID_AND_ATTRIBUTES[1] };

         try
         {
            bool success;
            int win32Error;

            var process = GetCurrentProcess();
            success = SucurityImportedFunctions.OpenProcessToken( process, Constants.TOKEN_ADJUST_PRIVILEGES | Constants.TOKEN_QUERY, out token );
            if (!success)
            {
               win32Error = Marshal.GetLastWin32Error();
               if (win32Error != Win32Result.ERROR_SUCCESS)
               {
                  throw new Win32Exception();
               }
            }

            // null for local system
            success = SucurityImportedFunctions.LookupPrivilegeValue( null, p_PrivilegeName, out tokenPrivileges.Privileges[0].Luid );
            if (!success)
            {
               win32Error = Marshal.GetLastWin32Error();
               if (win32Error != Win32Result.ERROR_SUCCESS)
               {
                  throw new Win32Exception();
               }
            }

            tokenPrivileges.PrivilegeCount = 1;

            
            switch (p_PrivilegeAction)
            {
               case PrivilegeAction.Enable:
                  tokenPrivileges.Privileges[0].Attributes = SecurityEntiryNames.SE_PRIVILEGE_ENABLED;
                  break;
               case PrivilegeAction.Disable:
                  tokenPrivileges.Privileges[0].Attributes = 0;
                  break;
               default:
                  throw new ArgumentOutOfRangeException("p_PrivilegeAction");
            }
            
            // ReSharper disable RedundantAssignment  Далее нас это значение интересовать не будет. За подробностями в MSDN.
            success = SucurityImportedFunctions.AdjustTokenPrivileges(
                                                                    // ReSharper restore RedundantAssignment
                                                                    token,
                                                                    false,
                                                                    ref tokenPrivileges,
                                                                    Marshal.SizeOf( tokenPrivileges ),
                                                                    IntPtr.Zero,
                                                                    IntPtr.Zero );

            win32Error = Marshal.GetLastWin32Error();
            if (win32Error != Win32Result.ERROR_SUCCESS)
            {
               throw new Win32Exception();
            }
         }
         finally
         {
            if (token != IntPtr.Zero)
            {
               FileManagementImportedFunctions.CloseHandle( token );
            }
         }
      }

      [DllImport( "kernel32.dll" )]
      public static extern IntPtr GetCurrentProcess();
   }
}