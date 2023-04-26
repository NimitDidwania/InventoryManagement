
using Microsoft.Extensions.Configuration;

namespace InventoryManagement.Common.Logger
{
    public class FileLogger : ICustomLogger
    {
        private readonly string? filepath;

        public FileLogger(IConfiguration configuration)
        {
            filepath = configuration["FilePath"];
        }
        public void Debug(string message)
        {
            throw new NotImplementedException();
        }

        public async Task Error( Exception ex)
        {
             await File.AppendAllTextAsync(filepath,"\n"+ DateTime.Now.ToString() + "\n" + ex.Message + "\n" +ex.Source + "\n");
        }
    }
}