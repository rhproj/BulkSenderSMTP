namespace BulkSenderSMTP.Models
{
    public class MailModelDetails
    {
        public string LetterSubject { get; }
        public MailModelDetails(string subject)
        {
            LetterSubject = subject;
        }
    }
}
