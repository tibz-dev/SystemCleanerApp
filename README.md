✨ Features

🕒 Automated Cleaning – User-defined interval (e.g., every 3 hours)

🗑️ Cleans System Junk – Removes files from:

		C:\Windows\Temp

		User %TEMP% directory

Recycle Bin

📄 Detailed Logging – Logs every deleted file (path + size) to deleted_files_log.txt

🔐 Requires Administrator Rights – For full cleaning access

=========================================================================

📦 How to Use
1. Clone or download the project.
2. Build and run using .NET SDK:

		dotnet run

4. Enter the cleaning interval in hours.

Leave the app running – it cleans automatically.



==========================================================================

⚙️ Requirements

1. Windows OS
2. .NET 6 or later
3. Run as Administrator for full access

✅ Example Log Output

	--- Cleaning started at 2025-05-04 12:00 PM ---
	Deleted File: C:\Windows\Temp\junk.tmp | Size: 3 KB
	Deleted File: C:\Users\YourUser\AppData\Local\Temp\cache.txt | Size: 5 KB
	Recycle Bin emptied.
	Total Freed: 18 MB

