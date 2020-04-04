using System.IO;

namespace JarochosDev.WindowsActivityTracker.Common.Logger
{
    public class FileMessageLogger : IMessageLogger
    {
        public string DirectoryToLog { get; }
        public string FileName { get; }

        public FileMessageLogger(string directoryToLog, string fileName)
        {
            DirectoryToLog = directoryToLog;
            FileName = fileName;
        }

        public void Log(string message)
        {
            if (!Directory.Exists(DirectoryToLog))
            {
                Directory.CreateDirectory(DirectoryToLog);
            }

            var fileNameWithPath = Path.Combine(DirectoryToLog, FileName);
            using (StreamWriter w = File.AppendText(fileNameWithPath))
            {
                w.WriteLine(message);
            }
        }
    }
}
