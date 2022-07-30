namespace Ordering.Application.Models
{
    public class EmailSettings
    {
        public const string Name = "EmailSettings";
        public string ApiKey { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
    }
}
