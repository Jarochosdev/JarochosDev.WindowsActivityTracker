using System.IO;

namespace JarochosDev.WindowsActivityTracker.Common.Logger
{
    public class TxtFileMessageLogger : IMessageLogger
    {
        public string DirectoryToLog { get; }
        public string FileNameWithoutExtension { get; }

        public TxtFileMessageLogger(string directoryToLog, string fileNameWithoutExtension)
        {
            DirectoryToLog = directoryToLog;
            FileNameWithoutExtension = fileNameWithoutExtension;
        }

        public void Log(string message)
        {
            if (!Directory.Exists(DirectoryToLog))
            {
                Directory.CreateDirectory(DirectoryToLog);
            }

            var fileNameWithPath = Path.Combine(DirectoryToLog, FileNameWithoutExtension);
            using (StreamWriter w = File.AppendText(fileNameWithPath))
            {
                w.WriteLine(message);
            }
        }
    }
}
