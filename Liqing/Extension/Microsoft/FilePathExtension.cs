using System;

namespace Csharp.Liqing.Extension.Microsoft
{
    public static class FilePathExtension
    {
        public static string GetAbsoluteFileName() {
            return System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        }

        public static string GetUsingAppDirectory() {
            try
            {
                return System.Environment.CurrentDirectory;
            }
            catch (Exception)
            {
                return System.IO.Directory.GetCurrentDirectory();
            }
        }

        public static string GetAppSetupDirecotry() {
            try
            {
                return System.AppDomain.CurrentDomain.BaseDirectory;
            }
            catch (Exception)
            {
                return System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            }

        }

    }
}
