using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

class SystemCleaner
{
    [DllImport("Shell32.dll", SetLastError = true)]
    static extern int SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, uint dwFlags);

    const uint SHERB_NOCONFIRMATION = 0x00000001;
    const uint SHERB_NOPROGRESSUI = 0x00000002;
    const uint SHERB_NOSOUND = 0x00000004;

    static string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "deleted_files_log.txt");

    static void Main()
    {
        Console.Write("Enter cleaning interval in hours (e.g., 3): ");
        if (!int.TryParse(Console.ReadLine(), out int intervalHours) || intervalHours <= 0)
        {
            Console.WriteLine("Invalid input. Exiting.");
            return;
        }

        TimeSpan interval = TimeSpan.FromHours(intervalHours);
        Console.WriteLine($"System Cleaner will run every {interval.TotalHours} hour(s).\n");

        while (true)
        {
            RunCleaner();
            Console.WriteLine($"\nNext cleaning in {interval.TotalHours} hour(s)...\n");
            Thread.Sleep(interval);
        }
    }

    static void RunCleaner()
    {
        Console.WriteLine("System Cleaner started...\n");
        File.AppendAllText(logPath, $"\n--- Cleaning started at {DateTime.Now} ---\n");

        long totalFreed = 0;

        totalFreed += CleanDirectory(@"C:\Windows\Temp");
        totalFreed += CleanDirectory(Path.GetTempPath());

        EmptyRecycleBin();

        Console.WriteLine($"\nTotal space cleaned: {totalFreed / (1024 * 1024)} MB");
        File.AppendAllText(logPath, $"Total Freed: {totalFreed / (1024 * 1024)} MB\n");
    }

    static long CleanDirectory(string path)
    {
        long freed = 0;

        Console.WriteLine($"Cleaning: {path}");

        try
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            foreach (FileInfo file in dir.GetFiles("*", SearchOption.AllDirectories))
            {
                try
                {
                    freed += file.Length;
                    File.AppendAllText(logPath, $"Deleted File: {file.FullName} | Size: {file.Length / 1024} KB\n");
                    file.Delete();
                }
                catch { }
            }

            foreach (DirectoryInfo subDir in dir.GetDirectories("*", SearchOption.AllDirectories))
            {
                try
                {
                    subDir.Delete(true);
                }
                catch { }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cleaning {path}: {ex.Message}");
        }

        return freed;
    }

    static void EmptyRecycleBin()
    {
        Console.WriteLine("Emptying Recycle Bin...");
        SHEmptyRecycleBin(IntPtr.Zero, null, SHERB_NOCONFIRMATION | SHERB_NOPROGRESSUI | SHERB_NOSOUND);
        File.AppendAllText(logPath, "Recycle Bin emptied.\n");
    }
}
