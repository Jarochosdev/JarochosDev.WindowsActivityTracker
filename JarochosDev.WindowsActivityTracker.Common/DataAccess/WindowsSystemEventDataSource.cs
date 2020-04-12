using System.Data;
using System.Data.SqlClient;
using JarochosDev.WindowsActivityTracker.Common.Models;

namespace JarochosDev.WindowsActivityTracker.Common.DataAccess
{
    public class WindowsSystemEventDataSource : IWindowsSystemEventDataSource
    {
        public string ConnectionString { get; }
      
        public WindowsSystemEventDataSource(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void Create(IWindowsSystemEvent windowsSystemEventEntities)
        {

            string query = "INSERT INTO dbo.WindowsSystemEvent " +
                           "(EventMessage, Type, DateTime, UserName, MachineName) " +
                           "VALUES " +
                           "(@EventMessage, @Type, @DateTime, @UserName, @MachineName) ";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, connection))
                {
                    sqlCommand.Parameters.Add("@EventMessage", SqlDbType.VarChar, 250).Value = windowsSystemEventEntities.EventMessage;
                    sqlCommand.Parameters.Add("@Type", SqlDbType.VarChar, 250).Value = windowsSystemEventEntities.Type.ToString();
                    sqlCommand.Parameters.Add("@DateTime", SqlDbType.DateTime2).Value = windowsSystemEventEntities.DateTime;
                    sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar, 250).Value = windowsSystemEventEntities.UserName;
                    sqlCommand.Parameters.Add("@MachineName", SqlDbType.VarChar, 250).Value = windowsSystemEventEntities.MachineName;

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
