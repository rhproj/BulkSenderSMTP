namespace BulkSenderSMTP.Models
{
    public class SmtpModel
    {
        public string SmtpServer { get; }
        public int SmtpPort { get; }

        public SmtpModel(string server, int port)
        {
            SmtpServer = server;
            SmtpPort = port;
        }
    }
}
