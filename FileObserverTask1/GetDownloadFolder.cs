using System;
using System.Runtime.InteropServices;

namespace FileObserverTask1
{
    public static class GetDownloadFolder
    {
        private static Guid _folderDownloads = new Guid("374DE290-123F-4565-9164-39C4925E467B");

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern int SHGetKnownFolderPath(ref Guid id, int flags, IntPtr token, out IntPtr path);

        public static string GetFolder()
        {
            if (Environment.OSVersion.Version.Major < 6) throw new NotSupportedException();

            var pathPtr = IntPtr.Zero;

            try
            {
                SHGetKnownFolderPath(ref _folderDownloads, 0, IntPtr.Zero, out pathPtr);
                return Marshal.PtrToStringUni(pathPtr);
            }
            finally
            {
                Marshal.FreeCoTaskMem(pathPtr);
            }
        }
    }
}